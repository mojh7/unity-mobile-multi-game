using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExit : UIControl
{
    public void BTN_ExitGame()
    {
#if UNITY_ANDROID
        Application.Quit();
#elif UNITY_EDITOR
        Debug.Log("EXIT !!!");
#endif
    }
}
