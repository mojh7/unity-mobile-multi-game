using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner;
using UBZ.Item;

// TODO : 미구현, 멀티플레이 구현이랑도 연관되서 생각 해야 됨.
// 코루틴 써야되서 모노비헤이어 상속 받아야 됨. 코루틴 따른 클래스에서 사용하게 되면 모노비헤이어 상속 안 받아도 됨
public class CharacterStatsEffectsManager : MonoBehaviour
{
    public enum ItemEffectUpdateType { ADD, REMOVE }

    #region variables
    private List<ItemData> inGameItemDataList;
    private List<ItemData> buffItemDataList;
    private List<ItemData> passiveItemDataList;

    private UBZ.Owner.Character owner;
    public ItemEffectsData ItemEffectsTotal { get; private set; }
    #endregion

    #region get / set
    public void SetOwner(Character owner)
    {
        this.owner = owner;
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        inGameItemDataList = new List<ItemData>();
        buffItemDataList = new List<ItemData>();
        passiveItemDataList = new List<ItemData>();

        ItemEffectsTotal = new ItemEffectsData
        {
            // 합 연산
            movingSpeedModified = 1f,
            itemAcquisitionRangeModified = 1f,
            // 곱 연산

            // on / off 속성
            canSlide = false
        };
    }
    #endregion

    #region func
    public void AddInGameItem(ItemData itemData, bool isMine)
    {
        if (null == itemData)
            return;
        
        if(typeof(InGameItemData) == itemData.GetType())
        {
            InGameItemData data = (InGameItemData)itemData;
            if(string.Empty != data.particleName)
            {
                ParticleManager.Instance.PlayParticle(data.particleName, Vector2.zero, data.particleScale, owner.GetbodyTransform(), data.particleDuration);
            }
        }

        switch (itemData.itemType)
        {
            case ItemType.CONSUMABLE:
                foreach (ItemEffectsData itemEffectData in itemData.itemEffectsDataList)
                    owner.ApplyConsumableItem(itemEffectData);
                break;
            case ItemType.BUFF:
                // TODO : Buff icon display ui show
                buffItemDataList.Add(itemData);
                UpdateEffectsTotal(itemData.itemEffectsDataList, ItemEffectUpdateType.ADD);
                if(isMine)
                {
                    StartCoroutine(RemoveBuffEffects(itemData));
                }
                break;
            case ItemType.PASSIVE:
                // TODO : 기획에 따라서 인게임 아이템 = 패시브 아이템으로 보던지 아웃게임에서 코스튬, 펫 등 기타 요소로 아이템 효과 보는 것이 생기면 알맞게 수정할 예정
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
            ItemEffectsTotal.movingSpeedModified += itemEffectsDataList[i].movingSpeedModified * sign;
            ItemEffectsTotal.itemAcquisitionRangeModified += itemEffectsDataList[i].itemAcquisitionRangeModified * sign;

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
            //if (itemEffectsDataList[i].canSlide)
            //    ItemEffectsTotal.canSlide = boolSign;
        }
        owner.ApplyItemEffect();
        // PassiveItemForDebug.Instance.UpdateEffectTotalValueText();
    }
    #endregion
            
    private IEnumerator RemoveBuffEffects(ItemData itemData)
    {
        yield return YieldInstructionCache.WaitForSeconds(itemData.effectiveTime);
        owner.RunOutOfBuffTime(itemData.itemName);
        //owner.RunOutOfBuffTime(itemData);
        //RemoveItemEffect(itemData);
    }
}