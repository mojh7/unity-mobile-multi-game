using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmoticonButton : BehaviorButtonBase
{
    [SerializeField] private UBZ.Owner.CharacterInfo.EmoticonType emoticonType;

    protected override bool Behavior()
    {
        ControllerUI.Instance.EmoticonButtonClicked();
        player.ShowEmoticon(emoticonType);
        return true;
    }

    protected override void UseAllCostFail()
    {
        Debug.Log("이모티콘 사용 실패");
    }
}
