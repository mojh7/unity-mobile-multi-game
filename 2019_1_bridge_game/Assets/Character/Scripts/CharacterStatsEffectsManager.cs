using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner;
using UBZ.Item;

// TODO : 미구현, 멀티플레이 구현이랑도 연관되서 생각 해야 됨.
public class CharacterStatsEffectsManager : MonoBehaviour
{

    public enum ItemEffectUpdateType { ADD, REMOVE }

    #region variables
    private List<ItemData> inGameItemDataList;
    private List<ItemData> buffItemDataList;
    private List<ItemData> passiveItemDataList;

    private UBZ.Owner.Character owner;
    private ItemEffectsData itemEffectsTotal;
    #endregion

    #region get / set

    /// <summary>
    /// owner 설정, owner 누구 껀지 꼭 구분 필요합니다. Init 이후의 함수 실행해주심 됨.
    /// </summary>
    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }
    #endregion

    #region initFunc
    public void Init()
    {
        inGameItemDataList = new List<ItemData>();
        buffItemDataList = new List<ItemData>();
        passiveItemDataList = new List<ItemData>();
        InitEffectsTotal();
    }

    /// <summary> 캐릭터 대상 효과 종합 초기화 </summary>
    public void InitEffectsTotal()
    {
        itemEffectsTotal = new ItemEffectsData
        {
            // 합 연산
            movingSpeedModified = 1f,
            itemGettingRangeModified = 1f,
            // 곱 연산

            // on / off 속성
            canSlide = false
        };
    }
    #endregion

    #region func
    public void AddInGameItem(ItemData itemData)
    {
        if (null == itemData)
            return;
        
        switch (itemData.itemType)
        {
            case ItemType.CONSUMABLE:
                // TODO
                break;
            case ItemType.BUFF:
                // TODO : Buff icon display ui show
                buffItemDataList.Add(itemData);
                UpdateEffectsTotal(itemData.itemEffectsDataList, ItemEffectUpdateType.ADD);
                StartCoroutine(RemoveBuffEffects(itemData));
                break;
            case ItemType.PASSIVE:
                // TODO : 계획에 따라서 인게임 아이템 = 패시브 아이템으로 보던지 아웃게임에서 코스튬, 펫 등 기타 요소로 아이템 효과 보는 것이 생기면 알맞게 수정할 예정
                inGameItemDataList.Add(itemData);
                passiveItemDataList.Add(itemData);
                UpdateEffectsTotal(itemData.itemEffectsDataList, ItemEffectUpdateType.ADD);
                break;
            default:
                break;
        }
    }

    /// <summary> 버프 제거 </summary> 
    public void RemoveItemEffect(ItemData itemData)
    {
        switch (itemData.itemType)
        {
            case ItemType.BUFF:
                buffItemDataList.Remove(itemData);
                // TODO : Buff icon display ui hide
                break;
            case ItemType.PASSIVE:
                inGameItemDataList.Remove(itemData);
                passiveItemDataList.Remove(itemData);
                break;
            default:
                break;
        }
        UpdateEffectsTotal(itemData.itemEffectsDataList, ItemEffectUpdateType.REMOVE);
    }

    public void UpdateEffectsTotal(ItemEffectsData[] itemEffectsDataList, ItemEffectUpdateType updateType)
    {
        int sign;
        bool boolSign;
        // 등록
        if (ItemEffectUpdateType.ADD == updateType)
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
        for (int i = 0; i < itemEffectsDataList.Length; i++)
        {
            // 합 연산
            itemEffectsTotal.movingSpeedModified += itemEffectsDataList[i].movingSpeedModified * sign;
            itemEffectsTotal.itemGettingRangeModified += itemEffectsDataList[i].itemGettingRangeModified * sign;

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
            if (itemEffectsDataList[i].canSlide)
                itemEffectsTotal.canSlide = boolSign;
        }
        // owner.ApplyItemEffect();
        // PassiveItemForDebug.Instance.UpdateEffectTotalValueText();
    }
    #endregion
            
    private IEnumerator RemoveBuffEffects(ItemData itemData)
    {
        float time = 0;
        yield return YieldInstructionCache.WaitForSeconds(itemData.effectiveTime);
        RemoveItemEffect(itemData);
    }
}