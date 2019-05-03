using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//메인로비 캐릭터
public class MainPlayer : MonoBehaviour
{
    #region components
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private CircleCollider2D interactiveCollider2D;
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private TextMesh nickNameText;
    public MainJoystick joystick;
    public float MoveSpeed;

    private Vector3 _moveVector;
    private Transform _transform;


    #endregion

    #region Unityfuc
    private void Start()
    {
        _transform = transform;
        _moveVector = Vector3.zero;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region joystick
    public void HandleInput()
    {
        _moveVector = poolInput();
    }

    public Vector3 poolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
        Vector3 moveDir = new Vector3(h, v, 0).normalized;

        return moveDir;
    }

    public void Move()
    {
        _transform.Translate(_moveVector * MoveSpeed * Time.deltaTime);
    }
    #endregion

    #region parameter
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }
    }
    public Transform SpriteTransform
    {
        get
        {
            return spriteTransform;
        }
    }
    public Animator Animator
    {
        get
        {
            return animator;
        }
    }

    public CircleCollider2D InteractiveCollider2D
    {
        get
        {
            return interactiveCollider2D;
        }
    }
    public CircleCollider2D CircleCollider2D
    {
        get
        {
            return circleCollider2D;
        }
    }
    public BoxCollider2D HitBox
    {
        get
        {
            return hitBox;
        }
    }
    public Transform ShadowTransform
    {
        get
        {
            return shadowTransform;
        }
    }
    public TextMesh NickNameText
    {
        get
        {
            return nickNameText;
        }
    }


    public Rigidbody2D Rigidbody2D { get; private set; }


    #endregion

    #region func
    public void Init()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    #endregion
}
