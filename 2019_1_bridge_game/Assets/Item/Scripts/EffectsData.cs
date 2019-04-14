using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "EffectsData", menuName = "Item/EffectsData", order = 0)]
[System.Serializable]
public class EffectsData
{
    // 합 연산
    public float movingSpeedIncrementRate;
    public float itemGettingRangeIncrementRate;

    // 곱 연산

    // t or f
    public bool canSlide;
}
