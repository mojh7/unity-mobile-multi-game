using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameUIManager : MonoBehaviourSingleton<InGameUIManager>
{
    #region variables
    private bool active;
    #endregion

    #region get / set
    public bool GetActived()
    {
        return active;
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
        active = true;
    }
    #endregion


}
