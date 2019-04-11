using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner;

public class PlayerManager : MonoBehaviourSingleton<PlayerManager>
{
    #region variable
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerObj;
    private MultiPlayer player;

    #endregion

    #region getter
    public MultiPlayer GetPlayer()
    {
        return player;
    }
    public Vector3 GetPlayerPosition()
    {
        if (playerObj == null)
            return Vector3.zero;
        return playerObj.transform.position;
    }
    #endregion

    #region function
    public void DeletePlayer()
    {
        Destroy(playerObj);
    }

    public void SpawnPlayer()
    {
        playerObj = Instantiate(playerPrefab, new Vector3(0.3f, 0.4f), Quaternion.identity);
        player = playerObj.GetComponent<MultiPlayer>();
        player.Init();
        //// 저장된 데이터 없이 새로운 게임을 시작할 때
        //if (false == GameStateManager.Instance.GetLoadsGameData())
        //{
        //    player.InitWithPlayerData(GameDataManager.Instance.GetPlayerData(GameDataManager.Instance.GetPlayerType()));
        //    //player.InitWithPlayerData(playerDatas[0]);
        //}
        //// 저장된 데이터를 로드한 상태일 때
        //else
        //{
        //    player.InitWithPlayerData(GameDataManager.Instance.GetPlayerData());
        //}
        //GameStateManager.Instance.SetLoadsGameData(false);
        //RoomManager.Instance.FindCurrentRoom(); // 플레이어 방찾기.
    }

    public void FindPlayer()
    {
        playerObj = Instantiate(playerPrefab, new Vector3(7, 4, 0), Quaternion.identity);
        player = playerObj.GetComponent<MultiPlayer>();
        player.Init();
        //// 저장된 데이터 없이 새로운 게임을 시작할 때
        //if (false == GameStateManager.Instance.GetLoadsGameData())
        //{
        //    player.InitWithPlayerData(GameDataManager.Instance.GetPlayerData(GameDataManager.Instance.GetPlayerType()));
        //    //player.InitWithPlayerData(playerDatas[0]);
        //}
        //// 저장된 데이터를 로드한 상태일 때
        //else
        //{
        //    player.InitWithPlayerData(GameDataManager.Instance.GetPlayerData());
        //}
        //GameStateManager.Instance.SetLoadsGameData(false);
        ////RoomManager.Instance.FindCurrentRoom(); // 플레이어 방찾기
        //// bool 써서 플레이어 방찾기 예외처리하기.
    }
    #endregion
}
