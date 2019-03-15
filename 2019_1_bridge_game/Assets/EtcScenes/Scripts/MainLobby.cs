using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLobby : MonoBehaviour
{
    #region variables
    #endregion

    #region unityFunc
    private void Start()
    {
    }
    #endregion

    #region func
    private void InitTitle()
    {
    }

    public void LoadInGame()
    {
        GameManager.Instance.LoadNextScene(GameScene.IN_GAME, true);
    }
    #endregion

    #region coroutine
    #endregion

}