using UnityEngine;
using LitJson;
using BackEnd;
using System.Collections.Generic;
using System;

public class BackendController : MonoBehaviourSingleton<BackendController>
{
    #region Variables
    private const string victoryDB   = "victory";
    private const string defeatDB    = "defeat";
    private const string stageDB     = "stage";
    private const string coinDB      = "coin";
    private const string levelDB     = "level";
    private const string characterDB = "character";
    private const string bgmDB       = "bgm";
    #endregion

    #region RANK
    // 유저 승리 받아오기
    public string GetUserVictoryData()
    {
        return BackendManager.Instance.GetUserRankData("victory", "N");
    }

    // 유저 패배 받아오기
    public string GetUserDefeatData()
    {
        return BackendManager.Instance.GetUserRankData("defeat", "N");
    }

    // 유저 점수 받아오기
    public string GetUserRankData()
    {
        return BackendManager.Instance.GetUserRankData("rank", "N");
    }

    // 이후 rank 점수 추가 호출

    // 승리하였을 시 호출 : +1
    public void SetVictoryIntoBackend()
    {
        Debug.Log("Update to Backend : DB " + victoryDB);
        BackendManager.Instance.GameResultIntoBackend(victoryDB, 1);
    }

    // 패배하였을 시 호출 : +1
    public void SetDefeatIntoBackend()
    {
        Debug.Log("Update to Backend : DB " + defeatDB);
        BackendManager.Instance.GameResultIntoBackend(defeatDB, 1);
    }

    #endregion

    #region USER 
    // 유저 코인 받아오기
    public string GetUserCoinData()
    {
        return BackendManager.Instance.GetUserData("character", "coin", "N");
    }

    // 유저 닉네임 받아오기
    public string GetUserNickName()
    {
        return BackendManager.Instance.GetUserData("character", "nickname", "S");
    }

    // 유저 레벨 받아오기
    public string GetUserLevel()
    {
        return BackendManager.Instance.GetUserData("character", "level", "N");
    }

    // 코인 업데이트
    public void SetCoinUpdateBackend(int coin)
    {
        Debug.Log("Update to Backend : DB " + coinDB);
        BackendManager.Instance.UserDataUpdateIntoBackend(coinDB, coin);
    }

    // 레벨 업데이트 : +1
    public void SetLevelUpdateBackend()
    {
        Debug.Log("Update to Backend : DB " + levelDB);
        BackendManager.Instance.UserDataUpdateIntoBackend(levelDB, 1);
    }


    #endregion

    #region FRIEND
    // 닉네임 검색 후 친구요청 
    public void AddFriendUpdateBackend(string nick)
    {
        Debug.Log("Update to Backend : DB Friend");
        string indate = BackendManager.Instance.FindFriend(nick);
        if (indate.ToLower().Equals("null") || indate.Equals(null))
        {
            Debug.Log("친구 닉네임이 존재하지 않습니다.");
        }

        string code = BackendManager.Instance.FriendUpdateIntoBackend(indate);
        if (BackendUtils.Instance.FriendRequestCheck(code))
        {
            Debug.Log("요청에 문제가 생겼습니다..");
        }
    }

    // 친구 요청 받은 목록 (개수, 닉네임, 식별키, 요청 시간)
    public (int, string[], string[], string[]) GetFriendList()
    {
        return BackendManager.Instance.GetFriendList();
    }

    // 친구 요청 받은 목록 (개수, 닉네임, 식별키, 요청 시간)
    public (int, string[], string[], string[]) GetReceivedFriendRequestList()
    {
        return BackendManager.Instance.FriendReceivedRequestList();
    }

    // 친구 요청 보낸 목록 (개수, 닉네임, 식별키, 요청 시간)
    public (int, string[], string[], string[]) GetSentFriendRequestList()
    {
        return BackendManager.Instance.FriendSentRequestList();
    }

    // 친구요청 수락 : indate - 위 List에서 받아 옴.
    public void AcceptFriendRequest(string indate)
    {
        BackendManager.Instance.FriendAcceptRequest(indate);
    }

    // 친구요청 거절 : indate - 위 List에서 받아 옴.
    public void RejectFriendRequest(string indate)
    {
        BackendManager.Instance.FriendRejectRequest(indate);
    }

    // 친구요청 철회 : 친구 요청을 보낸 목록에서 제거 함.
    public void RevokeFriendRequest(string indate)
    {
        BackendManager.Instance.FriendRevokeRequest(indate);
    }

    // 친구 삭제 : 친구 관계 없앰.
    public void BreakFriend(string indate)
    {
        BackendManager.Instance.FriendBreakRequest(indate);
    }
    #endregion

    #region STAGE

    // 유저의 현재 스테이지 받아오기
    public string GetUserStage()
    {
        return BackendManager.Instance.GetUserData("stage", "stage", "N");
    }

    // 스테이지를 클리어하였을 시 호출 : +1
    public void SetStageUpdateBackend()
    {
        Debug.Log("Update to Backend : DB " + stageDB);
        BackendManager.Instance.StageIncreaseIntoBackend(stageDB, 1);
    }
    #endregion

    #region ITEM

    // 유저가 구매한 캐릭터의 정보를 받아옴
    // kim_default, 0 (Key, Value)
    public Dictionary<string, int> GetCharacterDIct()
    {
        return BackendManager.Instance.GetDictCharacter();
    }

    public Dictionary<string, int> GetBgmDIct()
    {
        return BackendManager.Instance.GetDictBgm();
    }

    // 아이템 획득 : character kim_default, 1
    public void SetCharacterUpdateBackend(string name)
    {
        Debug.Log("Update to Backend : DB " + characterDB);
        bool isSuccess = BackendManager.Instance.UserCharacterGetIntoBackend(characterDB, "kim_default", 0);
        if (isSuccess)
        {
            Debug.Log(name + " : 구매 성공하였습니다."); // 구매 성공
        }
        else
        {
            Debug.Log(name + " : 구매 실패하였습니다.");// 구매 실패
        }
    }

    // 아이템 획득 : bgm song_0, 1
    public void SetBGMUpdateBackend(string name)
    {
        Debug.Log("Update to Backend : DB " + bgmDB);
        bool isSuccess = BackendManager.Instance.UserBGMGetIntoBackend(bgmDB, "song_1", 0);
        if (isSuccess)
        {
            Debug.Log(name + " : 구매 성공하였습니다."); // 구매 성공
        }
        else
        {
            Debug.Log(name + " : 구매 실패하였습니다.");// 구매 실패
        }
    }
    #endregion
}
