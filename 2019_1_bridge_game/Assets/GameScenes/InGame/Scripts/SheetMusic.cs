using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class SheetMusic : PickupItem
{
    private static readonly int[] pianoIndex =
    {
        4, 5, 6, 4, 9, 6, 5, 9, 5, 4, 2, 6, 4, 3, 3, 2, 3, 4, 5, 1, 4, 5, 6, 7, 7, 6, 5, 4, 5,
4, 5, 6, 4, 9, 6, 5, 9, 5, 4, 2, 2, 3, 4, 1, 1, 2, 3, 4, 5, 1, 4, 5, 6, 7, 7, 6, 5 ,4, 4 };
    
//6, 7, 8, 8, 8, 8, 8, 9, 8, 7,b ,6, 6 ,6, 6 ,6, 7, 6 ,5 ,4, 4, 4, 3, 2, 3, 3, 4 }
//    }
//    {
//        4, 5, 6, 4, 8, 6, 5, 8, 5, 4, 2, 6, 4, 3, 3, 2, 3, 4, 5, 1, 4, 5, 6, 7b, 7b, 6, 5, 4, 5,
//4, 5, 6, 4, 8, 6, 5, 8, 5, 4, 2, 2, 3, 4, 1, 1, 2, 3, 4, 5, 1, 4, 5, 6, 7b, 7b, 6, 5 ,4, 4
//6, 7b, 8, 8, 8, 8, 8, 9, 8, 7,b ,6, 6 ,6, 6 ,6, 7, 6 ,5 ,4, 4, 4, 3, 2, 3, 3, 4 }
//}
    protected override void OnPickedUp()
    {
        if (PickupIsMine)
        {
            //Debug.Log("I picked up something. That's a score!, " + pianoIndex.Length);
            AudioManager.Instance.PlaySound(pianoIndex[(PhotonNetwork.LocalPlayer.GetNumSheetMusic() % pianoIndex.Length)], SFXType.PIANO);
            PhotonNetwork.LocalPlayer.AddNumSheetMusic(1);
        }
        else
        {
            //Debug.Log("Someone else picked up something. Lucky!");
        }
    }
}
