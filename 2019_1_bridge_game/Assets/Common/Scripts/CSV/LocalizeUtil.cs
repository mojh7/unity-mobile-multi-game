using UnityEngine;
using System.Collections.Generic;
using System;

public class LocalizeUtil
{
    private static LocalizeUtil _ins = null;
    public static LocalizeUtil Instance
    {
        get
        {
            if (_ins == null)
            {
                _ins = new LocalizeUtil();
            }
            return _ins;
        }
    }

    private string _languageCode = "";

    private Dictionary<string, string> localizeText = new Dictionary<string, string>();

    // 추후에 시작 or 로딩 화면에서 사용
    public void Initialize()
	{
        Debug.Log("Init Language File");
        SetLanguageCode(Application.systemLanguage);
    }

    // 경로에 존재하는 assetName 파일
    // key, value 값으로 읽어들임
    private void LocalizeStringFromCSV(string assetName, Dictionary<string, string> dic)
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Localize/" + assetName);

		dic.Clear();

        for (var i = 0; i < data.Count; i++)
        {
			dic.Add(data[i]["KEY"].ToString(), data[i][_languageCode].ToString());
		}

    }

    // 불러들인 value 값에 따라 번역 사용
    private void SetLanguageCode(SystemLanguage lang)
    {
        switch (lang)
        {
            case SystemLanguage.Korean:
                _languageCode = "KO";
                break;
            default:
                _languageCode = "EN";
                break;
        }

        LocalizeStringFromCSV("Localize", localizeText);
    }

    public int GetLocalizeINT(string vKey)
    {
        if (localizeText.ContainsKey(vKey) == true)
        {
            return Convert.ToInt32(localizeText[vKey]);
        }
        return 0;
    }

    public string GetLocalizeText(string vKey)
    {
		if (localizeText.ContainsKey(vKey) == true)
		{
			return localizeText[vKey];
		}
        return "";
    }
}
