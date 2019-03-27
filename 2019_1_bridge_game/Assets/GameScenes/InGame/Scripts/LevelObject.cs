using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class LevelObject : MonoBehaviour
{
    protected PhotonView photonView;
    
    protected virtual void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
}

public abstract class CollisionLevelObject : LevelObject
{
    protected new Rigidbody rigidbody;
    protected bool isDestroyed;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();

        if (photonView.InstantiationData != null)
        {
            rigidbody.AddForce((Vector3)photonView.InstantiationData[0]);
            rigidbody.AddTorque((Vector3)photonView.InstantiationData[1]);
        }
    }

    protected virtual void Destroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
