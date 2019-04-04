using BackEnd;
using LitJson;
using System;
using UnityEngine;

public class BackendMember : MonoBehaviour
{
    [SerializeField] private UISignUp signUI;
    [SerializeField] private UILogIn loginUI;
    [SerializeField] private Title title;
    [SerializeField] private UICreateNick nickUI;
    [SerializeField] private UIForgotPassword forgotUI;
    [SerializeField] private UISystemPopup popupUI;

    private string serverErrorCode = null;

    private void Start()
    {
        Backend.Initialize(BRO =>
        {
            Debug.Log("Backend.Initialize " + BRO);
            // 성공
            if (BRO.IsSuccess())
            {
                if (!Backend.Utils.GetGoogleHash().Equals("")) Debug.Log(Backend.Utils.GetGoogleHash());

                serverErrorCode = Backend.Utils.GetServerTime().GetErrorCode();
                
                LoginWithTheBackendToken();
            }
            // 실패
            else
            {
                Debug.LogError("Failed to initialize the backend");
            }
        });
    }

    public void ServerCheckToBackend()
    {
        // 이상 없음.
        if (serverErrorCode == null) return;
        //message: timeout error
        if (serverErrorCode.Equals("408"))
        {
            Backend.Utils.GetServerTime(); // or 처음 씬 재 실행
        }
    }

    // TODO : Error 팝업 (닉네임, 아이디, 패스워드)
    public void CustomSignUp()
    {
        //DeleteDeviceToken();
        //Backend.BMember.Logout();
        PlayerPrefs.DeleteKey("access_token");

        Debug.Log("-------------ACustomSignUp-------------");
        var user = signUI.GetSignUpDataNIP();
        string id   = user.Item1.Trim();
        string pw   = user.Item2.Trim();

        if (!BackendUtils.Instance.IsCheckLength(pw, 15))
        {
            popupUI.ShowSystemText("비밀번호 길이가 너무 깁니다.");
            return;
        } // 팝업 호출

        BackendReturnObject isComplete = Backend.BMember.CustomSignUp(id, pw); Debug.Log(isComplete.ToString());

        if (!BackendUtils.Instance.SignUpErrorCheck(isComplete.GetStatusCode()))
        {
            popupUI.ShowSystemText("중복된 아이디가 존재합니다.");
            signUI.AlreadyExistID();
            return;
        };

        ServerCheckToBackend();
        if (!isComplete.IsSuccess()) return;    // 서버 연동 문제 ! 경고창 생각해둘 것.

        Backend.BMember.UpdatePasswordResetEmail(id);
        Debug.Log(Backend.BMember.CreateNickname(id).ToString());

        BackendManager.Instance.SetLoginData(id, pw);
        UIManager.Instance.ShowNew(nickUI);
    }

    public void CustomNickname()
    {
        string nick = nickUI.GetNickData().Trim();

        if (BackendUtils.Instance.IsInBadWord(nick)) { popupUI.ShowSystemText("비속어가 들어가있습니다."); return; }
        if (!BackendUtils.Instance.IsCheckLength(nick, 8)) { popupUI.ShowSystemText("이름의 길이가 너무 깁니다."); return; }

        BackendReturnObject isComplete = Backend.BMember.UpdateNickname(nick);

        if (!isComplete.IsSuccess()) return;    // 서버 연동 문제 ! 경고창 생각해둘 것.

        BackendManager.Instance.GameInfoInsert();
        title.LoadMainLobby();
    }

    // TODO : Error 팝업 (아이디, 패스워드)
    public void CustomLogin()
    {
        //DeleteDeviceToken();
        //Backend.BMember.Logout();
        PlayerPrefs.DeleteKey("access_token");

        Debug.Log("-------------ACustomLogin-------------");
        var user = loginUI.GetLogInDataIP();
        string id = user.Item1.Trim();
        string pw = user.Item2.Trim();

        Debug.Log("ID : " + id + " /PW : " + pw);

        BackendReturnObject isComplete = Backend.BMember.CustomLogin(id, pw); Debug.Log(isComplete.ToString());
        if (!BackendUtils.Instance.LoginErrorCheck(isComplete.GetStatusCode()))
        {
            popupUI.ShowSystemText("존재하지 않는 아이디 이거나 비밀번호가 틀렸습니다.");
            return;
        };

        ServerCheckToBackend();
        if (!isComplete.IsSuccess()) return;

        BackendManager.Instance.GetUserInfo();
        BackendManager.Instance.SetLoginData(id, pw);
        //BackendManager.Instance.GetTableList();
        title.LoadMainLobby();
    }

    public void LoginWithTheBackendToken()
    {
        BackendReturnObject isComplete = Backend.BMember.LoginWithTheBackendToken();
        Debug.Log(isComplete.ToString());

        if (isComplete.IsSuccess())
        {
            Debug.Log("Auto login");

            //BackendManager.Instance.SetLoginData(id, pw);
            BackendManager.Instance.GetUserInfo();
            title.LoadMainLobby();
        }
    }

    public void ResetPasswordToEmail()
    {
        string id = forgotUI.GetIdData();
        BackendReturnObject isComplete = Backend.BMember.ResetPassword(id, id);
        Debug.Log(isComplete.ToString());

        forgotUI.SendingMessageShow();

        if (!BackendUtils.Instance.EmailErrorCheck(isComplete.GetStatusCode()))
        {
            popupUI.ShowSystemText("없는 아이디이거나 형식이 잘못 되었습니다.");
            return;
        };

        if (isComplete.IsSuccess())
        {
            popupUI.ShowSystemText("메일이 성공적으로 전송 되었습니다."); // 메일 전송 성공 팝업
            forgotUI.CompleteMessageShow();
        }
        else
        {
            forgotUI.FailedMessageShow();
        }
    }

    public void DeleteDeviceToken()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log(Backend.Android.DeleteDeviceToken());
#elif UNITY_IOS
        Debug.Log(Backend.iOS.DeleteDeviceToken());
#endif
    }
}
