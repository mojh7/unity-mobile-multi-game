using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemPopup : UIControl
{
    [SerializeField] private Text systemTxt;

    public void ShowSystemText(string str)
    {
        systemTxt.text = str;
        UIManager.Instance.ShowNew(this);
    }

}
