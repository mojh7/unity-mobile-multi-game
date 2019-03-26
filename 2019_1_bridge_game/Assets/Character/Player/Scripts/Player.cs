using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
        private Transform objTransform;

        private PhotonView photonView;
        private new Rigidbody rigidbody;
        private new Collider collider;
        private new Renderer renderer;
        #endregion

        #region get / set
        #endregion

        #region unityFunc
        void Awake()
        {
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

        //void update()
        //{
        //    if (!photonView.IsMine || !controllable)
        //    {
        //        return;
        //    }

        //    rotation = Input.GetAxis("Horizontal");
        //    acceleration = Input.GetAxis("Vertical");

        //    if (Input.GetButton("Jump") && shootingTimer <= 0.0)
        //    {
        //        shootingTimer = 0.2f;

        //        photonView.RPC("Fire", RpcTarget.AllViaServer, rigidbody.position, rigidbody.rotation);
        //    }

        //    if (shootingTimer > 0.0f)
        //    {
        //        shootingTimer -= Time.deltaTime;
        //    }
        //}

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
            if (photonView.IsMine)
            {
                CameraController.Instance.AttachObject(this.transform); // get Camera
                baseColor = Color.white;
                Components.DirectionArrow.SetBaseTown(InGameManager.Instance.GetBaseTown());
                InitController();
                TimeController.Instance.PlayStart();
            }
            //textMesh.text = GameDataManager.Instance.userData.GetNickname();


            //animationHandler.Init(this, PlayerManager.Instance.runtimeAnimator);

            //Debug.Log("hpMax : " + hpMax);
        }

        #endregion

        #region func

        [PunRPC]
        public void Dash()
        {

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
            // player 자신
            if(photonView.IsMine)
            {
                if (rgbody)
                {
                    rgbody.MovePosition(objTransform.position
                    + controller.GetMovingInputVector() * (movingSpeed) * Time.fixedDeltaTime);
                }

#if UNITY_EDITOR
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
#endif
                directionDegree = controller.GetMovingInputDegree();

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

        #region coroutine


        #endregion
    }
}

