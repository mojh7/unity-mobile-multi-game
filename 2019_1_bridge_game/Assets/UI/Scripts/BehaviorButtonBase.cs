using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BehaviorButtonBase : MonoBehaviour, IPointerDownHandler
{
    #region variables
    [SerializeField] protected Image blackLayerImg;
    [SerializeField] protected Image coolTimeDisplayImg;
    [SerializeField] protected Text coolTimeDisplayTxt;
    [SerializeField] protected bool hasCoolTime;
    [SerializeField] protected float costFullRecoveryTime;
    protected float cost; // 0~1f
    private Coroutine displayCostCoroutine;
    protected UBZ.Owner.MultiPlayer player;
    #endregion

    #region get / set
    public void SetPlayer(UBZ.Owner.MultiPlayer player)
    {
        Debug.Log(Time.time);
        this.player = player;
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        Init();
    }
    #endregion

    #region func
    protected virtual void Init()
    {
        FillCostMax();
    }

    public abstract void OnPointerDown(PointerEventData ped);

    protected bool CanBehavior()
    {
        if (false == InGameUIManager.Instance.GetControllable())
            return false;

        if (cost < costFullRecoveryTime)
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

    protected void FillCostMax()
    {
        cost = costFullRecoveryTime;
        coolTimeDisplayImg.fillAmount = 1f-cost;
        blackLayerImg.enabled = false;
        coolTimeDisplayTxt.text = string.Empty;
    }
    public void UseAllCost()
    {
        cost = 0f;
        if(null == displayCostCoroutine)
            displayCostCoroutine = StartCoroutine(DisplayCost());
    }
    // TODO : 실패 시 휴대폰 진동 or 각 버튼에 맞게 알림 같은 것 추가?
    protected abstract void UseAllCostFail();
    #endregion


    #region coroutine
    protected virtual IEnumerator DisplayCost()
    {
        blackLayerImg.enabled = true;
        while (true)
        {
            if(costFullRecoveryTime - cost > 1.2f)
            {
                coolTimeDisplayTxt.text = Mathf.CeilToInt(costFullRecoveryTime - cost).ToString();
            }
            else if(costFullRecoveryTime - cost > 1f)
            {
                coolTimeDisplayTxt.text = "1";
            }
            else
            {
                coolTimeDisplayTxt.text = (((int)((costFullRecoveryTime - cost)*10))/10f).ToString();
            }
            coolTimeDisplayImg.fillAmount = 1-(cost / costFullRecoveryTime);
            yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
            cost += Time.fixedDeltaTime;
            if(cost >= costFullRecoveryTime)
            {
                FillCostMax();
                break;
            }
        }
        displayCostCoroutine = null;
    }
    #endregion
}
