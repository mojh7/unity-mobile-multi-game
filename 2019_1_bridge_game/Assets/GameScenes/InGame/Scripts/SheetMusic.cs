using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SheetMusic : CollisionLevelObject
{
    public void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                Debug.Log("악보와 충돌");
                Destroy();
            }
        }
    }
}
