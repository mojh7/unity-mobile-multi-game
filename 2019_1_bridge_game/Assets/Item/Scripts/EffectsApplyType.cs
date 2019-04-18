using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO : 일단은 InGameItemData 클래스 하나로 다 퉁쳐볼 수 있게 EffectsApplyType, EffectsData를 ScrtableObject로 안했음.
// 그리고 지금 인게임에서 일정 시간 효과를 보는 버프, 디버프 효과 아이템만 존재하는데 추후 아웃게임에서 아이템 구현(ex : 코스튬, 펫 등)
// 에 따른 버프형 아이템 말고 패시브, 소모품 이런 것들 구분 지어서 클래스 짜줘야 하는데 추후 수정할 예정.

[System.Serializable]
public class EffectsApplyType
{
    public enum Target { OUR_TEAM, SELF }
    public enum Type { CONSUMABLE, BUFF, PASSIVE }
    public Target target;
    public Type type;
    public float effectiveTime;

    [SerializeField]
    public EffectsData[] effectsDataList;

    //protected bool removable;
    //protected int itemId;

    public void UseItem()
    {
        for(int i = 0; i < effectsDataList.Length; i++)
        {
            // 아이템 효과 적용
        }
    }
    //public int GetItemId() { return itemId; }
    //public void SetItemId(int id) { itemId = id; }
    //public void SetCaster(Character caster) { this.caster = caster; }
}
