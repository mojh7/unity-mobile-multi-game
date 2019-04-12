using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : BehaviorButtonBase
{
    public override void OnPointerDown(PointerEventData ped)
    {
        if(CanBehavior())
        {
            player.OnSkill();
        }
    }

    protected override void UseAllCostFail()
    {
        Debug.Log("스킬 사용 실패");
    }
}
