using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UBZ.MultiGame.Owner.CharacterInfo;

namespace UBZ.MultiGame.Owner
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

        public enum AbnormalImmune
        {
            NONE, ALL
        }

        //public enum AimType
        //{
        //    AUTO, SEMIAUTO, MANUAL
        //}
    }

    public abstract class Character : MonoBehaviour
    {
        #region constants
        protected static readonly Color RED_COLOR = Color.red;
        protected static readonly Color BURN_COLOR = new Color(1, 0, 0);
        protected static readonly Color FREEZE_COLOR = new Color(.7f, .7f, 1);
        protected static readonly Color POISON_COLOR = new Color(.7f, 1, .7f);
        #endregion

        #region Status
        [SerializeField] protected float movingSpeed;     // Character move Speed
        //protected CharacterInfo.DamageImmune damageImmune;
        protected CharacterInfo.AbnormalImmune abnormalImmune;
        //protected CharacterInfo.AimType aimType;
        //protected CharacterInfo.State characterState;
        protected CharacterInfo.OwnerType ownerType;
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
        [SerializeField] protected Sprite sprite;

        protected bool isDash;

        protected Vector3 directionVector;
        protected float directionDegree;  // 바라보는 각도(총구 방향)
        protected bool isRightDirection;    // character 방향이 우측이냐(true) 아니냐(flase = 좌측)

        protected Color baseColor;

        protected LayerMask enemyLayer;
        /// <summary> owner 좌/우 바라볼 때 spriteObject scale 조절에 쓰일 player scale, 우측 (1, 1, 1), 좌측 : (-1, 1, 1) </summary>
        protected Vector3 scaleVector;
        protected Vector3 fixedObjectScale;
        #endregion

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
            isDash = false;
            canMove = true;
            canBehavior = true;
        }

        // TODO : skill 임시로 대시로 적용, 나중에는 캐릭터 마다 스킬 적용 해야 할 수도?
        public void OnSkill()
        {
            Dash(750, 120);
            Debug.Log("스킬(대시) 사용");
        }

        public void Dash(float dashSpeed, float distance)
        {
            if (!canBehavior)
                return;
            if (isDash)
            {
                StopCoroutine(checkingDashEnded);
                checkingDashEnded = StartCoroutine(CheckDashEnded(distance));
            }

            if (null == checkingDashEnded)
            {
                isDash = true;
                checkingDashEnded = StartCoroutine(CheckDashEnded(distance));
            }

            rgbody.velocity = Vector3.zero;
            rgbody.AddForce(dashSpeed * GetDirVector());
        }
        #endregion

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

        protected void StopControlTypeAbnormalStatus(ControlTypeAbnormalStatus controlTypeAbnormalStatusType)
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

        //protected abstract void StopControlTypeAbnormalStatus(ControlTypeAbnormalStatusType controlTypeAbnormalStatusType);

        public void ApplyStatusEffect(StatusEffectInfo statusEffectInfo)
        {
            if (null == statusEffectInfo || CharacterInfo.AbnormalImmune.ALL == abnormalImmune)
                return;

            if (0 != statusEffectInfo.knockBack)
                KnockBack(statusEffectInfo);

            if (0 < statusEffectInfo.stun)
                Stun(statusEffectInfo);
        }

        private void Stun(StatusEffectInfo info)
        {
            if (false == AbnormalChance(info.stunChance))
                return;

            int type = (int)ControlTypeAbnormalStatus.STUN;
            //StopControlTypeAbnormalStatus(ControlTypeAbnormalStatus.CHARM);
            // 기존에 걸려있는 기절이 없을 때
            if (null == controlTypeAbnormalStatusCoroutines[type])
            {
                controlTypeAbnormalStatusCoroutines[type] = StartCoroutine(StunCoroutine(info.stun));
            }
            // 걸려있는 기절이 있을 때
            else
            {
                controlTypeAbnormalStatusesDurationMax[type] = controlTypeAbnormalStatusTime[type] + info.stun;
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
                    checkingDashEnded = null;
                    isDash = false;
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




