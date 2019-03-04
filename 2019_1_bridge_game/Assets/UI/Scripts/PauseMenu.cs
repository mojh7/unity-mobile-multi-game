using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region variables
    [SerializeField]
    private GameObject pauseUIObj;
    #endregion

    #region get / set
    #endregion

    #region unityFunc
    private void Awake()
    {
        pauseUIObj.SetActive(false);
    }
    #endregion

    #region func
    public void TogglePauseMenu()
    {
        if (pauseUIObj.activeSelf)
        {
            TimeController.Instance.StartTime();
        }
        else
        {
            TimeController.Instance.StopTime();
        }
        pauseUIObj.SetActive(!pauseUIObj.activeSelf);
    }

    public void ReturnTitle()
    {
        GameManager.Instance.LoadNextScene(GameScene.TITLE, true);
    }
    #endregion

    #region coroutine
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
