using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmoticonButton : BehaviorButtonBase
{
    public override void OnPointerDown(PointerEventData ped)
    {
        if (CanBehavior())
        {
            player.ShowEmoticon((UBZ.MultiGame.Owner.CharacterInfo.EmoticonType)Random.Range(0,4));
        }
    }

    protected override void FillCostMax()
    {
        base.FillCostMax();
        coolTimeImage.color = FILLED_COST_COLOR;
        Debug.Log("cost 풀 충전, 이모티콘 사용 가능");
    }
    protected override void UseAllCost()
    {
        base.UseAllCost();
        coolTimeImage.color = EMPTY_COST_COLOR;
    }

    protected override void UseAllCostFail()
    {
        Debug.Log("이모티콘 사용 실패");
    }
}
