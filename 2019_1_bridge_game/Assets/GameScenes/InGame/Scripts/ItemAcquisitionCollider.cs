using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAcquisitionCollider : MonoBehaviour
{
    [SerializeField] private Photon.Pun.PhotonView ownerPhotonView;
    public Photon.Pun.PhotonView OwnerPhotonView
    {
        get
        {
            return ownerPhotonView;
        }
    }
}
