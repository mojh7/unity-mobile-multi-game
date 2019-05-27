using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private UBZ.Item.InGameBuffType type;

    private void OnEnable()
    {
        Init();
    }

    public override void Init()
    {
        spriteRenderer.sprite = InGameDataBase.Instance.GetInGameItemData(type).sprite;

    }

    protected override void OnPickedUp()
    {
        if (pickupIsMine)
        {
            //InGameManager.Instance.GetMultiPlayer().AddInGameItem(InGameDataBase.Instance.GetInGameItemData(type));
            InGameManager.Instance.GetMultiPlayer().PickUpInGameItem(type);
        }
        else
        {
        }
    }
}

