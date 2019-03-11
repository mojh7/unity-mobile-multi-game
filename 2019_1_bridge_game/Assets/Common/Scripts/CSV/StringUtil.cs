using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Text;

public class StringUtil
{
    public static string NewLine(string dest)
    {
        int length = dest.Length;

        char[] charArrayDest = dest.ToCharArray();
        for (int i = 0; i < length - 1; i++)
        {
            if (charArrayDest[i].Equals('\\') && charArrayDest[i + 1].Equals('n'))
            {
                charArrayDest[i] = '\n';
                i++;
                for (int j = i; j < length - 1; j++)
                {
                    if (j != length - 1)
                    {
                        charArrayDest[j] = charArrayDest[j + 1];
                    }
                    else
                    {
                        charArrayDest[j] = '\0';
                    }
                }
                length--;

            }
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(charArrayDest);

        return sb.ToString();
    }

    public static string FormatString(string body, params object[] arg)
    {
        StringBuilder sb = new StringBuilder();

        return sb.AppendFormat(body, arg).ToString();
    }

    public string Base64UrlEncode(byte[] arg)
    {
        string s = Convert.ToBase64String(arg); // Regular base64 encoder
        s = s.Split('=')[0]; // Remove any trailing '='s
        s = s.Replace('+', '-'); // 62nd char of encoding
        s = s.Replace('/', '_'); // 63rd char of encoding
        return s;
    }

    public string Base64UrlDecode(string arg)
    {
        return "";
    }

    public static string SmallLetter(string str)
    {
        return str.ToLower();
    }

    public static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    public static bool IsValidPhoneNumber(string phone)
    {
        return Regex.IsMatch(phone, @"01\d-\d{3,4}-\d{4}");
    }

    public static bool IsValidPassWord(string pw)
    {
        //return Regex.IsMatch(pw, @"^(?=.*[a-zA-Z])(?=.*[!@#$%^*+=-])(?=.*[0-9]).{8,31}$");
        return Regex.IsMatch(pw, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{8,16}$");
    }

    public static bool IsValidNickName(string nickname)
    {
        return Regex.IsMatch(nickname, @"^(?=.*[a-zA-Z])|(?=.*[0-9])|([가-힣]).{1,16}$");
    }

    public static bool isFolderName(string name)
    {
        return Regex.IsMatch(name, @"^(?=.*[a-zA-Z])|(?=.*[0-9])|(?=.*[가-힣]).{1,8}$");
    }

    public static bool isFolder(string name)
    {
        return Regex.IsMatch(name, @"^(?=.*\/)|(?=.*\\)|(?=.*\:)$");
    }
}