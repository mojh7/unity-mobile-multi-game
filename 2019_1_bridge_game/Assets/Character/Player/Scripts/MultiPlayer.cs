using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UBZ.Owner.CharacterInfo;
using UBZ.Item;

namespace UBZ.Owner
{
    public class MultiPlayer : Character, IPunObservable
    {
        #region constants
        public const string PLAYER = "Player";
        public const string SHOW_EMOTICON = "ShowEmoticon";
        #endregion
        
        #region variables
        [SerializeField] private PlayerController controller;    // 플레이어 컨트롤 관련 클래스

        private Transform objTransform;

        private PhotonView photonView;
        private new Rigidbody rigidbody;
        private new Collider collider;
        private new Renderer renderer;
        private Vector3 currentPos;
        private bool canSlide;
        [SerializeField] private Sprite redTemaRing;
        [SerializeField] private Sprite blueTemaRing;
        #endregion

        #region get / set
        public Photon.Realtime.Player GetUser()
        {
            return photonView.Owner;
        }

        public PunTeams.Team GetTeam()
        {
            return photonView.Owner.GetTeam();
        }
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
            scaleVector = Vector3.one;
            isRightDirection = true;
        }

        void Update()
        {
            if (false == InGameUIManager.Instance.GetControllable())
                return;

            if(null != bodyTransform)
                components.SpriteRenderer.sortingOrder = -Mathf.RoundToInt(bodyTransform.position.y * 100);

            if (IsBehavioring(BehaviorState.DASH))
                return;

            if(photonView.IsMine)
            {
                directionVector = controller.GetMoveRecentNormalInputVector();
                directionDegree = directionVector.GetDegFromVector();
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Dash(700f, 60f);
                }
                //if (canMove)
                //{
                //    bodyTransform.Translate(controller.GetMovingInputVector() * movingSpeed * Time.deltaTime);
                //    //Debug.Log((1.0f / PhotonNetwork.SerializationRate) + ", " + Time.deltaTime);

                //    // for debug
                //    if (Input.GetKey(KeyCode.W))
                //    {
                //        bodyTransform.Translate(Vector2.up * movingSpeed * Time.deltaTime);
                //    }
                //    else if (Input.GetKey(KeyCode.S))
                //    {
                //        bodyTransform.Translate(Vector2.down * movingSpeed * Time.deltaTime);
                //    }
                //    if (Input.GetKey(KeyCode.D))
                //    {
                //        bodyTransform.Translate(Vector2.right * movingSpeed * Time.deltaTime);
                //    }
                //    else if (Input.GetKey(KeyCode.A))
                //    {
                //        bodyTransform.Translate(Vector2.left * movingSpeed * Time.deltaTime);
                //    }
                //}

                if (-90 <= directionDegree && directionDegree < 90)
                {
                    isRightDirection = true;
                }
                else
                {
                    isRightDirection = false;
                }
            }
            else
            {
                //끊어진 시간이 너무 길 경우(텔레포트)
                if ((bodyTransform.position - currentPos).sqrMagnitude >= 5.0f * 5.0f)
                {
                    bodyTransform.position = currentPos;
                }
                //끊어진 시간이 짧을 경우(자연스럽게 연결 - 데드레커닝)
                else
                {
                    bodyTransform.position = Vector3.Lerp(bodyTransform.position, currentPos, Time.deltaTime * 10.0f);
                }
            }

            if(isRightDirection)
            {
                scaleVector.x = 1f;
            }
            else
            {
                scaleVector.x = -1f;
            }
            components.SpriteTransform.localScale = scaleVector;
        }

        void FixedUpdate()
        {
            Move();
        }
        #endregion

        #region func

        #region initialzation
        private void InitController()
        {
            ControllerUI.Instance.SetPlayer(this, ref controller);
        }

        // FIXME : 이제 로컬이 아닌 서버에서 닉네임을 받아드릴 것임.
        public override void Init()
        {
            InGameManager.Instance.SetMultiPlayer(this);
            photonView.RPC("PlayerInit", RpcTarget.All);
            /*
            //Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
            //Debug.Log(PhotonNetwork.LocalPlayer.GetTeam());
            //Debug.Log(PhotonNetwork.LocalPlayer.GetScore());
            //Debug.Log(PhotonNetwork.LocalPlayer.GetNext());
            //gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            //Components.InteractiveCollider2D.gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            //Components.HitBox.gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
            //textMesh.text = GameDataManager.Instance.userData.GetNickname();
            //animationHandler.Init(this, PlayerManager.Instance.runtimeAnimator);
            */
        }
        
        [PunRPC]
        private void PlayerInit()
        {
            base.Init();
            ownerType = CharacterInfo.OwnerType.PLAYER;
            abnormalImmune = 0;
            directionVector = new Vector3(1, 0, 0);
            InGameManager.Instance.AddMultiPlayerInTeam(this);
            components.NickNameText.text = photonView.Owner.NickName;

            Transform baseZoneTransform = null;
            if (PunTeams.Team.RED == photonView.Owner.GetTeam())
            {
                components.ShadowRenderer.sprite = redTemaRing;
                gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
                //components.HitBox.gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
                components.DashEffect.Init(this, PunTeams.Team.RED);
            }
            else if (PunTeams.Team.BLUE == photonView.Owner.GetTeam())
            {
                components.ShadowRenderer.sprite = blueTemaRing;
                gameObject.layer = LayerMask.NameToLayer(InGameManager.BLUE_TEAM_PLAYER);
                //components.HitBox.gameObject.layer = LayerMask.NameToLayer(InGameManager.BLUE_TEAM_PLAYER);
                components.DashEffect.Init(this, PunTeams.Team.BLUE);
            }

            if (photonView.IsMine)
            {
                if (PunTeams.Team.RED == photonView.Owner.GetTeam())
                {
                    baseZoneTransform = InGameManager.Instance.GetRedTeamBaseZone();
                }
                else if (PunTeams.Team.BLUE == photonView.Owner.GetTeam())
                {
                    baseZoneTransform = InGameManager.Instance.GetBlueTeamBaseZone();
                }
                CameraController.Instance.AttachObject(this.transform, new Vector2(0, 0.5f)); // get Camera
                components.DirectionArrow.SetBaseTown(baseZoneTransform);
                InitController();
            }
            else
            {
                components.DirectionArrow.RemoveDirectionArrow();
            }
        }
        #endregion

        // 참고 : https://you-rang.tistory.com/193?category=764030
        private void Move()
        {
            if (!canMove || IsBehavioring(BehaviorState.DASH))
                return;

            // player 자신
            if (photonView.IsMine)
            {
                if (rgbody)
                {
                    rgbody.MovePosition(objTransform.position
                    + controller.GetMovingInputVector() * (movingSpeed) * Time.deltaTime);
                }

                //#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.W))
                {
                    bodyTransform.Translate(Vector2.up * movingSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    bodyTransform.Translate(Vector2.down * movingSpeed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    bodyTransform.Translate(Vector2.right * movingSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    bodyTransform.Translate(Vector2.left * movingSpeed * Time.fixedDeltaTime);
                }
                //#endif

                if (controller.GetMovingInputVector().sqrMagnitude > 0.1f)
                {
                    components.AnimHandler.Walk();
                }
                else
                {
                    components.AnimHandler.Idle();
                }
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
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //Debug.Log(Time.time);
            if (stream.IsWriting)
            {
                stream.SendNext(bodyTransform.position);
                stream.SendNext(isRightDirection);
            }
            else
            {
                currentPos = (Vector3)stream.ReceiveNext();
                isRightDirection = (bool)stream.ReceiveNext();
            }
        }
        public bool IsMine()
        {
            return photonView.IsMine;
        }
        public override void Dash(float dashSpeed, float distance)
        {
            base.Dash(dashSpeed, distance);
            photonView.RPC(DISPLAY_EFFECT, RpcTarget.AllViaServer, BehaviorState.DASH, true, directionDegree);
        }
        public override bool StopBehavior(BehaviorState stopState)
        {
            bool result = base.StopBehavior(stopState);
            //Debug.Log("stop behavior : " + result);
            if (result)
            {
                photonView.RPC(DISPLAY_EFFECT, RpcTarget.AllViaServer, BehaviorState.DASH, false, 0f);
            }
            return result;
        }
        public void ShowEmoticon(EmoticonType type)
        {
            photonView.RPC("PunShowEmoticon", RpcTarget.AllViaServer, type);
        }
        [PunRPC]
        public void PunShowEmoticon(EmoticonType type)
        {
            Debug.Log("ShowEmoticon : " + type);
            components.Emoticon.ShowEmoticon(type);
        }

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
        /// <summary> 행동 방해 상태 이상 갯수 증가 및 공격 AI OFF Check </summary>
        protected override void AddRetrictsBehaviorCount()
        {
            restrictBehaviorCount += 1;
            if (1 >= restrictBehaviorCount)
            {
                canBehavior = false;
            }
        }
        /// <summary> 행동 방해 상태 이상 갯수 감소 및 공격 AI ON Check </summary>
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

        #region statsfunc
        public override void ApplyItemEffect()
        {
            base.ApplyItemEffect();
            ItemEffectsData itemEffectsTotal = statsEffectsManager.ItemEffectsTotal;
            movingSpeed = movingSpeedOriginal * itemEffectsTotal.movingSpeedModified;
            components.ItemAcquisitionCollider.size = itemAcquisitionRangeOriginal * Mathf.Sqrt(itemEffectsTotal.itemAcquisitionRangeModified);
        }

        public override void ApplyConsumableItem(ItemEffectsData itemEffectsData)
        {
            if(true == itemEffectsData.canSlide)
            {
                Sliding();
            }
        }

        public override void PickUpInGameItem(UBZ.Item.InGameBuffType inGameBuffType)
        {
            ItemData itemData = InGameDataBase.Instance.GetInGameItemData(inGameBuffType);
            switch (itemData.target)
            {
                case ItemTarget.SELF:
                    photonView.RPC("AddInGameItem", RpcTarget.AllViaServer, inGameBuffType);
                    break;
                case ItemTarget.OUR_TEAM:
                    foreach(MultiPlayer multiPlayer in InGameManager.Instance.GetMultiPlayersPerTeam(GetTeam()))
                    {
                        if (null == multiPlayer) 
                            continue;
                        Debug.Log(GetTeam() + "팀 버프 적용 " + multiPlayer.GetUser().NickName);
                        multiPlayer.ApplyTeamBuff(inGameBuffType);
                    }
                    break;
                case ItemTarget.OPPONENT_TEAM:
                    // 현재 적팀 대상 효과 아이템 없음. 0526
                    break;
            }
        }

        public void ApplyTeamBuff(InGameBuffType inGameBuffType)
        {
            photonView.RPC("AddInGameItem", RpcTarget.AllViaServer, inGameBuffType);
        }

        public override void RunOutOfBuffTime(string itemName)
        {
            photonView.RPC("RemoveInGameItem", RpcTarget.AllViaServer, itemName);
        }


        // scriptbaleObejct
        [PunRPC]
        protected override void AddInGameItem(InGameBuffType inGameBuffType)
        {
            Debug.Log(inGameBuffType + " 아이템 적용, isMine : " + photonView.IsMine);
            statsEffectsManager.AddInGameItem(InGameDataBase.Instance.GetInGameItemData(inGameBuffType), photonView.IsMine);
        }

        [PunRPC]
        protected override void RemoveInGameItem(string itemName)
        {
            Debug.Log(itemName + " 버프 종료, isMine : " + photonView.IsMine);
            statsEffectsManager.RemoveItemEffect(InGameDataBase.Instance.GetInGameItemData(itemName));
        }
        #endregion


        #region collision
        private void OnCollisionEnter2D(Collision2D coll)
        {
         if (UtilityClass.CheckLayer(coll.gameObject.layer, 10))
            {
                Debug.Log("벽 충돌");
                canSlide = false;
            }   
        }
       
        // TODO : .
        public void HitDash(Vector2 pos, Vector2 dir, Photon.Realtime.Player dashOwner)
        {
            //Debug.Log(photonView.Owner.ActorNumber);
            photonView.RPC("PunHitDash", RpcTarget.AllViaServer, pos, dir, dashOwner);
        }
        [PunRPC]
        protected override void PunHitDash(Vector2 pos, Vector2 dir, Photon.Realtime.Player user)
        {
            KnockBack(500f, pos, dir, false);
            Stun(1f, 1f);
            if(photonView.IsMine)
            {
                int stolenSheetMusicCount = Mathf.CeilToInt((photonView.Owner.GetNumSheetMusic()) * InGameManager.DASH_OPPONENT_TEAM_STEALING_RATIO);
                photonView.Owner.AddNumSheetMusic(-stolenSheetMusicCount);
                user.AddNumSheetMusic(stolenSheetMusicCount);
            }
        }
        #endregion
        #endregion

        #region coroutine
        protected override IEnumerator StunCoroutine(float effectiveTime)
        {
            int type = (int)ControlTypeAbnormalStatus.STUN;
            abnormalComponents.StunEffect.SetActive(true);
            AddRetrictsMovingCount();
            AddRetrictsBehaviorCount();
            components.AnimHandler.Idle();
            //animationHandler.Idle();
            isControlTypeAbnormalStatuses[type] = true;
            controlTypeAbnormalStatusTime[type] = 0;
            controlTypeAbnormalStatusesDurationMax[type] = effectiveTime;
            while (controlTypeAbnormalStatusTime[type] <= controlTypeAbnormalStatusesDurationMax[type])
            {
                if (CharacterInfo.AbnormalImmune.STUN == (abnormalImmune & CharacterInfo.AbnormalImmune.STUN))
                {
                    controlTypeAbnormalStatusesDurationMax[type] = 0;
                    break;
                }
                controlTypeAbnormalStatusTime[type] += Time.fixedDeltaTime;
                yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
            }
            StopControlTypeAbnormalStatus(ControlTypeAbnormalStatus.STUN);
        }

        protected override IEnumerator SlidingCoroutine()
        {
            canSlide = true;
            int type = (int)ControlTypeAbnormalStatus.SLIDING;
            abnormalComponents.SlidingEffect.SetActive(true);
            AddRetrictsMovingCount();
            AddRetrictsBehaviorCount();
            //animationHandler.Idle();
            isControlTypeAbnormalStatuses[type] = true;
            Vector2 dirVector = controller.GetMoveRecentNormalInputVector();
            Debug.Log("sliding 코루틴 시작, " + dirVector);
            while (CharacterInfo.AbnormalImmune.SLIDING != (abnormalImmune & CharacterInfo.AbnormalImmune.SLIDING))
            {
                if(IsMine())
                {
                    // TODO : Modify Sliding animation
                    components.AnimHandler.Walk();
                    bodyTransform.Translate(dirVector * movingSpeed * Time.fixedDeltaTime);
                }
                if (false == canSlide)
                {
                    components.AnimHandler.Idle();
                    Debug.Log("sliding 코루틴 종료");
                    break;
                }
                yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
            }
            StopControlTypeAbnormalStatus(ControlTypeAbnormalStatus.SLIDING);
        }

        #endregion
    }
}

