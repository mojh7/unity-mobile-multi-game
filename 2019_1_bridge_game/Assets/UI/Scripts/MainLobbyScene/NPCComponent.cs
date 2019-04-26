using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCComponent : MonoBehaviour
{
    #region components
    [SerializeField] private CircleCollider2D interactiveCollider2D;
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private TextMesh nickNameText;
    #endregion

    #region parameter
 
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
        //BuffManager = GetComponent<BuffManager>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        //AIController = GetComponent<AIController>();
    }
    #endregion
}
