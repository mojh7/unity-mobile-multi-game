using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriend : UIControl
{
    [SerializeField] private Toggle friendToggle;

    // TODO 
    // 친구 버튼을 누르면
    // 토글이 active 되게 끔.
    public override void OnShow()
    {
        base.OnShow();
        //friendToggle.isOn = true;
    }
}
