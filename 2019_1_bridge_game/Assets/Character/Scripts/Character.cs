using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner.CharacterInfo;
using Photon.Pun;
using Photon.Realtime;

namespace UBZ.Owner
{
    namespace CharacterInfo
    {
        public enum OwnerType
        {
            PLAYER, ENEMY, OBJECT, PET
        }

        //public enum State
        //{
        //    DIE, ALIVE
        //}

        //public enum DamageImmune
        //{
        //    NONE, ALL
        //}

        // TODO : -1 값 설정하면 다중 enum everyThing 이랑 겹치는지 테스트
        public enum AbnormalImmune
        {
            ALL = 0x11111111,
            STUN = 0x00000001,
        }

        public enum BehaviorState
        {
            DASH = 0x00000001,
        }

        public enum EmoticonType
        {
            SMILE,
            SAD,
            FRUSTRATION,
            QUESTION_MARK
        }

        //public enum AimType
        //{
        //    AUTO, SEMIAUTO, MANUAL
        //}
    }
    
    // TODO : Single, Multi game에 따른 클래스 설계 및 구현 다르게 해야되서 고민 좀 해야됨.

    public abstract class Character : MonoBehaviour
    {
        #region constants
        // TODO : 상수만 나중에 따로 다른 곳에 옮길 수도
        protected const string DASH = "Dash";
        protected const string DISPLAY_EFFECT = "DisplayEffect";
        protected readonly static StatusEffectInfo DASH_INFO = new StatusEffectInfo() { stun = 1f };
        #endregion
        
        #region componets
        protected CharacterComponents Components;
        protected AbnormalComponents abnormalComponents;
        protected SpriteRenderer spriteRenderer;
        protected Transform spriteTransform;
        protected Transform nickNameTransform;
        protected Transform abnormalStatusTransform;
        protected CircleCollider2D interactiveCollider2D;
        //protected AnimationHandler animationHandler;
        //protected BuffManager buffManager;
        protected Rigidbody2D rgbody;
        //protected AIController aiController;
        protected Transform shadowTransform;
        protected Transform bodyTransform;

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return spriteRenderer;
            }
        }
        #endregion

        #region variables
        [SerializeField] protected float movingSpeed;     // Character move Speed
        [SerializeField] protected Sprite sprite;
        protected CharacterInfo.AbnormalImmune abnormalImmune;
        protected CharacterInfo.OwnerType ownerType;
        protected Vector3 directionVector;
        protected float directionDegree;  // 바라보는 각도(총구 방향)
        protected bool isRightDirection;    // character 방향이 우측이냐(true) 아니냐(flase = 좌측)
        protected Color baseColor;
        protected LayerMask enemyLayer;
        /// <summary> owner 좌/우 바라볼 때 spriteObject scale 조절에 쓰일 player scale, 우측 (1, 1, 1), 좌측 : (-1, 1, 1) </summary>
        protected Vector3 scaleVector;
        protected BehaviorState behaviorState;

        #region abnormalStatusVariables
        protected bool canMove;
        protected bool canBehavior;
        protected int restrictMovingCount;
        protected int restrictBehaviorCount;

        protected bool[] isControlTypeAbnormalStatuses;
        protected float[] controlTypeAbnormalStatusTime;
        protected float[] controlTypeAbnormalStatusesDurationMax;
        protected Coroutine[] controlTypeAbnormalStatusCoroutines;

        protected Coroutine checkingknockBackEnded;
        protected Coroutine checkingDashEnded;
        #endregion

        #endregion

        #region get / set
        //public CharacterComponents GetCharacterComponents()
        //{
        //    return Components;
        //}
        public virtual bool GetRightDirection()
        {
            return isRightDirection;
        }
        public virtual float GetDirDegree()
        {
            return directionDegree;
        }
        public virtual Vector3 GetDirVector()
        {
            return directionVector;
        }
        public virtual Vector3 GetPosition()
        {
            return bodyTransform.position;
        }
        public Transform GetbodyTransform()
        {
            return bodyTransform;
        }
        //public BuffManager GetBuffManager()
        //{
        //    return buffManager;
        //}
        public CharacterInfo.OwnerType GetOwnerType()
        {
            return ownerType;
        }
        #endregion

        #region unityFunc
        protected virtual void Awake()
        {
            InitStatusEffects();

            Components = GetComponent<CharacterComponents>();
            Components.Init();
            abnormalComponents = GetComponent<AbnormalComponents>();
            spriteRenderer = Components.SpriteRenderer;
            spriteTransform = Components.SpriteTransform;
            nickNameTransform = Components.NickNameText.transform;
            abnormalStatusTransform = abnormalComponents.AbnormalStatusTransform;
            interactiveCollider2D = Components.InteractiveCollider2D;
            //animationHandler = Components.AnimationHandler;
            rgbody = Components.Rigidbody2D;
            shadowTransform = Components.ShadowTransform;
            bodyTransform = GetComponent<Transform>();
        }
        #endregion

        #region func
        public virtual void Init()
        {
            behaviorState = 0;
            canMove = true;
            canBehavior = true;
        }

        protected bool IsBehavioring(BehaviorState state)
        {
            return (behaviorState & state) == state;
        }


        // TODO : skill 임시로 대시로 적용, 나중에는 캐릭터 마다 스킬 적용 해야 할 수도?
        public void OnSkill()
        {
            Dash(750, 120);
            Debug.Log("스킬(대시) 사용");
        }

        public virtual void Dash(float dashSpeed, float distance)
        {
            if (!canBehavior)
                return;
            if (IsBehavioring(BehaviorState.DASH))
            {
                StopCoroutine(checkingDashEnded);
                checkingDashEnded = StartCoroutine(CheckDashEnded(distance));
            }

            if (null == checkingDashEnded)
            {
                // TODO : 상태 추가, 제거, 아예 set하는 것들 함수화
                behaviorState = behaviorState | BehaviorState.DASH;
                checkingDashEnded = StartCoroutine(CheckDashEnded(distance));
            }
            rgbody.velocity = Vector3.zero;
            rgbody.AddForce(dashSpeed * GetDirVector());
        }

        [PunRPC]
        protected void DisplayEffect(BehaviorState behaviorState, bool canDisplay, float directionDegree)
        {
            //Debug.Log("DisplayEffect " + behaviorState + ", " + canDisplay + ", " + directionDegree);
            switch(behaviorState)
            {
                case BehaviorState.DASH:
                    Components.DashEffectObj.SetActive(canDisplay);
                    if(canDisplay)
                        Components.DashEffectObj.transform.rotation = Quaternion.Euler(0, 0, directionDegree);
                    break;
                default:
                    break;
            }
        }

        #region AbnormalStatusFunc
        protected abstract bool IsControlTypeAbnormal();

        protected void InitStatusEffects()
        {
            int controlTypeAbnormalStatusTypeLength = (int)ControlTypeAbnormalStatus.END;
            isControlTypeAbnormalStatuses = new bool[controlTypeAbnormalStatusTypeLength];
            controlTypeAbnormalStatusCoroutines = new Coroutine[controlTypeAbnormalStatusTypeLength];
            controlTypeAbnormalStatusTime = new float[controlTypeAbnormalStatusTypeLength];
            controlTypeAbnormalStatusesDurationMax = new float[controlTypeAbnormalStatusTypeLength];

            restrictMovingCount = 0;
            restrictBehaviorCount = 0;
            for (int i = 0; i < (int)ControlTypeAbnormalStatus.END; i++)
            {
                isControlTypeAbnormalStatuses[i] = false;
                controlTypeAbnormalStatusCoroutines[i] = null;
                controlTypeAbnormalStatusTime[i] = 0;
                controlTypeAbnormalStatusesDurationMax[i] = 0;
            }
        }
        #endregion



        /// <summary> 상태 이상 효과 적용 </summary>
        protected bool AbnormalChance(float appliedChance)
        {
            float chance = Random.Range(0, 1f);
            if (chance < appliedChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 이동 방해 상태 이상 갯수 증가 및 이동 AI OFF Check </summary>
        protected abstract void AddRetrictsMovingCount();
        /// <summary> 이동 방해 상태 이상 갯수 감소 및 이동 AI ON Check </summary>
        protected abstract void SubRetrictsMovingCount();
        /// <summary> 행동 방해 상태 이상 갯수 증가 및 공격 AI OFF Check </summary>
        protected abstract void AddRetrictsBehaviorCount();
        /// <summary> 행동 방해 상태 이상 갯수 감소 및 공격 AI ON Check </summary>
        protected abstract void SubRetrictsBehaviorCount();

        public void StopControlTypeAbnormalStatus(ControlTypeAbnormalStatus controlTypeAbnormalStatusType)
        {
            int type = (int)controlTypeAbnormalStatusType;
            if (false == isControlTypeAbnormalStatuses[type])
                return;
            isControlTypeAbnormalStatuses[type] = false;

            if (null != controlTypeAbnormalStatusCoroutines[type])
                StopCoroutine(controlTypeAbnormalStatusCoroutines[type]);
            controlTypeAbnormalStatusCoroutines[type] = null;

            switch (controlTypeAbnormalStatusType)
            {
                case ControlTypeAbnormalStatus.STUN:
                    abnormalComponents.StunEffect.SetActive(false);
                    SubRetrictsMovingCount();
                    SubRetrictsBehaviorCount();
                    break;
                default:
                    break;
            }
        }

        // TODO : 수정좀 해야될 듯.
        /// <summary>
        /// 행동 종료 요청이 들어온 행동 중에서 현재 행동 중이면 stop 처리
        /// </summary>
        /// <param name="stopState">종료시킬 행동, 다중 입력 가능</param>
        public virtual bool StopBehavior(BehaviorState stopState)
        {
            BehaviorState state = behaviorState & stopState;
            
            if(BehaviorState.DASH == (state & BehaviorState.DASH))
            {
                if (null != checkingDashEnded)
                {
                    StopCoroutine(checkingDashEnded);
                    checkingDashEnded = null;
                }
                behaviorState = behaviorState ^ BehaviorState.DASH;
                return true;
            }
            return false;
        }

        //protected abstract void StopControlTypeAbnormalStatus(ControlTypeAbnormalStatusType controlTypeAbnormalStatusType);

        public void ApplyStatusEffect(StatusEffectInfo statusEffectInfo)
        {
            if (null == statusEffectInfo || CharacterInfo.AbnormalImmune.ALL == abnormalImmune)
                return;

            if (0 != statusEffectInfo.knockBack)
                KnockBack(statusEffectInfo);

            if (0 < statusEffectInfo.stun)
                Stun(statusEffectInfo.stun, statusEffectInfo.stunChance);
        }

        public void Stun(float effectiveTime, float chance)
        {
            if (false == AbnormalChance(chance))
                return;

            int type = (int)ControlTypeAbnormalStatus.STUN;
            StopBehavior(BehaviorState.DASH);
            // 기존에 걸려있는 기절이 없을 때
            if (null == controlTypeAbnormalStatusCoroutines[type])
            {
                controlTypeAbnormalStatusCoroutines[type] = StartCoroutine(StunCoroutine(effectiveTime));
            }
            // 걸려있는 기절이 있을 때
            else
            {
                controlTypeAbnormalStatusesDurationMax[type] = controlTypeAbnormalStatusTime[type] + effectiveTime;
            }
        }

        public void KnockBack(float knockBack, Vector2 pos, Vector2 dir, bool positionBasedKnockBack)
        {
            StopBehavior(BehaviorState.DASH);
            // 기본 상태에서 넉백
            if (null == checkingknockBackEnded)
            {
                AddRetrictsMovingCount();
                checkingknockBackEnded = StartCoroutine(CheckKnockbackEnded());
            }

            rgbody.velocity = Vector3.zero;

            // bullet과 충돌 Object 위치 차이 기반의 넉백  
            if (positionBasedKnockBack)
            {
                rgbody.AddForce(knockBack * ((Vector2)bodyTransform.position - pos).normalized);
            }
            // bullet 방향 기반의 넉백
            else
            {
                rgbody.AddForce(knockBack * dir);
            }
        }

        public void KnockBack(StatusEffectInfo info)
        {
            // 기본 상태에서 넉백
            if (null == checkingknockBackEnded)
            {
                AddRetrictsMovingCount();
                checkingknockBackEnded = StartCoroutine(CheckKnockbackEnded());
            }

            rgbody.velocity = Vector3.zero;

            // bullet과 충돌 Object 위치 차이 기반의 넉백  
            if (info.positionBasedKnockBack)
            {
                rgbody.AddForce(info.knockBack * ((Vector2)bodyTransform.position - info.KnockBackPos).normalized);
            }
            // bullet 방향 기반의 넉백
            else
            {
                rgbody.AddForce(info.knockBack * info.KnockBackDir);
            }
        }
        #endregion

        #region collision
        [PunRPC]
        protected virtual void PunHitDash(Vector2 pos, Vector2 dir) { }
        #endregion

        #region AbnormalCoroutine
        protected abstract IEnumerator StunCoroutine(float effectiveTime);
        protected IEnumerator CheckDashEnded(float distance)
        {
            float dashDistanceTotal = 0;
            while (true)
            {
                //Debug.Log(rgbody.velocity + " | " + rgbody.velocity.magnitude + " | " + dashDistanceTotal);
                yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
                dashDistanceTotal += rgbody.velocity.magnitude;
                if (rgbody.velocity.magnitude < 1f || dashDistanceTotal >= distance)
                {
                    rgbody.velocity = Vector2.zero;
                    StopBehavior(BehaviorState.DASH);
                    break;
                }
            }
        }
        protected IEnumerator CheckKnockbackEnded()
        {
            while (true)
            {
                yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
                if (rgbody.velocity.magnitude < 0.2f)
                {
                    SubRetrictsMovingCount();
                    checkingknockBackEnded  = null;
                    break;
                }
            }
        }
        #endregion

        //public virtual CustomObject Interact()
        //{
        //    //TODO : 만약에 Enemy를 조종하게 될 경우 Enemy Class에 재정의 필요
        //    return null;
        //}
    }
}




