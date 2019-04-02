using UnityEngine;
using System.Collections;

/// <summary> 상태이상(CC기 포함) 효과 정보 </summary>
[System.Serializable]
public class StatusEffectInfo
{
    [Header("넉백 세기와 넉백 방식")]
    public float knockBack;
    public bool positionBasedKnockBack;
    public Vector2 KnockBackPos { get; set; }
    public Vector2 KnockBackDir { get; set; }

    [Header("기절 시간")]
    [Range(0, 10)]
    public float stun;
    [Range(0, 1)]
    public float stunChance;

    public StatusEffectInfo()
    {
        stunChance = 1f;
    }

    public StatusEffectInfo(StatusEffectInfo info)
    {
        knockBack = info.knockBack;
        positionBasedKnockBack = info.positionBasedKnockBack;

        stun = info.stun;

        stunChance = info.stunChance;
    }
}