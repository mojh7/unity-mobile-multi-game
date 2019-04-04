using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BehaviorButtonBase : MonoBehaviour, IPointerDownHandler
{
    #region variables
    private const float COST_MAX = 1f;
    [SerializeField] protected Image buttonImage;
    [SerializeField] protected Image coolTimeImage;
    [SerializeField] protected bool hasCoolTime;
    protected float cost; // 0~1f
    [SerializeField] protected float costFullRecoveryTime;
    protected UBZ.MultiGame.Owner.Character character;
    #endregion

    #region get / set
    public void SetPlayer(UBZ.MultiGame.Owner.Character character)
    {
        this.character = character;
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        cost = COST_MAX;
    }
    #endregion

    #region func
    public abstract void OnPointerDown(PointerEventData ped);

    protected bool CanBehavior()
    {
        if (cost < COST_MAX)
        {
            UseAllCostFail();
            return false;
        }
        else
        {
            UseAllCost();
            return true;
        }
    }

    protected virtual void FillCostMax()
    {
        cost = COST_MAX;
        coolTimeImage.fillAmount = cost;
    }
    protected virtual void UseAllCost()
    {
        cost = 0f;
        StartCoroutine(DisplayCost());
    }
    // TODO : 실패 시 휴대폰 진동 or 각 버튼에 맞게 알림 같은 것 추가?
    protected abstract void UseAllCostFail();
    #endregion


    #region coroutine
    protected virtual IEnumerator DisplayCost()
    {
        // 본래 식 COST_MAX / ( costFullRecoveryTime / Time.fixedDeltaTime), COST_MAX = 1f 이라 간소화 함.
        float RecoveryAmount = Time.fixedDeltaTime / costFullRecoveryTime;
        while (true)
        {
            coolTimeImage.fillAmount = cost;
            yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
            cost += RecoveryAmount;
            if(cost >= COST_MAX)
            {
                FillCostMax();
                break;
            }
        }
    }
    #endregion
}
