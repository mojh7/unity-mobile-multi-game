using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace UBZ.Item
{
    public enum InGameBuffType
    {
        SPEED_UP,
        MAGNET,
        SPEED_DOWN,
        SLIDING
    };
}


public class InGameBuffItem : PickupItem
{
    private static int NUM_BUFF_ITEM = 4;
    [SerializeField] private UBZ.Item.InGameBuffType type;

    [Photon.Pun.PunRPC]
    private void SetInGameBuffType(UBZ.Item.InGameBuffType type)
    {
        this.type = type;
    }

    protected override void OnPickedUp()
    {
        if (pickupIsMine)
        {
            InGameManager.Instance.GetMultiPlayer().PickUpInGameItem(type);
            UBZ.Item.InGameBuffType sentType = InGameDataBase.Instance.GetInGameItemType();
            this.photonView.RPC("SetInGameBuffType", RpcTarget.AllViaServer, sentType);
        }
        else
        {
        }
    }
}

