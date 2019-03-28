using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviourSingleton<RoomManager>
{
    public void LoadInGame()
    {
        GameManager.Instance.LoadNextScene(GameScene.IN_GAME, true);
    }
}