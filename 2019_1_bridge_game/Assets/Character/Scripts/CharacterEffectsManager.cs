using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner;


// TODO : 미구현, 멀티플레이 구현이랑도 연관되서 생각 해야 됨.
public class CharacterEffectsManager : MonoBehaviour
{
    public enum TargetEffectTotalUpdateType { REGISTER, REMOVE }
    // CONSUMABLE_BUFF 일단 안씀.
    public enum EffectApplyType { BUFF, PASSIVE, CONSUMABLE_BUFF }

    // 등록, 제거 여러 개 일 때 true, false 처리하기 위해서
    public enum CharacterBoolPropertyType { NONE, IS_NOT_CONSUME_STAMINA, IS_NOT_CONSUME_AMMO, END }
    public enum WeaponBoolPropertyType { NONE, END }

    #region variables
    private List<InGameItemData> inGameItemDataList;
    //private List<EffectsApplyType> passiveEffects;
    private List<InGameItemData> buffEffectsItemList;
    private UBZ.Owner.Character owner;
    private EffectsData effectsTotal;
    private int[] characterBoolPropertyCounts;
    private int[] weaponBoolPropertyCounts;
    #endregion

    #region get / set property
    //public List<EffectsApplyType> PassiveEffects
    //{
    //    get { return passiveEffects; }
    //}
    ////public int[] PassiveIds { get; private set; }
    //public List<int> PassiveIds { get; private set; }
    //public int PassiveEffectsLength
    //{
    //    get { return passiveEffects.Count; }
    //}
    public List<InGameItemData> BuffEffectsItemList
    {
        get { return buffEffectsItemList; }
    }
    public int BuffEffectsLength
    {
        get { return buffEffectsItemList.Count; }
    }
    public EffectsData EffectsData
    {
        get { return effectsTotal; }
    }
    #endregion

    #region initialization
    public void Init()
    {
        inGameItemDataList = new List<InGameItemData>();
        //passiveEffects = new List<EffectsApplyType>();
        buffEffectsItemList = new List<InGameItemData>();
        //PassiveIds = new List<int>();
        characterBoolPropertyCounts = new int[(int)CharacterBoolPropertyType.END];
        weaponBoolPropertyCounts = new int[(int)WeaponBoolPropertyType.END];
        InitEffectsTotal();
    }

    /// <summary>
    /// owner 설정, owner 누구 껀지 꼭 구분 필요합니다. Init 이후의 함수 실행해주심 됨.
    /// </summary>
    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }

    /// <summary> 캐릭터 대상 효과 종합 초기화 </summary>
    public void InitEffectsTotal()
    {
        effectsTotal = new EffectsData
        {
            // 합 연산
            movingSpeedIncrementRate = 1f,
            itemGettingRangeIncrementRate = 1f,
            // 곱 연산

            // on / off 속성
            canSlide = false
        };
    }
    #endregion

    #region function
    //public void RegisterInGameItem(InGameData data)
    //{
    //    if (typeof(MiscItemInfo) == info.GetType())
    //    {
    //        AddPassiveItme(info);
    //    }

    //    for (int i = 0; i < info.EffectApplyTypes.Length; i++)
    //    {
    //        info.EffectApplyTypes[i].SetItemId(info.GetId());
    //        info.EffectApplyTypes[i].UseItem();
    //    }
    //}

    //public void AddPassiveItme(UsableItemInfo info)
    //{
    //    if (PassiveIds.Count < 25)
    //    {
    //        PassiveIds.Add(info.GetId());
    //        PassiveItemSlot.Instance.UpdatePassiveItemUI();
    //        PassiveItemForDebug.Instance.UpdatePassiveItemUI();
    //    }
    //    else
    //    {
    //        Debug.Log("패시브 아이템 꽉참");
    //        return;
    //    }
    //}

    public void RegisterInGameItem(InGameItemData data)
    {
        if (null == data)
            return;
        inGameItemDataList.Add(data);
        //StartCoroutine(RemoveBuffEffects(data));
        for (int i = 0; i < data.effectsApplyTypeList.Length; i++)
        {
            switch(data.effectsApplyTypeList[i].type)
            {
                case EffectsApplyType.Type.CONSUMABLE:
                    break;
                case EffectsApplyType.Type.BUFF:
                    //buffEffects.Add(EffectsApplyType);
                    
                    break;
                case EffectsApplyType.Type.PASSIVE:
                    break;
                default:
                    break;
            }
            UpdateEffectsTotal(data, TargetEffectTotalUpdateType.REGISTER);
        }
    }

    /// <summary> 버프 제거 </summary> 
    public void RemoveEffects(InGameItemData data, EffectApplyType effectApplyType)
    {
        switch (effectApplyType)
        {
            case EffectApplyType.BUFF:
                //buffEffects.Remove(effectsApplyType);
                break;
            case EffectApplyType.PASSIVE:
                //passiveEffects.Remove(effectsApplyType);
                break;
            case EffectApplyType.CONSUMABLE_BUFF:
                break;
            default:
                break;
        }
        UpdateEffectsTotal(data, TargetEffectTotalUpdateType.REMOVE);
    }

    public void UpdateEffectsTotal(InGameItemData data, TargetEffectTotalUpdateType updateType)
    {
        int sign;
        bool boolSign;
        // 등록
        if (TargetEffectTotalUpdateType.REGISTER == updateType)
        {
            sign = 1;
            boolSign = true;
        }
        // 제거
        else
        {
            sign = -1;
            boolSign = false;
        }
        for (int i = 0; i < data.effectsApplyTypeList.Length; i++)
        {
            for (int j = 0; j < data.effectsApplyTypeList[i].effectsDataList.Length; j++)
            {
                // 합 연산
                effectsTotal.movingSpeedIncrementRate += data.effectsApplyTypeList[i].effectsDataList[j].movingSpeedIncrementRate * sign;
                effectsTotal.itemGettingRangeIncrementRate += data.effectsApplyTypeList[i].effectsDataList[j].itemGettingRangeIncrementRate * sign;

                // 곱 연산
                /*
                if (1 == sign)
                {
                    //CharacterTargetEffectTotal *= (1.0f - targetEffect);
                }
                else
                {
                    //CharacterTargetEffectTotal /= (1.0f - targetEffect);
                }*/

                // bool형 on / off 종류, 해당 되는 항목들은 아이템 등록시 true, 제거시 false로 total 정보를 설정 함.
                if (data.effectsApplyTypeList[i].effectsDataList[j].canSlide)
                    effectsTotal.canSlide = boolSign;
            }
        }
        // owner.ApplyItemEffect();
        // PassiveItemForDebug.Instance.UpdateEffectTotalValueText();
    }
    #endregion
            
    private IEnumerator RemoveBuffEffects(EffectsApplyType effectsApplyType)
    {
        float time = 0;
        yield return YieldInstructionCache.WaitForSeconds(effectsApplyType.effectiveTime);
        //RemoveEffects(effectsApplyType, EffectApplyType.BUFF);
    }
}