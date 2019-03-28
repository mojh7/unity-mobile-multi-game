using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class SheetMusic : PickupItem
{
    protected override void OnPickedUp()
    {
        if (PickupIsMine)
        {
            Debug.Log("I picked up something. That's a score!");
            PhotonNetwork.LocalPlayer.AddNumSheetMusic(1);
        }
        else
        {
            Debug.Log("Someone else picked up something. Lucky!");
        }
    }
}
