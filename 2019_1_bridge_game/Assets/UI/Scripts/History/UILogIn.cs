using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogIn : UIControl
{
    [SerializeField] private Text idText;
    [SerializeField] private Text pwText;

    // id, pw 
    public (string, string) GetLogInDataIP()
    {
        return (idText.text, pwText.text);
    }
}
