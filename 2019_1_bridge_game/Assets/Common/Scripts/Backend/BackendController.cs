using UnityEngine;
using LitJson;
using BackEnd;
using System.Collections.Generic;
using System;

public class BackendController : MonoBehaviourSingleton<BackendController>
{
    private const string victoryDB  = "victory";
    private const string defeatDB   = "defeat";
    private const string stageDB    = "stage";
    private const string coinDB     = "coin";
    private const string levelDB    = "level";



    // 이후 rank 점수 추가 호출

    // 승리하였을 시 호출 : +1
    public void VictoryIntoBackend()
    {
        Debug.Log("Update to Backend : DB " + victoryDB);
        BackendManager.Instance.GameResultIntoBackend(victoryDB, 1);
    }

    // 패배하였을 시 호출 : +1
    public void DefeatIntoBackend()
    {
        Debug.Log("Update to Backend : DB " + defeatDB);
        BackendManager.Instance.GameResultIntoBackend(defeatDB, 1);
    }

    // 스테이지를 클리어하였을 시 호출 : +1
    public void StageUpdateBackend()
    {
        Debug.Log("Update to Backend : DB " + stageDB);
        BackendManager.Instance.StageIncreaseIntoBackend(stageDB, 1);
    }

    // 코인 업데이트
    public void CoinUpdateBackend(int coin)
    {
        Debug.Log("Update to Backend : DB " + coinDB);
        BackendManager.Instance.UserDataUpdateIntoBackend(coinDB, coin);
    }

    // 레벨 업데이트 : +1
    public void LevelUpdateBackend()
    {
        Debug.Log("Update to Backend : DB " + levelDB);
        BackendManager.Instance.UserDataUpdateIntoBackend(levelDB, 1);
    }

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
    public void GetFriendList()
    {
        var list = BackendManager.Instance.GetFriendList();
        int count = list.Item1;
        if (count <= 0)
        {
            Debug.Log("친구 목록이 없습니다."); return;
        }

        string[] nick, Indate, timeAt;
        nick    = list.Item2;
        Indate  = list.Item3;
        timeAt  = list.Item4;

        // 목록 생성 처리
    }

    // 친구 요청 받은 목록 (개수, 닉네임, 식별키, 요청 시간)
    public void GetReceivedFriendRequestList()
    {
        var list = BackendManager.Instance.FriendReceivedRequestList();
        int count = list.Item1;
        if (count <= 0)
        {
            Debug.Log("친구 요청을 받은 목록이 없습니다."); return;
        }

        string[] nick, Indate, timeAt;
        nick    = list.Item2;
        Indate  = list.Item3;
        timeAt  = list.Item4;

        // 목록 생성 처리
    }

    // 친구 요청 보낸 목록 (개수, 닉네임, 식별키, 요청 시간)
    public void GetSentFriendRequestList()
    {
        var list = BackendManager.Instance.FriendSentRequestList();
        int count = list.Item1;
        if (count <= 0)
        {
            Debug.Log("친구 요청을 보낸 목록이 없습니다."); return;
        }

        string[] nick, Indate, timeAt;
        nick    = list.Item2;
        Indate  = list.Item3;
        timeAt  = list.Item4;

        // 목록 생성 처리
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
}
