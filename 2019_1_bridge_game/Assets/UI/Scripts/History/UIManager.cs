using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 사용법 
// --- 현재 위치 Title Scene (이후 LogoScene에서부터 실행)
// --- 각 UI에 맞게 네이밍 UIxxxx.cs (상속 : UIControl) 
public class UIManager : MonoBehaviourSingleton<UIManager>
{
    private bool isInitialized = false;

    //메세지 UI
    //[SerializeField] private MessageUIControl messageUI;
    //[SerializeField] private PauseUIControl pauseUI;
    [SerializeField] private UIControl exitUIControl;

    //UI 기록
    private Stack<UIControl> uiHistory = new Stack<UIControl>();


    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
        Initialize();
    }

    public void Initialize()
    {

    }

    public void ShowMessage(string _message)
    {
        //messageUI.ShowMessage(_message);
    }

    void Update()
    {
        //Back키 입력 시 뒤로 가기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideAndShowPreview();
        }
    }

    //새로운 UIControl 추가 및 표시
    public void ShowNew(UIControl newUI)
    {
        newUI.OnShow();
        uiHistory.Push(newUI);
    }

    public void AddHistroy(UIControl newUI)
    {
        uiHistory.Push(newUI);
    }

    public void DeleteHistory()
    {
        uiHistory.Pop();
    }

    //이전 UIControl 숨김 후 새로운 UIControl 추가 및 표시
    public void HidePreviewAndShowNew(UIControl newUI)
    {
        if (uiHistory.Count != 0)
        {
            uiHistory.Peek().OnHide();
        }
        newUI.OnShow();
        uiHistory.Push(newUI);
    }

    //현재 UIControl 숨김 후 이전 UIControl 표시
    public void HideAndShowPreview()
    {
        if (uiHistory.Count != 0)
        {
            uiHistory.Pop().OnHide();

            if (uiHistory.Count != 0)
            {
                uiHistory.Peek().OnShow();
            }
        }
        else
        {
            //아무 UI도 표시 안되있을 경우 종료 UI 표시
            ShowNew(exitUIControl);
        }
    }

}
