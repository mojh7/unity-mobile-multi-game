using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLobby : MonoBehaviour
{
    #region variables
    [SerializeField] private FriendController friendController;
    #endregion

    #region unityFunc
    private void Start()
    {
        friendController.Initialized();
    }
    #endregion

    #region func
    public void LoadRoom()
    {
        GameManager.Instance.LoadNextScene(GameScene.ROOM, true);
    }
    #endregion

    #region coroutine
    #endregion

}