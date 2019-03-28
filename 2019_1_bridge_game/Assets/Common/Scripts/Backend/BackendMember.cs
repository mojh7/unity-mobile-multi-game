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
        DeleteDeviceToken();
        Backend.BMember.Logout();

        Debug.Log("-------------ACustomSignUp-------------");
        var user = signUI.GetSignUpDataNIP();
        string id   = user.Item1.Trim();
        string pw   = user.Item2.Trim();

        if (!BackendUtils.Instance.IsCheckLength(id, 10)) { popupUI.ShowSystemText("아이디의 길이가 너무 깁니다."); return; } // 팝업 호출
        if (BackendUtils.Instance.IsSpecialCharacter(id)) { popupUI.ShowSystemText("특수문자가 포함되어 있습니다."); return; } // 팝업 호출
        if (!BackendUtils.Instance.IsCheckLength(pw, 15)) { popupUI.ShowSystemText("비밀번호 길이가 너무 깁니다."); return; } // 팝업 호출

        //BackendReturnObject isComplete =  Backend.BMember.CustomSignUp(id, pw, "tester"); Debug.Log(isComplete.ToString());

        //if (!BackendUtils.Instance.SignUpErrorCheck(isComplete.GetStatusCode())) { popupUI.ShowSystemText("중복된 아이디가 존재합니다."); return; };

        //ServerCheckToBackend();
        //if (!isComplete.IsSuccess()) return;    // 서버 연동 문제 ! 경고창 생각해둘 것.

        //Debug.Log(Backend.BMember.CreateNickname(id).ToString());

        BackendManager.Instance.SetLoginData(id, pw);
        UIManager.Instance.ShowNew(nickUI);
    }

    public void CustomNickname()
    {
        string nick = nickUI.GetNickData();

        if (BackendUtils.Instance.IsInBadWord(nick)) { popupUI.ShowSystemText("비속어가 들어가있습니다."); return; }
        if (!BackendUtils.Instance.IsCheckLength(nick, 8)) { popupUI.ShowSystemText("이름의 길이가 너무 깁니다."); return; }

        //BackendReturnObject isComplete = Backend.BMember.UpdateNickname(nick);

        //if (!isComplete.IsSuccess()) return;    // 서버 연동 문제 ! 경고창 생각해둘 것.

        //BackendManager.Instance.GameInfoInsert();
        title.LoadMainLobby();
    }

    // TODO : Error 팝업 (아이디, 패스워드)
    public void CustomLogin()
    {
        DeleteDeviceToken();
        Backend.BMember.Logout();

        Debug.Log("-------------ACustomLogin-------------");
        var user = loginUI.GetLogInDataIP();
        string id = user.Item1.Trim();
        string pw = user.Item2.Trim();

        Debug.Log("ID : " + id + " /PW : " + pw);

        BackendReturnObject isComplete = Backend.BMember.CustomLogin(id, pw); Debug.Log(isComplete.ToString());
        if (!BackendUtils.Instance.LoginErrorCheck(isComplete.GetStatusCode())) { popupUI.ShowSystemText("존재하지 않는 아이디 이거나 비밀번호가 틀렸습니다."); return; };

        ServerCheckToBackend();
        if (!isComplete.IsSuccess()) return;

        BackendManager.Instance.SetLoginData(id, pw);
        BackendManager.Instance.GetTableList();
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
            BackendManager.Instance.GetTableList();
            Backend.BMember.GetUserInfo();
            title.LoadMainLobby();
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
