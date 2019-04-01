using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIForgotPassword : UIControl
{
    [SerializeField] private InputField idText;
    [SerializeField] private Text guideText;

    private string guideMessage = "왜 까먹었어 이자식아";
    private string sendMessage = "메일을 전송중입니다.";
    private string complete = "비밀번호가 변경되었습니다.";
    private string failed = "문제가 발생하여 변경에 실패하였습니다.";

    private void OnEnable()
    {
        idText.text = "";
        guideText.text = guideMessage;
    }

    public string GetIdData()
    {
        return idText.text;
    }

    public void SendingMessageShow()
    {
        guideText.text = sendMessage;
    }

    public void CompleteMessageShow()
    {
        guideText.text = complete;
    }

    public void FailedMessageShow()
    {
        guideText.text = failed;
    }
}
