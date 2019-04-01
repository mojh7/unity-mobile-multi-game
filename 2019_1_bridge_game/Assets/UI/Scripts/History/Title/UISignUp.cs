using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISignUp : UIControl
{
    [SerializeField] private InputField idText;
    [SerializeField] private InputField pwText;
    [SerializeField] private InputField cpText;
    [SerializeField] private Image idBox;
    [SerializeField] private Image pwBox;
    [SerializeField] private Image cpBox;
    [SerializeField] private GameObject idCheckIcon, idFailIcon;
    [SerializeField] private GameObject pwCheckIcon, pwFailIcon;
    [SerializeField] private GameObject cpCheckIcon, cpFailIcon;

    private void OnEnable()
    {
        idText.text = ""; pwText.text = ""; cpText.text = "";
        idBox.color = Color.black; pwBox.color = Color.black; cpBox.color = Color.black;
        idCheckIcon.SetActive(false); idFailIcon.SetActive(false);
        pwCheckIcon.SetActive(false); pwFailIcon.SetActive(false);
        cpCheckIcon.SetActive(false); cpFailIcon.SetActive(false);
    }

    // nickname, id, pw 
    public (string, string) GetSignUpDataNIP()
    {
        return (idText.text, pwText.text);
    }

    // 아이디 입력을 완료한 후
    public void CheckID()
    {
        if (!BackendUtils.Instance.IsValidEmail(idText.text))
        {
            idBox.color = Color.red; idFailIcon.SetActive(true); idCheckIcon.SetActive(false);
        }
        else
        {
            idBox.color = Color.green; idCheckIcon.SetActive(true); idFailIcon.SetActive(false);
        }
    }

    // 비밀번호를 입력 완료 한 후
    public void CheckPW()
    {
        cpText.text = ""; CheckCP();

        if (!BackendUtils.Instance.IsCheckLength(pwText.text, 15))
        {
            pwBox.color = Color.red; pwFailIcon.SetActive(true); pwCheckIcon.SetActive(false);
        }
        else
        {
            pwBox.color = Color.green; pwCheckIcon.SetActive(true); pwFailIcon.SetActive(false);
        }
    }

    // 비밀번호 재 입력을 완료 한 후
    public void CheckCP()
    {
        if (!BackendUtils.Instance.IsConfirmPassword(pwText.text, cpText.text))
        {
            cpBox.color = Color.red; cpFailIcon.SetActive(true); cpCheckIcon.SetActive(false);
        }
        else
        {
            cpBox.color = Color.green;  cpCheckIcon.SetActive(true); cpFailIcon.SetActive(false);
        }
    }

    public void AlreadyExistID()
    {
        idBox.color = Color.red;
    }
}
