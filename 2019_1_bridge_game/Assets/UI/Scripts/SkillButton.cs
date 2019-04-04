using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : BehaviorButtonBase
{
    [SerializeField] private Color EMPTY_COST_COLOR;
    [SerializeField] private Color FILLED_COST_COLOR;

    public override void OnPointerDown(PointerEventData ped)
    {
        if(CanBehavior())
        {
            character.OnSkill();
        }
    }

    protected override void FillCostMax()
    {
        base.FillCostMax();
        coolTimeImage.color = FILLED_COST_COLOR;
        Debug.Log("cost 풀 충전, 스킬 사용 가능");
    }
    protected override void UseAllCost()
    {
        base.UseAllCost();
        coolTimeImage.color = EMPTY_COST_COLOR;
    }

    protected override void UseAllCostFail()
    {
        Debug.Log("스킬 사용 실패");
    }
}
