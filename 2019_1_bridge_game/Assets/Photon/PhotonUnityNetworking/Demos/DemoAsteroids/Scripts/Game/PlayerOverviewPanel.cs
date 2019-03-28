// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerNumbering.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player Overview Panel
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Asteroids
{
    public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerOverviewEntryPrefab;

        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            //playerListEntries = new Dictionary<int, GameObject>();

            //foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            //{
            //    GameObject entry = Instantiate(PlayerOverviewEntryPrefab);
            //    entry.transform.SetParent(gameObject.transform);
            //    entry.transform.localScale = Vector3.one;
            //    entry.GetComponent<Text>().color = InGameManager.GetPlayerColorWithTeam(p.GetPlayerNumber());
            //    //entry.GetComponent<Text>().text = string.Format("{0}\n개인 점수 : {1}\n팀 점수 : {2}\n보유 악보 수: {3}", p.NickName, p.GetScore(), AsteroidsGame.PLAYER_MAX_LIVES);
            //    entry.GetComponent<Text>().text = string.Format("{0}\n개인 점수 : {1}\n보유 악보 수: {2}", p.NickName, p.GetScore(), 0);

            //    playerListEntries.Add(p.ActorNumber, entry);
            //}
        }
        #endregion

        public void OnGUI()
        {
            PunTeams.Team teamName = PunTeams.Team.RED;
            int redTeamScore = 0, blueTeamScore = 0;
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 24;
            guiStyle.normal.textColor = Color.red;
            GUILayout.Label("Team: " + teamName.ToString(), guiStyle);
            List<Player> redTeamPlayers = PunTeams.PlayersPerTeam[teamName];
            foreach (Player player in redTeamPlayers)
            {
                //GUILayout.Label("  " + player.ToStringFull() + " Score: " + player.GetScore(), guiStyle);
                GUILayout.Label(player.NickName + " : 점수 : " + player.GetScore() + ", 악보 수 : " + player.GetNumSheetMusic(), guiStyle);
                redTeamScore += player.GetScore();
            }
            guiStyle.normal.textColor = Color.blue;
            teamName = PunTeams.Team.BLUE;
            GUILayout.Label("Team: " + teamName.ToString(), guiStyle);
            List<Player> blueTeamPlayers = PunTeams.PlayersPerTeam[teamName];
            foreach (Player player in blueTeamPlayers)
            {
                //GUILayout.Label("  " + player.ToStringFull() + " Score: " + player.GetScore(), guiStyle);
                GUILayout.Label("  " + player.ToStringFull() + " Score: " + player.GetScore(), guiStyle);
                blueTeamScore += player.GetScore();
            }
            guiStyle.normal.textColor = Color.white;
            GUILayout.Label("팀 점수 Red : " + redTeamScore + ", Blue : " + blueTeamScore, guiStyle);
        }

        #region PUN CALLBACKS

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            //Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            //playerListEntries.Remove(otherPlayer.ActorNumber);
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            //Debug.Log(targetPlayer.NickName);
            //GameObject entry;
            //if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            //{
            //    entry.GetComponent<Text>().text = string.Format("{0}\n개인 점수 : {1}\n보유 악보 수: {2}", targetPlayer.NickName, targetPlayer.GetScore(), targetPlayer.GetNumSheetMusic());
            //    //entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", targetPlayer.NickName, targetPlayer.GetScore(), targetPlayer.CustomProperties[AsteroidsGame.PLAYER_LIVES]);
            //}
        }

        #endregion
    }
}