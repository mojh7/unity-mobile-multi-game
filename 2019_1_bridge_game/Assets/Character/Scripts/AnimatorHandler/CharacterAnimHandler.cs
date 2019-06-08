using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 필요 애니메이션 형태와 구조에 따라서 클래스 구조 바뀔 수도 있음.

public abstract class CharacterAnimHandler : MonoBehaviour
{

    #region constants
    private const string IDLE = "IDLE";
    private const string WALK = "WALK";
    #endregion

    [SerializeField] private Animator animator;

    public virtual void Idle()
    {
        ResetAllParameter();
        animator.SetTrigger(IDLE);
    }

    public virtual void Walk()
    {
        ResetAllParameter();
        animator.SetTrigger(WALK);
    }
    
    private void ResetAllParameter()
    {
        animator.ResetTrigger(IDLE);
        //animator.ResetTrigger("attack");
        //animator.ResetTrigger("attacked");
        animator.ResetTrigger(WALK);
        //animator.ResetTrigger("run");
        //animator.SetInteger("skill", -1);
    }
}
