using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuying : UIControl
{
    [SerializeField] private Text itemCoin;
    [SerializeField] private Text nameText;
    [SerializeField] private UIbuysuccess buysuccess;
    [SerializeField] private UIbuysuccess buyfail;
    private int tcoin;


    public void setBuyingpanel(string name, int coin)
    {
        nameText.text = name;
        itemCoin.text = coin.ToString();
    }

    public void setBuyingpanel(string name)
    {
        nameText.text = name;
    }
    public void onBuysuccess()
    {
        //코인 잔액 비교
        /*
        if(tcoin <= int.Parse(BackendController.Instance.GetUserCoinData()))
        {
            buysuccess.OnShow();
        }
        else
        {
            buyfail.OnShow();
        }*/
        buysuccess.OnShow();
    }

    public void hideBuying()
    {
        this.OnHide();
    }
}
