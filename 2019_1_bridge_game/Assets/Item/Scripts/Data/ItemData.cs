using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UBZ.Item
{
    public enum ItemTarget { SELF, OUR_TEAM, OPPONENT_TEAM }
    public enum ItemType { CONSUMABLE, BUFF, PASSIVE }

    [System.Serializable]
    // 아이템이라 부를만 한게 다양하고, 그 다른 것이 Item이라는 이름으로 클래스를 따로 만들거나 상속 받아서
    // 구조 짤 수도 있어 보여서, 일단 인 게임 아이템들은 InGameItemData class 따로 만듬
    public abstract class ItemData : ScriptableObject
    {
        public string itemName;
        [Tooltip("화면에 표시할 설명문. 툴팁 개념")]
        [SerializeField] protected string description;

        public Sprite sprite;
        public ItemType itemType;
        // TODO : 기획에 따라 buff = InGameData, passive = OutGame item 으로 봐야 될 수도, 바뀌는 것에 따라 이 변수 어느 클래스에 놓을지 달라짐.
        public float effectiveTime;
        public ItemTarget target;
        public ItemEffectsData[] itemEffectsDataList;

        public virtual void UseItem()
        {

        }
    }
}

