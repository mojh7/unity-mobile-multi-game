using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISignUp : UIControl
{
    [SerializeField] private Text nnText;
    [SerializeField] private Text idText;
    [SerializeField] private Text pwText;

    // nickname, id, pw 
    public (string, string, string) GetSignUpDataNIP()
    {
        return (nnText.text, idText.text, pwText.text);
    }
}
