using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class TeamBaseZone : MonoBehaviour
{
    [SerializeField] private PunTeams.Team team;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        int numSheetMusic = PhotonNetwork.LocalPlayer.GetNumSheetMusic();
        PhotonNetwork.LocalPlayer.AddScore(numSheetMusic);
        PhotonNetwork.LocalPlayer.SetNumSheetMusic(0);
        Debug.Log("악보를 모았다 : " + numSheetMusic + "(개), score : " + PhotonNetwork.LocalPlayer.GetScore());
    }
}
