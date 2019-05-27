using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBZ.Item
{
    //[CreateAssetMenu(fileName = "EffectsData", menuName = "Item/EffectsData", order = 0)]
    [System.Serializable]
    public class ItemEffectsData
    {
        // 합 연산
        public float movingSpeedModified;
        public float itemAcquisitionRangeModified;
        // 곱 연산

        // flag
        public bool canSlide;
    }
}
