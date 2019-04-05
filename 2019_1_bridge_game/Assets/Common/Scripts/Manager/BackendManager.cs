using UnityEngine;
using LitJson;
using BackEnd;
using System.Collections.Generic;
using System;

public class BackendManager : MonoBehaviourSingleton<BackendManager>
{
    private string id = "";
    private string pw = "";
    private string nick = "";

    private string indate;     //gameinfo update, gameinfo delete, findOne에 사용
    private const string characterTable = "character";  // private
    private const string rankTable      = "rank";       // public 
    private const string stageTable     = "stage";      // private
    private const string itemTable      = "item";       // private

    private List<string> PublicTables = new List<string>();
    private List<string> PrivateTables = new List<string>();

    #region Get / Set
    public void SetLoginData(string id, string pw)
    {
        this.id = id; this.pw = pw;
    }

    public string ID { get { return id; } set { id = value; } }
    public string PW { get { return pw; } set { pw = value; } }
    public string Nick { get { return nick; } set { nick = value; } }
    public string Indate { get { return indate; } set { indate = value; } }
    #endregion

    // 접속 성공시 유저의 indate값
    public void GetUserInfo()
    {
        BackendReturnObject isComplete = Backend.BMember.GetUserInfo();
        if (isComplete.IsSuccess())
        {
            JsonData json = isComplete.GetReturnValuetoJSON()["row"];

            Indate = json["inDate"].ToString();
            Nick   = json["nickname"].ToString();

            Debug.Log(nick + " /" + Indate);
        }
    }

    // 닉네임 수정 
    public void UpdateNickname(string nick)
    {
        Debug.Log("-------------UpdateNickname-------------");
        Debug.Log(Backend.BMember.UpdateNickname(nick).ToString());
    }

    // 7일의 유예기간, 그 전에 로그인하면 탈퇴 철회
    public void SignOutToken(string reason)
    {
        BackendReturnObject isComplete = Backend.BMember.SignOut(reason);

        if (isComplete.IsSuccess()) UIManager.Instance.HideAndShowPreview();
        else { /*시스템 에러 팝업*/ }
    }

    // 유저의 초기 데이터베이스 생성
    public void GameInfoInsert()
    {
        Debug.Log("-----------------A GameInfo Insert-----------------");

        // 유저 기본 데이터 
        Param param = new Param();
        param.Add("id", id);
        param.Add("nickname", nick);
        param.Add("coin", 0);
        param.Add("level", 0);

        InsertGameInfo(characterTable, param);

        // 랭크 기본 데이터
        param.Clear();
        param.Add("rank", 0);
        param.Add("victory", 0);
        param.Add("defeat", 0);

        InsertGameInfo(rankTable, param);

        // 스테이지 기본 데이터
        param.Clear();
        param.Add("stage", 0);

        InsertGameInfo(stageTable, param);

        // 아이템 기본 데이터
        param.Clear();
        Dictionary<string, int> character = new Dictionary<string, int>
        {
            { "kim_default", 1 },
            { "kim_navy", 0 }
        };
        Dictionary<string, int> bgm = new Dictionary<string, int>
        {
            { "song1", 1 },
            { "song2", 0 }
        };
        param.Add("character", character);
        param.Add("bgm", bgm);

        InsertGameInfo(itemTable, param);
    }

    // 게임 정보 수정
    public void GameInfoUpdate(string table, string indate, Param param)
    {
        BackendReturnObject isComplete = Backend.GameInfo.Update(table, indate, param);

        Debug.Log(table + "update : " + isComplete.ToString());
    }

    // 게임 로그 생성 : 로그 타입, Param
    private void InsertLog(string logType, Param param)
    {
        Debug.Log("-----------------Insert Log-----------------");
        Debug.Log(Backend.GameInfo.InsertLog(logType, param).ToString());
    }

    // 데이터베이스 
    private void InsertGameInfo(string table, Param param)
    {
        BackendReturnObject isComplete =  Backend.GameInfo.Insert(table, param);

        if (isComplete.IsSuccess())
        {
            Debug.Log(table + " insert : " + isComplete.ToString());
        }
    }

    // 게임 결과 : 랭크 점수 반영
    public void GameResultIntoBackend(string key, int val)
    {
        GetUserInfo();
        BackendReturnObject isComplete = Backend.GameInfo.GetPublicContentsByGamerIndate("rank", Indate);

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

            IntoDataForJson(data, rankTable, key, val);
        }
    }

    // 스테이지 클리어
    public void StageIncreaseIntoBackend(string key, int val)
    {
        BackendReturnObject isComplete = Backend.GameInfo.GetPrivateContents("stage");

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

            IntoDataForJson(data, stageTable, key, val);
        }
    }

    // 유저 정보 (코인, 레벨) 업데이트
    public void UserDataUpdateIntoBackend(string key, int val)
    {
        BackendReturnObject isComplete = Backend.GameInfo.GetPrivateContents("character");

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

            IntoDataForJson(data, characterTable, key, val);
        }
    }

    // 친구 요청
    public string FriendUpdateIntoBackend(string indate)
    {
        BackendReturnObject isComplete = Backend.Social.Friend.RequestFriend(indate);

        if (isComplete.IsSuccess())
        {
            return isComplete.GetStatusCode();
        }

        return null;
    }

    // 친구 찾기 : 닉네임, null 이면 유저 없음
    public string FindFriend(string nick)
    {
        BackendReturnObject isComplete = Backend.Social.GetGamerIndateByNickname(nick);

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

             return data["rows"][0]["inDate"]["S"].ToString();
        }
        return null;
    }

     // 친구 목록 : 개수, 닉네임, 식별키, 요청 시간
    public (int, string[], string[], string[]) GetFriendList()
    {
        BackendReturnObject isComplete = Backend.Social.Friend.GetFriendList();

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

            return FriendDataForJson(data);
        }

        return (0, null, null, null);
    }

    // 친구 요청 받은 목록 : 개수, 닉네임, 식별키, 요청 시간
    public (int, string[], string[], string[]) FriendReceivedRequestList()
    {
        BackendReturnObject isComplete = Backend.Social.Friend.GetReceivedRequestList();

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

            return FriendDataForJson(data);
        }

        return (0, null, null, null);
    }

    // 친구 요청 보낸 목록 : 닉네임, 식별키, 요청 시간
    public (int, string[], string[], string[]) FriendSentRequestList()
    {
        BackendReturnObject isComplete = Backend.Social.Friend.GetSentRequestList();

        if (isComplete.IsSuccess())
        {
            JsonData data = isComplete.GetReturnValuetoJSON();

            return FriendDataForJson(data);
        }

        return (0, null, null, null);
    }

    // 친구 요청 철회
    public bool FriendRevokeRequest(string indate)
    {
        BackendReturnObject isComplete = Backend.Social.Friend.RevokeSentRequest(indate);

        return isComplete.IsSuccess();
    }

    // 친구 요청 수락
    public bool FriendAcceptRequest(string indate)
    {
        BackendReturnObject isComplete = Backend.Social.Friend.AcceptFriend(indate);

        return isComplete.IsSuccess();
    }

    // 친구 요청 거절
    public bool FriendRejectRequest(string indate)
    {
        BackendReturnObject isComplete = Backend.Social.Friend.RejectFriend(indate);

        return isComplete.IsSuccess();
    }

    // 친구 삭제
    public bool FriendBreakRequest(string indate)
    {
        BackendReturnObject isComplete = Backend.Social.Friend.BreakFriend(indate);

        return isComplete.IsSuccess();
    }

    private void IntoDataForJson(JsonData data, string table, string key, int val)
    {
        if (data["rows"].Count > 0)
        {
            string value = data["rows"][0][key]["N"].ToString();
            string Indate = data["rows"][0]["inDate"]["S"].ToString();

            int result = Convert.ToInt32(value) + val;
            if (result < 0) return;

            Param param = new Param();
            param.Add(key, result);

            GameInfoUpdate(table, Indate, param);
        }
        else
        {
            Debug.Log("Error : no update to victory !");
        }
    }

    private (int, string[], string[], string[]) FriendDataForJson(JsonData data)
    {
        string[] nick, Indate, timeAt;
        nick   = new string[data.Count];
        Indate = new string[data.Count];
        timeAt = new string[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            nick[i]   = data["rows"][i]["nickname"]["S"].ToString();
            Indate[i] = data["rows"][i]["inDate"]["S"].ToString();
            timeAt[i] = data["rows"][i]["createdAt"]["S"].ToString(); // "2018-12-27T04:43:18.715Z"
        }

        return (data.Count, nick, Indate, timeAt);
    }
}
