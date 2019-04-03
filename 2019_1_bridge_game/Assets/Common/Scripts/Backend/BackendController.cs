using UnityEngine;
using LitJson;
using BackEnd;
using System.Collections.Generic;
using System;

public class BackendController : MonoBehaviourSingleton<BackendController>
{
    private const string victoryDB = "victory";
    private const string defeatDB = "defeat";

    // 승리하였을 시 호출
    public void VictoryIntoBackend()
    {
        Debug.Log("Update to Backend : DB " + victoryDB);
        BackendManager.Instance.GameResultIntoBackend(victoryDB);
    }

    // 패배하였을 시 호출
    public void DefeatIntoBackend()
    {
        Debug.Log("Update to Backend : DB " + defeatDB);
        BackendManager.Instance.GameResultIntoBackend(defeatDB);
    }
}
