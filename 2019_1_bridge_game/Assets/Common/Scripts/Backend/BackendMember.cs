using BackEnd;
using LitJson;
using System;
using UnityEngine;

public class BackendMember : MonoBehaviour
{
    [SerializeField] private UISignUp signUI;
    [SerializeField] private UILogIn loginUI;
    [SerializeField] private Title title;

    private bool isSuccess = false;
    private BackendReturnObject bro = new BackendReturnObject();

    private void Start()
    {
        if (!Backend.IsInitialized)
        {
            Backend.Initialize(BackendCallback);
        }
        else
        {
            BackendCallback();
        }
    }

    // 초기화 함수 이후에 실행되는 callback 
    private void BackendCallback()
    {
        // 초기화 성공한 경우 실행
        if (Backend.IsInitialized)
        {
            // example 
            // 버전체크 -> 업데이트 

            Debug.Log(Backend.Utils.GetServerTime());
            
            LoginWithTheBackendToken();
        }
        // 초기화 실패한 경우 실행 
        else
        {
            Debug.LogError("Backend Callback Err ");
        }
    }

    //private void SuccessSaveToken()
    //{
    //    Debug.Log("------------------Update !");
    //    Backend.BMember.SaveToken(bro);
    //    bro.Clear();
    //}

    // TODO : Error 팝업 (닉네임, 아이디, 패스워드)
    public void CustomSignUp()
    {
        Debug.Log("-------------ACustomSignUp-------------");
        var user = signUI.GetSignUpDataNIP();
        string nick = user.Item1.Trim();
        string id   = user.Item2.Trim();
        string pw   = user.Item3.Trim();

        if (!CheckNickname(nick)) return; // 팝업 호출
        if (!CheckID(id))         return; // 팝업 호출
        if (!CheckPassword(pw))   return; // 팝업 호출

        BackendReturnObject isComplete =  Backend.BMember.CustomSignUp(id, pw, "tester");

        if (!CheckSignStatusCode(isComplete.ToString())) return; // 팝업 호출

        BackendManager.Instance.SetSignupData(id, pw, nick);
        BackendManager.Instance.GameInfoInsert();
        title.LoadMainLobby();
    }

    // TODO : Error 팝업 (아이디, 패스워드)
    public void CustomLogin()
    {
        Debug.Log("-------------ACustomLogin-------------");
        var user = loginUI.GetLogInDataIP();
        string id = user.Item1.Trim();
        string pw = user.Item2.Trim();

        if (!CheckID(id))         return; // 팝업 호출
        if (!CheckPassword(pw))   return; // 팝업 호출

        Debug.Log("ID : " + id + " /PW : " + pw);

        BackendReturnObject isComplete = Backend.BMember.CustomLogin(id, pw);

       if (!CheckSignStatusCode(isComplete.ToString())) return; // 팝업 호출

        BackendManager.Instance.SetLoginData(id, pw);
        BackendManager.Instance.GetTableList();
        title.LoadMainLobby();
    }

    public void CustomCreateNickname()
    {
        BackendManager.Instance.CreateNickname();
    }

    public void LoginWithTheBackendToken()
    {
        Debug.Log("-------------LoginWithTheBackendToken-------------");
        BackendReturnObject isComplete = Backend.BMember.LoginWithTheBackendToken();
        Debug.Log(isComplete.ToString());

        if (isComplete.IsSuccess())
        {
            Debug.Log("Auto login");

            //BackendManager.Instance.SetLoginData(id, pw);
            BackendManager.Instance.GetTableList();
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

//    public void PutDeviceToken()
//    {
//        Debug.Log("-------------PutDeviceToken-------------");
//#if UNITY_ANDROID
//        try
//        {
//            Debug.Log(Backend.Android.PutDeviceToken());
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e);
//        }
//#elif  UNITY_IOS
//        Debug.Log(Backend.iOS.PutDeviceToken(isDevelopment.iosProd));
//#endif
//    }
}
