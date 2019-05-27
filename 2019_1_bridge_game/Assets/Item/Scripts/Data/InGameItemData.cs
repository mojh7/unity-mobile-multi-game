using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBZ.Item
{
    [CreateAssetMenu(fileName = "InGameItemType", menuName = "ItemAsset/InGameItemData", order = 0)]
    public class InGameItemData : ItemData
    {
        // particle 정보 따로 class, struct로 묶을 수도
        // TODO : enum으로 바꿀 수도?
        public string particleName;
        public Vector3 particleScale;
        public float particleDuration;
        [SerializeField]
        [TextArea(2, 100)]
        [Header("개발용 메모장")]
        private string memo;

        //public int GetItemId() { return itemId; }
        //public void SetItemId(int id) { itemId = id; }
        //public void SetCaster(Character caster) { this.caster = caster; }
    }
}
    
