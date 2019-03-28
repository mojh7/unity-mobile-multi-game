using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButler : UIControl
{
    List<string> texts = new List<string>();
    private Text text;
    int cnt = 0;

    void Start()
    {
        text = GetComponent<Text>();
        //임의 스크립트
        texts.Add("1:안녕하세요");
        texts.Add("2:반갑습니다");
        texts.Add("3:안녕히가세요");
    }
    void Update()
    {
        
    }
}
