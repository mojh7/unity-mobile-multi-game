using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace UBZ.MultiGame.Owner
{
    public class Player : Character
    {
        #region constants
        #endregion

        #region components
        [SerializeField] private PlayerController controller;    // 플레이어 컨트롤 관련 클래스
        #endregion

        #region variables
        private Photon.Pun.UtilityScripts.PunTeams team;

        private Transform objTransform;

        private PhotonView photonView;
        private new Rigidbody rigidbody;
        private new Collider collider;
        private new Renderer renderer;
        #endregion

        #region get / set
        #endregion

        #region unityFunc
        protected override void Awake()
        {
            base.Awake();
            photonView = photonView = GetComponent<PhotonView>();
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            renderer = GetComponent<Renderer>();
            objTransform = GetComponent<Transform>();
            scaleVector = new Vector3(1f, 1f, 1f);
            isRightDirection = true;
        }

        private void Start()
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                //r.material.color = InGame.GetPlayerColor(photonView.Owner.GetPlayerNumber());
            }
        }

        void Update()
        {
            if (false == InGameUIManager.Instance.GetControllable())
                return;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash(700f, 60f);
            }

            if(null != bodyTransform)
                spriteRenderer.sortingOrder = -Mathf.RoundToInt(bodyTransform.position.y * 100);
            if (isDash)
                return;

            if(photonView.IsMine)
            {
                directionVector = controller.GetMoveRecentNormalInputVector();
                directionDegree = directionVector.GetDegFromVector();
                if (-90 <= directionDegree && directionDegree < 90)
                {
                    isRightDirection = true;
                    scaleVector.x = 1f;
                    spriteTransform.localScale = scaleVector;
                }
                else
                {
                    isRightDirection = false;
                    scaleVector.x = -1f;
                    spriteTransform.localScale = scaleVector;
                }
            }
        }

        void FixedUpdate()
        {
            Move();
        }
        #endregion

        #region initialzation
        private void InitController()
        {
            ControllerUI.Instance.SetPlayer(this, ref controller);
        }

        // FIXME : 이제 로컬이 아닌 서버에서 닉네임을 받아드릴 것임.
        public override void Init()
        {
            base.Init();
            characterState = CharacterInfo.State.ALIVE;
            ownerType = CharacterInfo.OwnerType.PLAYER;
            damageImmune = CharacterInfo.DamageImmune.NONE;
            abnormalImmune = CharacterInfo.AbnormalImmune.NONE;
            directionVector = new Vector3(1, 0, 0);

            Transform baseZoneTransform = null;

            if (PunTeams.Team.RED == photonView.Owner.GetTeam())
            {
                Components.SpriteRenderer.color = Color.red;
                baseZoneTransform = InGameManager.Instance.GetRedTeamBaseZone();
                gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
                Components.HitBox.gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            }
            else if (PunTeams.Team.BLUE == photonView.Owner.GetTeam())
            {
                Components.SpriteRenderer.color = Color.blue;
                baseZoneTransform = InGameManager.Instance.GetBlueTeamBaseZone();
                gameObject.layer = LayerMask.NameToLayer(InGameManager.BLUE_TEAM_PLAYER);
                Components.HitBox.gameObject.layer = LayerMask.NameToLayer(InGameManager.BLUE_TEAM_PLAYER);
            }
            else
            {
                Components.SpriteRenderer.color = Color.black;
            }

            if (photonView.IsMine)
            {
                CameraController.Instance.AttachObject(this.transform); // get Camera
                //baseColor = Color.white;
                Components.DirectionArrow.SetBaseTown(baseZoneTransform);
                InitController();
                //TimeController.Instance.PlayStart();
            }
            else
            {
                Components.DirectionArrow.RemoveDirectionArrow();
            }

            //Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
            //Debug.Log(PhotonNetwork.LocalPlayer.GetTeam());
            //Debug.Log(PhotonNetwork.LocalPlayer.GetScore());
            //Debug.Log(PhotonNetwork.LocalPlayer.GetNext());
            //gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            //Components.InteractiveCollider2D.gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            //Components.HitBox.gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            //textMesh.text = GameDataManager.Instance.userData.GetNickname();

            //animationHandler.Init(this, PlayerManager.Instance.runtimeAnimator);

            //Debug.Log("hpMax : " + hpMax);
        }

        #endregion

        #region func
        public bool IsMine()
        {
            return photonView.IsMine;
        }

        //public override CustomObject Interact()
        //{
        //    float bestDistance = interactiveCollider2D.radius;
        //    Collider2D bestCollider = null;

        //    Collider2D[] collider2D = Physics2D.OverlapCircleAll(bodyTransform.position, interactiveCollider2D.radius, (1 << 1) | (1 << 9));

        //    for (int i = 0; i < collider2D.Length; i++)
        //    {
        //        if (!collider2D[i].GetComponent<CustomObject>().GetAvailable())
        //            continue;
        //        float distance = Vector2.Distance(bodyTransform.position, collider2D[i].transform.position);

        //        if (distance < bestDistance)
        //        {
        //            bestDistance = distance;
        //            bestCollider = collider2D[i];
        //        }
        //    }

        //    if (null == bestCollider)
        //        return null;

        //    return bestCollider.GetComponent<CustomObject>();
        //}

        // 참고 : https://you-rang.tistory.com/193?category=764030
        private void Move()
        {
            if (!canMove || isDash)
                return;

            // player 자신
            if(photonView.IsMine)
            {
                if (rgbody)
                {
                    rgbody.MovePosition(objTransform.position
                    + controller.GetMovingInputVector() * (movingSpeed) * Time.fixedDeltaTime);
                }

//#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.W))
                {
                    bodyTransform.Translate(Vector2.up * 5f * Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    bodyTransform.Translate(Vector2.down * 5f * Time.fixedDeltaTime);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    bodyTransform.Translate(Vector2.right * 5f * Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    bodyTransform.Translate(Vector2.left * 5f * Time.fixedDeltaTime);
                }
//#endif
            }
            else // 타 user player
            {

            }

            //if (controller.GetMoveInputVector().sqrMagnitude > 0.1f)
            //{
            //    animationHandler.Walk();
            //}
            //else
            //{
            //    animationHandler.Idle();
            //}
        }

        #endregion

        #region abnormalStatusFunc
        protected override bool IsControlTypeAbnormal()
        {
            return isControlTypeAbnormalStatuses[(int)ControlTypeAbnormalStatus.STUN];
        }

        // 여러 상태이상, 단일 상태이상 중첩 시 공격, 이동 제한을 한 곳에서 관리하기 위해서
        /// <summary> 이동 방해 상태 이상 갯수 증가 및 이동 AI OFF Check </summary>
        protected override void AddRetrictsMovingCount()
        {
            restrictMovingCount += 1;
            if (1 <= restrictMovingCount)
            {
                canMove = false;
            }
        }
        /// <summary> 이동 방해 상태 이상 갯수 감소 및 이동 AI ON Check </summary>
        protected override void SubRetrictsMovingCount()
        {
            restrictMovingCount -= 1;
            if (0 >= restrictMovingCount)
            {
                restrictMovingCount = 0;
                canMove = true;
            }
        }
        /// <summary> 공격 방해 상태 이상 갯수 증가 및 공격 AI OFF Check </summary>
        protected override void AddRetrictsBehaviorCount()
        {
            restrictBehaviorCount += 1;
            if (1 >= restrictBehaviorCount)
            {
                canBehavior = false;
            }
        }
        /// <summary> 공격 방해 상태 이상 갯수 감소 및 공격 AI ON Check </summary>
        protected override void SubRetrictsBehaviorCount()
        {
            restrictBehaviorCount -= 1;
            if (0 <= restrictBehaviorCount)
            {
                restrictBehaviorCount = 0;
                canBehavior = true;
            }
        }
        #endregion


        #region coroutine
        protected override IEnumerator StunCoroutine(float effectiveTime)
        {
            int type = (int)ControlTypeAbnormalStatus.STUN;
            abnormalComponents.StunEffect.SetActive(true);
            AddRetrictsMovingCount();
            AddRetrictsBehaviorCount();
            //animationHandler.Idle();
            isControlTypeAbnormalStatuses[type] = true;
            controlTypeAbnormalStatusTime[type] = 0;
            controlTypeAbnormalStatusesDurationMax[type] = effectiveTime;
            while (controlTypeAbnormalStatusTime[type] <= controlTypeAbnormalStatusesDurationMax[type])
            {
                if (abnormalImmune == CharacterInfo.AbnormalImmune.ALL)
                {
                    controlTypeAbnormalStatusesDurationMax[type] = 0;
                    break;
                }
                controlTypeAbnormalStatusTime[type] += Time.fixedDeltaTime;
                yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
            }

            StopControlTypeAbnormalStatus(ControlTypeAbnormalStatus.STUN);
        }

        #endregion
    }
}

