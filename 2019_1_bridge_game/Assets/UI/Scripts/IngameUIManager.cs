using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameUIManager : MonoBehaviourSingleton<InGameUIManager>
{
    #region variables
    private bool active;
    private bool controllable;
    #endregion

    #region get / set
    public bool GetControllable()
    {
        return controllable;
    }
    public bool SetControllable(bool controllable)
    {
        return this.controllable = controllable;
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        InitIngameUI();
    }
    #endregion

    #region func
    private void InitIngameUI()
    {
        active = false;
    }
    #endregion
}
