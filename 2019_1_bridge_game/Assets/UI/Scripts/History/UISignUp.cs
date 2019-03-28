using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISignUp : UIControl
{
    [SerializeField] private InputField nnText;
    [SerializeField] private InputField idText;
    [SerializeField] private InputField pwText;

    // nickname, id, pw 
    public (string, string, string) GetSignUpDataNIP()
    {
        return (nnText.text, idText.text, pwText.text);
    }
}
