using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterComponents : MonoBehaviour
{
    #region components
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private CharacterAnimHandler animHandler;
    //[SerializeField] private AnimationHandler animationHandler;
    [SerializeField] private BoxCollider2D itemAcquisitionCollider;
    [SerializeField] private SpriteRenderer shadowRenderer;
    [SerializeField] private TextMesh nickNameText;
    [SerializeField] private DashEffect dashEffect;
    [SerializeField] private Emoticon emoticon;
    [SerializeField] private TextMeshPro sheetMusicCountTextMesh;

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
    public CharacterAnimHandler AnimHandler
    {
        get
        {
            return animHandler;
        }
    }
    //public AnimationHandler AnimationHandler
    //{
    //    get
    //    {
    //        return animationHandler;
    //    }
    //}
    public BoxCollider2D ItemAcquisitionCollider
    {
        get
        {
            return itemAcquisitionCollider;
        }
    }
    public SpriteRenderer ShadowRenderer
    {
        get
        {
            return shadowRenderer;
        }
    }
    public TextMesh NickNameText
    {
        get
        {
            return nickNameText;
        }
    }
    public DashEffect DashEffect
    {
        get
        {
            return dashEffect;
        }
    }
    public GameObject DashEffectObj { get; private set; }

    public Emoticon Emoticon
    {
        get
        {
            return emoticon;
        }
    }
    //public BuffManager BuffManager { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public DirectionArrow DirectionArrow { get; private set; }
    //public AIController AIController { get; private set; }

    public TextMeshPro SheetMusicCountTextMesh
    {
        get
        {
            return sheetMusicCountTextMesh;
        }
    }
    #endregion

    private void Awake()
    {
        DashEffectObj = dashEffect.GetDashEffectObj();
    }

    #region func
    public void Init()
    {
        //BuffManager = GetComponent<BuffManager>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        DirectionArrow = GetComponent<DirectionArrow>();
        //AIController = GetComponent<AIController>();
    }
    #endregion
}
