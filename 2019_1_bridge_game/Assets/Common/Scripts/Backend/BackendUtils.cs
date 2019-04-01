using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class BackendUtils
{
    private static BackendUtils _ins = null;
    public static BackendUtils Instance
    {
        get
        {
            if (_ins == null)
            {
                _ins = new BackendUtils();
            }
            return _ins;
        }
    }

    private HashSet<string> wordHash = new HashSet<string>();
    private string badWordName = "badCSV";

    public void Initialize()
    {
        Debug.Log("Initialize Bad Word ");
        LoadBadWordFromCSV(badWordName);
    }

    // 특수문자가 있다면 true
    public bool IsSpecialCharacter(string input)
    {
        bool isSpecial = Regex.IsMatch(input, @"[^a-zA-Z0-9가-힣]");

        return isSpecial;
    }

    // 이메일 주소 형식
    public bool IsValidEmail(string email)
    {
        bool valid = Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");

        return valid;
    }

    // 문자열의 길이 확인
    public bool IsCheckLength(string input, int maxLen)
    {
        string inputTrim = input.Trim();

        if (inputTrim.Length == 0) return false;
        if (inputTrim.Length <= maxLen) return true;
        else                            return false;
    }

    // 나쁜 단어가 들어있다면 : true
    public bool IsInBadWord(string input)
    {
        foreach (string item in wordHash)
        {
            if (item.Equals(input)) return true;
        }
        return false;
    }

    // 비밀번호 확인 작업, 맞다면 : true
    public bool IsConfirmPassword(string pw, string confirm)
    {
        if (pw.Trim().Length == 0) return false;
        if (pw.Trim().Equals(confirm.Trim())) return true;
        else                                  return false;
    }

    public bool SignUpErrorCheck(string code)
    {
        //CustomSignUp - 중복된 customId 가 존재하는 경우
        if (code.Equals("409")) return false;
        return true;
    }

    public bool LoginErrorCheck(string code)
    {
        //CustomLogin - 존재하지 않는 아이디의 경우 or 비밀번호가 틀린 경우
        if (code.Equals("401")) return false;
        return true;
    }

    public bool EmailErrorCheck(string code)
    {
        //등록한 이메일이 없는 경우 or 잘못된 이메일을 입력한 경우
        if (code.Equals("404") || code.Equals("400")) return false;
        return true;
    }

    private void LoadBadWordFromCSV(string assetName)
    {
        wordHash = CSVReader.ReadHash("BadWord/" + assetName);
    }
}
