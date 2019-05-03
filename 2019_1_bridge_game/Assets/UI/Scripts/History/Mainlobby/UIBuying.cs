using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//item 구매 창
//도감 or 상점에서 setactive&애니메이션 제어(UIManager X)
public class UIBuying : UIControl
{
    [SerializeField] private Text itemCoin;
    [SerializeField] private Text nameText;
    [SerializeField] private UIbuysuccess buysuccess;
    [SerializeField] private UIbuysuccess buyfail;
    private int tcoin;

    //구매 아이템 정보
    public void setBuyingpanel(string name, int coin)
    {
        nameText.text = name;
        itemCoin.text = coin.ToString();
    }

    public void setBuyingpanel(string name)
    {
        nameText.text = name;
    }

    //구매 버튼 클릭 시 호출
    public void onBuysuccess()
    {
        //코인 잔액 비교
        /*
         //잔액 충분
        if(tcoin <= int.Parse(BackendController.Instance.GetUserCoinData()))
        {
            buysuccess.OnShow();
        }
        //잔액 부족
        else
        {
            buyfail.OnShow();
        }*/
        buysuccess.OnShow();
    }

    //구매창 숨기기
    public void hideBuying()
    {
        this.OnHide();
    }
}
