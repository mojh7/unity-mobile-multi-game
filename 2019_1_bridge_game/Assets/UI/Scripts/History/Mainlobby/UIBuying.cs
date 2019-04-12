using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuying : UIControl
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private UIbuysuccess buysuccess;

    public void setBuyingpanel(Sprite image, string name)
    {
        this.image.sprite = image;
        nameText.text = name;
    }
    public void setBuyingpanel(string name)
    {
        image.gameObject.SetActive(false);
        nameText.rectTransform.position = image.rectTransform.position;
        nameText.text = name;
    }

    public void onBuysuccess()
    {
        buysuccess.OnShow();
    }

    public void hideBuying()
    {
        this.OnHide();
    }
}
