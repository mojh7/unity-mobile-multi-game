using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBZ.Item
{
    //[CreateAssetMenu(fileName = "EffectsData", menuName = "Item/EffectsData", order = 0)]
    [System.Serializable]
    public class ItemEffectsData
    {
        public ItemTarget target;

        // 합 연산
        public float movingSpeedModified;
        public float itemGettingRangeModified;
        // 곱 연산

        // flag
        public bool canSlide;
    }
}
