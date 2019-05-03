using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UIBuying에서 애니메이션&setactive 제어
//Buysuccess & Buyfail 둘 다 사용
public class UIbuysuccess : UIControl
{
    [SerializeField] private bool is_on;
    [SerializeField] private UIBuying buyingpanel;

    private void OnEnable()
    {
        is_on = true;
        Invoke("panelfalse", 1);
    }
    public void panelfalse()
    {
        is_on = false;
        OnHide();
        buyingpanel.OnHide();
    }

}
