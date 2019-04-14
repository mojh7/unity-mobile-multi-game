using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InGameItemType", menuName = "ItemAsset/InGameItemData", order = 0)]
public class InGameItemData : ItemData
{
    [SerializeField]
    [TextArea(3, 100)]
    [Header("개발용 메모장")]
    private string memo;
    public Sprite sprite;
    public EffectsApplyType[] effectsApplyTypeList;
    public void UseItem()
    {
        //effectsApplyTypeList.UseItem();
    }
}
