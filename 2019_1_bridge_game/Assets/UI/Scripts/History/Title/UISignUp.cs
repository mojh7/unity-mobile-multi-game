using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISignUp : UIControl
{
    [SerializeField] private InputField idText;
    [SerializeField] private InputField pwText;

    // nickname, id, pw 
    public (string, string) GetSignUpDataNIP()
    {
        return (idText.text, pwText.text);
    }
}
