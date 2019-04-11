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
            player.ShowEmoticon((UBZ.Owner.CharacterInfo.EmoticonType)Random.Range(0,4));
        }
    }

    protected override void UseAllCostFail()
    {
        Debug.Log("이모티콘 사용 실패");
    }
}
