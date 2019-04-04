using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalComponents : MonoBehaviour
{
    #region components
    [SerializeField] private Transform abnormalStatusTransform;
    [SerializeField] private GameObject stunEffect;
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
    #endregion
}
