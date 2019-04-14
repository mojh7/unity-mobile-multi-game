using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsData", menuName = "Item/EffectsData", order = 0)]
public class EffectsData : ScriptableObject
{
    // 합 연산
    public float movingSpeedIncrementRate;

    // 곱 연산
    public float itemGettingRangeIncrementRate;

    // t or f
    public bool canSlide;
}
