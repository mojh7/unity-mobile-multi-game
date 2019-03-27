using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogIn : UIControl
{
    [SerializeField] private InputField idText;
    [SerializeField] private InputField pwText;

    // id, pw 
    public (string, string) GetLogInDataIP()
    {
        return (idText.text, pwText.text);
    }
}
