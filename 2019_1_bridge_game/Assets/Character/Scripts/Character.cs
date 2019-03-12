using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInfo
{
    public enum SpawnType
    {
        NORMAL, SERVANT
    }
    public enum OwnerType
    {
        PLAYER, ENEMY, OBJECT, PET
    }

    public enum State
    {
        DIE, ALIVE
    }

    public enum DamageImmune
    {
        NONE, ALL
    }
    // 생각 중, 
    public enum AbnormalImmune
    {
        NONE, ALL
    }

    public enum AimType
    {
        AUTO, SEMIAUTO, MANUAL
    }
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
    [SerializeField]
    protected float movingSpeed;     // Character move Speed
    protected CharacterInfo.DamageImmune damageImmune;
    protected CharacterInfo.AbnormalImmune abnormalImmune;
    protected CharacterInfo.AimType aimType;
    protected CharacterInfo.State characterState;
    protected CharacterInfo.OwnerType ownerType;
    protected CharacterInfo.SpawnType spawnType;
    #endregion

    #region componets
    protected CharacterComponents Components;
    protected SpriteRenderer spriteRenderer;
    protected Transform spriteTransform;
    protected CircleCollider2D interactiveCollider2D;
    //protected AnimationHandler animationHandler;
    //protected BuffManager buffManager;
    protected Rigidbody2D rgbody;
    //protected AIController aiController;
    protected Transform shadowTransform;
    protected Transform bodyTransform;

    protected TextMesh textMesh;

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }
    }
    #endregion

    #region variables

    [SerializeField]
    protected Sprite sprite;

    protected bool isAutoAiming;    // 오토에임 적용 유무
    protected Vector3 directionVector;
    protected float directionDegree;  // 바라보는 각도(총구 방향)
    protected bool isRightDirection;    // character 방향이 우측이냐(true) 아니냐(flase = 좌측)

    protected Color baseColor;

    protected LayerMask enemyLayer;
    /// <summary> owner 좌/우 바라볼 때 spriteObject scale 조절에 쓰일 player scale, 우측 (1, 1, 1), 좌측 : (-1, 1, 1) </summary>
    protected Vector3 scaleVector;
    #endregion

    #region get / set
    //public CharacterComponents GetCharacterComponents()
    //{
    //    return Components;
    //}
    //public AbnormalComponents GetAbnormalComponents()
    //{
    //    return abnormalComponents;
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
    //public virtual WeaponManager GetWeaponManager()
    //{
    //    return weaponManager;
    //}
    //public BuffManager GetBuffManager()
    //{
    //    return buffManager;
    //}
    public CharacterInfo.OwnerType GetOwnerType()
    {
        return ownerType;
    }

    //public bool GetIsAcitveAttack()
    //{
    //    return isActiveAttack;
    //}
    //public bool GetIsAcitveMove()
    //{
    //    return isActiveMove;
    //}
    #endregion

    #region func
    
    public virtual void Init()
    {
        Components = GetComponent<CharacterComponents>();
        Components.Init();

        spriteRenderer = Components.SpriteRenderer;
        spriteTransform = Components.SpriteTransform;
        interactiveCollider2D = Components.InteractiveCollider2D;
        //animationHandler = Components.AnimationHandler;
        rgbody = Components.Rigidbody2D;
        shadowTransform = Components.ShadowTransform;
        bodyTransform = GetComponent<Transform>();

        textMesh = Components.TextMesh;
        spawnType = CharacterInfo.SpawnType.NORMAL;
    }

    #endregion


    //public virtual CustomObject Interact()
    //{
    //    //TODO : 만약에 Enemy를 조종하게 될 경우 Enemy Class에 재정의 필요
    //    return null;
    //}


    #region Abnormal
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

    
    
    #endregion



    public void SetSpawnType(CharacterInfo.SpawnType spawnType)
    {
        this.spawnType = spawnType;
    }



}


