using BackEnd;
using LitJson;
using System;
using UnityEngine;

public class BackendMember : MonoBehaviour
{
    [SerializeField] private UISignUp signUI;
    [SerializeField] private UILogIn loginUI;
    [SerializeField] private Title title;

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
        string nick = user.Item1.Trim();
        string id   = user.Item2.Trim();
        string pw   = user.Item3.Trim();

        if (!CheckNickname(nick)) return; // 팝업 호출
        if (!CheckID(id))         return; // 팝업 호출
        if (!CheckPassword(pw))   return; // 팝업 호출

        BackendReturnObject isComplete =  Backend.BMember.CustomSignUp(id, pw, "tester");
        Debug.Log(isComplete.ToString());

        ServerCheckToBackend();
        //if (!CheckSignStatusCode(isComplete.ToString())) return; // 팝업 호출
        if (!isComplete.IsSuccess()) return;

        BackendManager.Instance.SetSignupData(id, pw, nick);
        BackendManager.Instance.GameInfoInsert();
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

        if (!CheckID(id))         return; // 팝업 호출
        if (!CheckPassword(pw))   return; // 팝업 호출

        Debug.Log("ID : " + id + " /PW : " + pw);

        BackendReturnObject isComplete = Backend.BMember.CustomLogin(id, pw);
        Debug.Log(isComplete.ToString());

        ServerCheckToBackend();
        //if (!CheckSignStatusCode(isComplete.ToString())) return; // 팝업 호출
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

    // TODO : 닉네임 제한 설정
    private bool CheckNickname(string nick)
    {
        if (nick.Equals("")) return false;
        return true;
    }

    // TODO : 아이디 제한 설정
    private bool CheckID(string id)
    {
        if (id.Equals("")) return false;
        return true;
    }

    // TODO : 비밀번호 제한 설정
    private bool CheckPassword(string pw)
    {
        if (pw.Equals("")) return false;
        return true;
    }

    private bool CheckSignStatusCode(string code)
    {
        // 409 : 중복된 아이디
        // 401 : 비밀번호가 틀린 경우
        // 403 : 차단 당한 경우
        if (code.Equals("409")) return false;
        if (code.Equals("401")) return false;
        if (code.Equals("403")) return false;

        return true;
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
