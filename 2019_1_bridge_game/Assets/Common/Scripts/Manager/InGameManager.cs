using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameManager가 전반적인 관리하고
// 각 씬당 관리하는 manager 둘 듯?

// 맵 생성 -> ui fade in, player, time 시작 하면 될 듯

public class InGameManager : MonoBehaviourSingleton<InGameManager>
{
    #region variables
    #endregion

    #region get / set
    #endregion

    #region unityFunc
    private void Awake()
    {
        //RoomSetManager.Instance.Init();
    }
    private void Start()
    {
        TimeController.Instance.StartTime();
        //GenerateMap();
        SpawnPlayer();
        //DrawUI();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //        GoUpFloor();
    //}
    #endregion


    #region Func
    //public int GetFloor()
    //{
    //    return GameDataManager.Instance.GetFloor();
    //}
    //public void GoUpFloor()
    //{
    //    GameDataManager.Instance.SetFloor();
    //    GameDataManager.Instance.Savedata(GameDataManager.UserDataType.INGAME);

    //    GameDataManager.Instance.LoadData(GameDataManager.UserDataType.INGAME);
    //    GameStateManager.Instance.SetLoadsGameData(true);
    //    GameStateManager.Instance.LoadInGame();
    //    System.GC.Collect();
    //}
    // 데이터 저장 타이밍
    //void GenerateMap()
    //{
    //    Map.MapManager.Instance.GenerateMap(GameDataManager.Instance.GetFloor()); // 맵생성
    //}
    void SpawnPlayer()
    {
        PlayerManager.Instance.SpawnPlayer(); // 플레이어 스폰
    }
    //void DrawUI()
    //{
    //    UIManager.Instance.FadeInScreen(); // 화면 밝히기
    //    MiniMap.Instance.DrawMinimap(); // 미니맵 그리기
    //}
    #endregion
}
