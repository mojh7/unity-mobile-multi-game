// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PunTeams.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
// Implements teams in a room/game with help of player properties. Access them by Player.GetTeam extension.
// </summary>
// <remarks>
// Teams are defined by enum Team. Change this to get more / different teams.
// There are no rules when / if you can join a team. You could add this in JoinTeam or something.
// </remarks>                                                                                           
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>
    /// 플레이어 속성의 도움으로 방 / 게임에서 팀을 구현합니다.Player.GetTeam 확장자로 액세스하십시오.
    /// Implements teams in a room/game with help of player properties. Access them by Player.GetTeam extension.
    /// </summary>
    /// <remarks>
    /// 팀은 enum Team에 의해 정의됩니다.이 팀을 변경하여 더 많은 / 다른 팀을 만드십시오.
    /// 팀에 가입 할 수 있는 경우 규칙이 없습니다. 당신은 JoinTeam 또는 뭔가에 이것을 추가 할 수 있습니다.
    /// Teams are defined by enum Team. Change this to get more / different teams.
    /// There are no rules when / if you can join a team. You could add this in JoinTeam or something.
    /// </remarks>
    public class PunTeams : MonoBehaviourPunCallbacks
    {
        /// <summary>Enum defining the teams available. First team should be neutral (it's the default value any field of this enum gets).</summary>
        public enum Team : byte { NONE, RED, BLUE, NUM_STATS };

        /*
        /// <summary>The main list of teams with their player-lists. Automatically kept up to date.</summary>
        /// <remarks>Note that this is static. Can be accessed by PunTeam.PlayersPerTeam. You should not modify this.</remarks>
        */
        /// <summary> 플레이어 목록이 있는 팀의 주요 목록. 자동으로 최신 상태로 유지됩니다. </summary>
        /// <remarks> 이것은 정적이라는 점에 유의하십시오. PunTeam.PlayersPerTeam에서 액세스 할 수 있습니다. 이것을 수정해서는 안됩니다. </remarks>
        public static Dictionary<Team, List<Player>> PlayersPerTeam;

        // <summary>Defines the player custom property name to use for team affinity of "this" player.</summary>
        /// <summary>"this"플레이어의 팀 유사성에 사용할 플레이어 사용자 정의 속성 이름을 정의합니다.</summary>
        public const string TeamPlayerProp = "team";

        #region Events by Unity and Photon

        public void Start()
        {
            PlayersPerTeam = new Dictionary<Team, List<Player>>();
            Array enumVals = Enum.GetValues(typeof(Team));
            foreach (var enumVal in enumVals)
            {
                PlayersPerTeam[(Team)enumVal] = new List<Player>();
            }
        }

        public override void OnDisable()
        {
            PlayersPerTeam = new Dictionary<Team, List<Player>>();
        }



        /*/// <summary>Needed to update the team lists when joining a room.</summary>
        /// <remarks>Called by PUN. See enum MonoBehaviourPunCallbacks for an explanation.</remarks>*/
        /// <summary> 방에 들어갈 때 팀 목록을 업데이트 해야합니다. </summary>
        /// <remarks> PUN이 부름. 설명을 보려면 enum MonoBehaviourPunCallbacks를 참조하십시오. </remarks>
        public override void OnJoinedRoom()
        {
            Debug.Log("팀 조인드 룸");
            this.UpdateTeams();
        }

        public override void OnLeftRoom()
        {
            Start();
        }

        /*
        /// <summary>Refreshes the team lists. It could be a non-team related property change, too.</summary>
        /// <remarks>Called by PUN. See enum MonoBehaviourPunCallbacks for an explanation.</remarks>
        */
        /// <summary> 팀 목록을 새로 고칩니다. 팀과 관련없는 속성 변경 일 수도 있습니다. </summary>
        /// <remarks> PUN이 부름. 설명을 보려면 enum MonoBehaviourPunCallbacks를 참조하십시오. </remarks>
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            this.UpdateTeams();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            this.UpdateTeams();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            this.UpdateTeams();
        }

        #endregion


        public void UpdateTeams()
        {
            Array enumVals = Enum.GetValues(typeof(Team));
            foreach (var enumVal in enumVals)
            {
                PlayersPerTeam[(Team)enumVal].Clear();
            }

            /*
            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log("--Master, team setting--");
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Player player = PhotonNetwork.PlayerList[i];
                    if (i % 2 == 0)
                    {
                        player.SetTeam(Team.RED);
                    }
                    else
                    {
                        player.SetTeam(Team.BLUE);
                    }
                    Debug.Log("i : " + player.ActorNumber + ", " + player.GetTeam());
                }
            }
            */

            // Debug.Log("--UpdateTeams--");
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player = PhotonNetwork.PlayerList[i];
                PlayersPerTeam[player.GetTeam()].Add(player);
                //Debug.Log("i : " + player.ActorNumber + ", " + playerTeam);
            }
        }
    }

    /// <summary>Extension used for PunTeams and Player class. Wraps access to the player's custom property.</summary>
    public static class TeamExtensions
    {
        /// <summary>Extension for Player class to wrap up access to the player's custom property.</summary>
        /// <returns>PunTeam.Team.none if no team was found (yet).</returns>
        public static PunTeams.Team GetTeam(this Player player)
        {
            object teamId;
            if (player.CustomProperties.TryGetValue(PunTeams.TeamPlayerProp, out teamId))
            {
                return (PunTeams.Team)teamId;
            }

            return PunTeams.Team.NONE;
        }

        /*
        /// <summary>Switch that player's team to the one you assign.</summary>
        /// <remarks>Internally checks if this player is in that team already or not. Only team switches are actually sent.</remarks>
        /// <param name="player"></param>
        /// <param name="team"></param>
        */

        /// <summary> 지정한 팀으로 팀을 전환하십시오. </summary>
        /// <remarks> 이 플레이어가 해당 팀에 이미 있는지 여부를 내부적으로 확인합니다. 팀 스위치 만 실제로 전송됩니다. </remarks>
        /// <param name = "player"> </param>
        /// <param name = "team"> </param>
        public static void SetTeam(this Player player, PunTeams.Team team)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
                return;
            }

            PunTeams.Team currentTeam = player.GetTeam();
            if (currentTeam != team)
            {
                player.SetCustomProperties(new Hashtable() { { PunTeams.TeamPlayerProp, (byte)team } });
            }
        }
    }
}