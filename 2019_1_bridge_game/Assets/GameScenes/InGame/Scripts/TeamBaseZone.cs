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
        if(collision.GetComponent<UBZ.MultiGame.Owner.Player>().IsMine())
        {
            int numSheetMusic = PhotonNetwork.LocalPlayer.GetNumSheetMusic();
            if(numSheetMusic > 0)
            {
                AudioManager.Instance.PlaySound("ComboMax", SFXType.COMMON);
            }
            PhotonNetwork.LocalPlayer.AddScore(numSheetMusic);
            PhotonNetwork.LocalPlayer.SetNumSheetMusic(0);
            Debug.Log("악보를 모았다 : " + numSheetMusic + " (개), 개인 점수 : " + PhotonNetwork.LocalPlayer.GetScore());
        }
    }
}
