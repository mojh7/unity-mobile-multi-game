// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerNumbering.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities,
// </copyright>
// <summary>
//  Assign numbers to Players in a room. Uses Room custom Properties
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>
    /// Implements consistent numbering in a room/game with help of room properties. Access them by Player.GetPlayerNumber() extension.
    /// 실내 속성을 사용하여 실내 / 게임에서 일관된 번호 매기기를 구현합니다. Player.GetPlayerNumber () 확장 프로그램을 통해 액세스하십시오.
    /// </summary>
    /// <remarks>
    /// indexing ranges from 0 to the maximum number of Players.
    /// indexing remains for the player while in room.
    /// If a Player is numbered 2 and player numbered 1 leaves, numbered 1 become vacant and will assigned to the future player joining (the first available vacant number is assigned when joining)
    /// 인덱싱 범위는 0에서 최대 수까지입니다.
    /// 실내에서 플레이어는 인덱싱을 유지합니다.
    /// 플레이어의 번호가 2이고 플레이어의 번호가 1 번인 경우 1 번 번호는 비어 있고 향후 플레이어가 참여할 수 있도록 할당됩니다 (가입 할 때 첫 번째로 사용 가능한 빈 번호가 할당 됨)
    /// </remarks>
    public class PlayerNumbering : MonoBehaviourPunCallbacks
    {
        //TODO: Add a "numbers available" bool, to allow easy access to this?!
        //이것에 쉽게 접근 할 수 있도록 "숫자 이용 가능"bool을 추가하십시오!

        #region Public Properties

        /// <summary>
        /// The instance. EntryPoint to query about Room Indexing. 
        /// 인스턴스.EntryPoint가 룸 인덱싱에 대해 쿼리합니다.
        /// </summary>
        public static PlayerNumbering instance;

        public static Player[] SortedPlayers;

        /// <summary>
        /// OnPlayerNumberingChanged delegate. Use
        /// OnPlayerNumberingChanged 대리자입니다.용도
        /// </summary>
        public delegate void PlayerNumberingChanged();
        /// <summary>
        /// Called everytime the room Indexing was updated. Use this for discrete updates. Always better than brute force calls every frame.
        /// 객실 인덱싱이 업데이트 될 때마다 호출됩니다. 이산 업데이트에 사용하십시오. 모든 프레임에 무차별 적 호출보다 항상 좋습니다.
        /// </summary>
        public static event PlayerNumberingChanged OnPlayerNumberingChanged;


        /// <summary>Defines the room custom property name to use for room player indexing tracking.</summary>
        /// room 플레이어 색인 추적에 사용할 방 사용자 정의 속성 이름을 정의합니다.
        public const string RoomPlayerIndexedProp = "pNr";

        /// <summary>
        /// dont destroy on load flag for this Component's GameObject to survive Level Loading.
        /// 레벨 로딩에서 생존하기 위해이 컴포넌트의 GameObject에 대한 로드 플래그를 파괴하지 마십시오.
        /// </summary>
        public bool dontDestroyOnLoad = false;


        #endregion


        #region MonoBehaviours methods

        public void Awake()
        {

            if (instance != null && instance != this && instance.gameObject != null)
            {
                GameObject.DestroyImmediate(instance.gameObject);
            }

            instance = this;
            if (dontDestroyOnLoad)
            { 
                DontDestroyOnLoad(this.gameObject);
            }

            this.RefreshData();
        }

        #endregion


        #region PunBehavior Overrides

        public override void OnJoinedRoom()
        {
            this.RefreshData();
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Remove(PlayerNumbering.RoomPlayerIndexedProp);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            this.RefreshData();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            this.RefreshData();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps != null && changedProps.ContainsKey(PlayerNumbering.RoomPlayerIndexedProp))
            {
                this.RefreshData();
            }
        }

        #endregion


        // each player can select it's own playernumber in a room, if all "older" players already selected theirs
        // 모든 "오래된"플레이어가 이미 자신의 것을 선택한 경우 각 플레이어가 방 안의 자신의 플레이어 번호를 선택할 수 있습니다.

        // Internal call Refresh the cached data and call the OnPlayerNumberingChanged delegate.
        /// <summary>
        /// 내부 호출 캐시 된 데이터를 새로 고치고 OnPlayerNumberingChanged 대리자를 호출합니다.
        /// </summary>
        public void RefreshData()
        {
            if (PhotonNetwork.CurrentRoom == null)
            {
                return;
            }

            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() >= 0)
            {
                SortedPlayers = PhotonNetwork.CurrentRoom.Players.Values.OrderBy((p) => p.GetPlayerNumber()).ToArray();
                if (OnPlayerNumberingChanged != null)
                {
                    OnPlayerNumberingChanged();
                }
                return;
            }


            HashSet<int> usedInts = new HashSet<int>();
            Player[] sorted = PhotonNetwork.PlayerList.OrderBy((p) => p.ActorNumber).ToArray();

            string allPlayers = "all players: ";
            foreach (Player player in sorted)
            {
                allPlayers += player.ActorNumber + "=pNr:"+player.GetPlayerNumber()+", ";

                int number = player.GetPlayerNumber();

                // if it's this user, select a number and break
                // else:
                    // check if that user has a number
                    // if not, break!
                    // else remember used numbers

                if (player.IsLocal)
                {
					Debug.Log ("PhotonNetwork.CurrentRoom.PlayerCount = " + PhotonNetwork.CurrentRoom.PlayerCount);

                    // select a number
                    for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                    {
                        if (!usedInts.Contains(i))
                        {
                            player.SetPlayerNumber(i);
                            break;
                        }
                    }
                    // then break
                    break;
                }
                else
                {
                    if (number < 0)
                    {
                        break;
                    }
                    else
                    {
                        usedInts.Add(number);
                    }
                }
            }

            //Debug.Log(allPlayers);
            //Debug.Log(PhotonNetwork.LocalPlayer.ToStringFull() + " has PhotonNetwork.player.GetPlayerNumber(): " + PhotonNetwork.LocalPlayer.GetPlayerNumber());

            SortedPlayers = PhotonNetwork.CurrentRoom.Players.Values.OrderBy((p) => p.GetPlayerNumber()).ToArray();
            if (OnPlayerNumberingChanged != null)
            {
                OnPlayerNumberingChanged();
            }
        }
    }



    /// <summary>Extension used for PlayerRoomIndexing and Player class.</summary>
    public static class PlayerNumberingExtensions
    {
        /*
        /// <summary>Extension for Player class to wrap up access to the player's custom property.
		/// Make sure you use the delegate 'OnPlayerNumberingChanged' to knoiw when you can query the PlayerNumber. Numbering can changes over time or not be yet assigned during the initial phase ( when player creates a room for example)
		/// </summary>
        /// <returns>persistent index in room. -1 for no indexing</returns>
        */

        /// <summary> 플레이어 클래스가 플레이어의 사용자 지정 속성에 대한 액세스를 마무리하는 확장 프로그램입니다.
        /// PlayerNumber를 쿼리 할 수있을 때 대리자 'OnPlayerNumberingChanged'를 사용하여 확인하십시오. 번호 매김은 시간이 지남에 따라 변경되거나 초기 단계에서 지정되지 않을 수 있습니다 (플레이어가 예를 들어 회의실을 만들 때).
        /// </summary>
        /// <returns> 방의 영구 인덱스. 인덱스가없는 경우 -1 </returns>
        public static int GetPlayerNumber(this Player player)
        {
			if (player == null) {
				return -1;
			}

            if (PhotonNetwork.OfflineMode)
            {
                return 0;
            }
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                return -1;
            }

            object value;
			if (player.CustomProperties.TryGetValue (PlayerNumbering.RoomPlayerIndexedProp, out value)) {
				return (byte)value;
			}
            return -1;
        }

        /*
		/// <summary>
		/// Sets the player number.
		/// It's not recommanded to manually interfere with the playerNumbering, but possible.
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="playerNumber">Player number.</param>
        */

        /// <summary>
        /// 플레이어 번호를 설정합니다.
        /// 수동으로 플레이어를 방해하는 것은 권장되지 않습니다. 넘버링은 가능하지만 가능합니다.
        /// </summary>
        /// <param name = "player"> 플레이어. </param>
        /// <param name = "playerNumber"> 플레이어 번호. </param>
        public static void SetPlayerNumber(this Player player, int playerNumber)
        {
			if (player == null) {
				return;
			}

            if (PhotonNetwork.OfflineMode)
            {
                return;
            }

            if (playerNumber < 0)
            {
                Debug.LogWarning("Setting invalid playerNumber: " + playerNumber + " for: " + player.ToStringFull());
            }

            if (!PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogWarning("SetPlayerNumber was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
                return;
            }

            int current = player.GetPlayerNumber();
            if (current != playerNumber)
            {
				Debug.Log("PlayerNumbering: Set number "+playerNumber);
                player.SetCustomProperties(new Hashtable() { { PlayerNumbering.RoomPlayerIndexedProp, (byte)playerNumber } });
            }
        }
    }
}