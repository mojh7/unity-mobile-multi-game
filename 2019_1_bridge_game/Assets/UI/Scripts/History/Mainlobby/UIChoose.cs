using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Item 선택 시 안내창
//현재는 DJ창에서만 적용
public class UIChoose : UIControl
{
    [SerializeField] private Text item;

    private void OnEnable()
    {
        Invoke("panelfalse", 1);
    }
    public void SetItem(string item)
    {
        this.item.text = item;
    }
    private void panelfalse()
    {
        UIManager.Instance.HideAndShowPreview();
    }
} 
