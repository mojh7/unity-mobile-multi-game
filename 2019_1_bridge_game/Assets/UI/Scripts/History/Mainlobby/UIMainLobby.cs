using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainLobby : UIControl
{
    public GameObject panel;

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
    }
    public void DisableBuysuccess()
    {
        Invoke("panelfalse", 1);
    }
    public void panelfalse()
    {
        panel.SetActive(false);
    }
}
