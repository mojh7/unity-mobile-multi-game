using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
