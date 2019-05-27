using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalComponents : MonoBehaviour
{
    #region components
    [SerializeField] private Transform abnormalStatusTransform;
    [SerializeField] private GameObject stunEffect;
    [SerializeField] private GameObject slidingEffect;
    #endregion

    #region parameter
    public Transform AbnormalStatusTransform
    {
        get
        {
            return abnormalStatusTransform;
        }
    }

    public GameObject StunEffect
    {
        get
        {
            return stunEffect;
        }
    }

    public GameObject SlidingEffect
    {
        get
        {
            return slidingEffect;
        }
    }
    #endregion
}
