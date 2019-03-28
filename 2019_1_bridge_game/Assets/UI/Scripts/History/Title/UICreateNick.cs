using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateNick : UIControl
{
    [SerializeField] private InputField nickText;

    public string GetNickData()
    {
        return nickText.text;
    }
}
