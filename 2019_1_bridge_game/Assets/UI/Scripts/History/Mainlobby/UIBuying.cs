using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuying : UIControl
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private UIbuysuccess buysuccess;

    private void Update()
    {
        if(buysuccess.GetIs_on() == false)
        {
            buysuccess.SetIs_on(true);
            OnHide();
        }
    }
    public void setBuyingpanel(Sprite image, string name)
    {
        this.image.sprite = image;
        nameText.text = name;
    }

    public void onBuysuccess()
    {
        buysuccess.OnShow();
    }
}
