using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalComponents : MonoBehaviour
{
    #region components
    [SerializeField]
    private GameObject stunEffect;
    #endregion

    #region parameter
    public GameObject StunEffect
    {
        get
        {
            return stunEffect;
        }
    }
    #endregion
}
