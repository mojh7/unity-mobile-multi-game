// ----------------------------------------------------------------------------
// <copyright file="Player.cs" company="Exit Games GmbH">
//   Loadbalancing Framework for Photon - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//   Per client in a room, a Player is created. This client's Player is also
//   known as PhotonClient.LocalPlayer and the only one you might change
//   properties for.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY
    using UnityEngine;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
#endif

    // single Game에서 쓸 Player 개념보다 User 개념이 더 강한 듯!

    /*
    /// <summary>
    /// Summarizes a "player" within a room, identified (in that room) by ID (or "actorNumber").
    /// </summary>
    /// <remarks>
    /// Each player has a actorNumber, valid for that room. It's -1 until assigned by server (and client logic).
    /// </remarks>
     */

    /// <summary>
    /// ID (또는 "actorNumber")로 식별되는 (해당 방에서) 방 안의 "플레이어"를 요약합니다.
    /// </summary>
    /// <remarks>
    /// 각 플레이어는 해당 방에서 유효한 actorNumber를가집니다. 서버 (및 클라이언트 논리)가 지정할 때까지 -1입니다.
    /// </remarks>
    public class Player
    {

        // Used internally to identify the masterclient of a room.
        /// <summary>
        /// 방의 마스터 클라이언트를 식별하기 위해 내부적으로 사용됩니다.
        /// </summary>
        protected internal Room RoomReference { get; set; }


        /// <summary>Backing field for property.</summary>
        private int actorNumber = -1;

        /*
        /// <summary>Identifier of this player in current room. Also known as: actorNumber or actorNumber. It's -1 outside of rooms.</summary>
        /// <remarks>The ID is assigned per room and only valid in that context. It will change even on leave and re-join. IDs are never re-used per room.</remarks>
        */

        /// <summary> 현재 방에있는 이 플레이어의 식별자. actorNumber 또는 actorNumber로도 알려져 있습니다. 객실 밖에서는 -1입니다. </summary>
        /// <remarks> ID는 방당 할당되며 해당 상황에서만 유효합니다. 휴가와 재결합시에도 변경됩니다. ID는 객실 당 결코 재사용되지 않습니다. </remarks>
        public int ActorNumber
        {
            get { return this.actorNumber; }
        }

        // Only one player is controlled by each client. Others are not local.
        /// <summary>
        /// 한 명의 플레이어만 각 클라이언트가 제어합니다. 다른 것들은 지역 적이 아닙니다.
        /// </summary>
        public readonly bool IsLocal;
         

        /// <summary>Background field for nickName.</summary>
		private string nickName = string.Empty;

        /*
        /// <summary>Non-unique nickname of this player. Synced automatically in a room.</summary>
        /// <remarks>
        /// A player might change his own playername in a room (it's only a property).
        /// Setting this value updates the server and other players (using an operation).
        /// </remarks>
        */

        /// <summary>
        /// 이 플레이어의 고유하지 않은 별명. 방에 자동으로 동기화되었습니다.
        /// </summary>
        /// <remarks>
        /// 플레이어는 방에서 자신의 플레이어 이름을 변경할 수 있습니다 (속성 일뿐입니다).
        /// 이 값을 설정하면 서버 및 다른 플레이어가 업데이트됩니다 (작업 사용).
        /// </remarks>
        public string NickName
        {
            get
            {
                return this.nickName;
            }
            set
            {
                if (!string.IsNullOrEmpty(this.nickName) && this.nickName.Equals(value))
                {
                    return;
                }

                this.nickName = value;

                // update a room, if we changed our nickName (locally, while being in a room)
                if (this.IsLocal && this.RoomReference != null)
                {
                    this.SetPlayerNameProperty();
                }
            }
        }

        /*
        /// <summary>UserId of the player, available when the room got created with RoomOptions.PublishUserId = true.</summary>
        /// <remarks>Useful for PhotonNetwork.FindFriends and blocking slots in a room for expected players (e.g. in PhotonNetwork.CreateRoom).</remarks>
        */

        /// <summary> RoomIptions.PublishUserId = true로 룸을 만들었을 때 사용할 수있는 플레이어의 UserId입니다. </summary>
        /// <remarks> 예를 들어, PhotonNetwork.FindFriends와 예상되는 방 (예 : PhotonNetwork.CreateRoom)의 방을 차단하는 데 유용합니다. </remarks>
        public string UserId { get; internal set; }

        /*
        /// <summary>
        /// True if this player is the Master Client of the current room.
        /// </summary>
        /// <remarks>
        /// See also: PhotonNetwork.MasterClient.
        /// </remarks>
        */

        /// <summary>
        /// 이 플레이어가 현재 방의 마스터 클라이언트 인 경우 true입니다.
        /// </summary>
        /// <remarks>
        /// 참고 : PhotonNetwork.MasterClient.
        /// </remarks>
        public bool IsMasterClient
        {
            get
            {
                if (this.RoomReference == null)
                {
                    return false;
                }

                return this.ActorNumber == this.RoomReference.MasterClientId;
            }
        }

        /*
        /// <summary>If this player is active in the room (and getting events which are currently being sent).</summary>
        /// <remarks>
        /// Inactive players keep their spot in a room but otherwise behave as if offline (no matter what their actual connection status is).
        /// The room needs a PlayerTTL != 0. If a player is inactive for longer than PlayerTTL, the server will remove this player from the room.
        /// For a client "rejoining" a room, is the same as joining it: It gets properties, cached events and then the live events.
        /// </remarks>
        */

        /// <summary>이 플레이어가 방에서 활동 중일 때 (현재 보내는 이벤트를받는 경우) </summary>
        /// <remarks>
        /// 비활성 플레이어는 방에서 자신의 자리를 지키지 만, 그렇지 않은 경우 (실제 연결 상태가 무엇이든 상관없이) 오프라인으로 행동합니다.
        /// Room에 PlayerTTL! = 0이 필요합니다. 플레이어가 PlayerTTL보다 오래 비활성 상태 인 경우 서버는이 플레이어를 회의실에서 제거합니다.
        /// 클라이언트가 방에 다시 참여하는 경우 참여하는 것과 동일합니다. 속성, 캐시 된 이벤트 및 라이브 이벤트를 가져옵니다.
        /// </remarks>
        public bool IsInactive { get; protected internal set; }

        /* 
        /// <summary>Read-only cache for custom properties of player. Set via Player.SetCustomProperties.</summary>
        /// <remarks>
        /// Don't modify the content of this Hashtable. Use SetCustomProperties and the
        /// properties of this class to modify values. When you use those, the client will
        /// sync values with the server.
        /// </remarks>
        /// <see cref="SetCustomProperties"/>
        */

            
        /// <summary> 플레이어의 사용자 정의 속성에 대한 읽기 전용 캐시. Player.SetCustomProperties를 통해 설정하십시오. </summary>
        /// <remarks>
        /// 이 Hashtable의 내용을 변경하지 않습니다.SetCustomProperties 및
        /// 이 클래스의 속성으로 값을 수정합니다. 당신이 그들을 사용할 때, 클라이언트는
        /// 서버와 값을 동기화합니다.
        /// </remarks>
        /// <see cref = "SetCustomProperties"/>
        public Hashtable CustomProperties { get; set; }

        /*
        /// <summary>Can be used to store a reference that's useful to know "by player".</summary>
        /// <remarks>Example: Set a player's character as Tag by assigning the GameObject on Instantiate.</remarks>
        */

        /// <summary> "플레이어 별"을 아는 데 유용한 참조를 저장하는 데 사용할 수 있습니다. </summary>
        /// <remarks> 예 : Instantiate에서 GameObject를 지정하여 플레이어의 캐릭터를 태그로 설정합니다. </remarks>
        public object TagObject;

        /*
        /// <summary>
        /// Creates a player instance.
        /// To extend and replace this Player, override LoadBalancingPeer.CreatePlayer().
        /// </summary>
        /// <param name="nickName">NickName of the player (a "well known property").</param>
        /// <param name="actorNumber">ID or ActorNumber of this player in the current room (a shortcut to identify each player in room)</param>
        /// <param name="isLocal">If this is the local peer's player (or a remote one).</param>
        */

        /// <summary>
        /// 플레이어 인스턴스를 만듭니다.
        ///이 Player를 확장하고 바꾸려면 LoadBalancingPeer.CreatePlayer ()를 재정의합니다.
        /// </summary>
        /// <param name = "nickName"> 플레이어의 닉네임 ( "잘 알려진 속성"). </param>
        /// <param name = "actorNumber"> 현재 방에있는이 플레이어의 ID 또는 ActorNumber (방의 각 플레이어를 식별하는 지름길) </param>
        /// <param name = "isLocal"> 로컬 피어의 플레이어 (또는 원격 피어) 인 경우 </param>
        protected internal Player(string nickName, int actorNumber, bool isLocal) : this(nickName, actorNumber, isLocal, null)
        {
        }

        /*
        /// <summary>
        /// Creates a player instance.
        /// To extend and replace this Player, override LoadBalancingPeer.CreatePlayer().
        /// </summary>
        /// <param name="nickName">NickName of the player (a "well known property").</param>
        /// <param name="actorNumber">ID or ActorNumber of this player in the current room (a shortcut to identify each player in room)</param>
        /// <param name="isLocal">If this is the local peer's player (or a remote one).</param>
        /// <param name="playerProperties">A Hashtable of custom properties to be synced. Must use String-typed keys and serializable datatypes as values.</param>
        */ 

        /// <summary>
        /// 플레이어 인스턴스를 만듭니다.
        /// 이 Player를 확장하고 바꾸려면 LoadBalancingPeer.CreatePlayer ()를 재정의합니다.
        /// </summary>
        /// <param name = "nickName"> 플레이어의 닉네임 ( "잘 알려진 속성"). </param>
        /// <param name = "actorNumber"> 현재 방에있는이 플레이어의 ID 또는 ActorNumber (방의 각 플레이어를 식별하는 지름길) </param>
        /// <param name = "isLocal"> 로컬 피어의 플레이어 (또는 원격 피어) 인 경우 </param>
        /// <param name = "playerProperties"> 동기화 할 사용자 정의 속성의 해시 테이블. 문자열 형식의 키와 직렬화 가능한 데이터 유형을 값으로 사용해야합니다. </param>
        protected internal Player(string nickName, int actorNumber, bool isLocal, Hashtable playerProperties)
        {
            this.IsLocal = isLocal;
            this.actorNumber = actorNumber;
            this.NickName = nickName;

            this.CustomProperties = new Hashtable();
            this.InternalCacheProperties(playerProperties);
        }


        /*
        /// <summary>
        /// Get a Player by ActorNumber (Player.ID).
        /// </summary>
        /// <param name="id">ActorNumber of the a player in this room.</param>
        /// <returns>Player or null.</returns>
        */ 

        /// <summary>
        /// ActorNumber (Player.ID)별로 플레이어를 가져옵니다.
        /// </summary>
        /// <param name = "id">이 방에있는 플레이어의 ActorNumber입니다. </param>
        /// <returns> Player 또는 null. </returns>
        public Player Get(int id)
        {
            if (this.RoomReference == null)
            {
                return null;
            }

            return this.RoomReference.GetPlayer(id);
        }

        /*
        /// <summary>Gets this Player's next Player, as sorted by ActorNumber (Player.ID). Wraps around.</summary>
        /// <returns>Player or null.</returns>
        */

        /// <summary> ActorNumber (Player.ID)로 정렬하여이 Player의 다음 Player를 가져옵니다. 둘러보기. </summary>
        /// <returns> Player 또는 null. </returns>
        public Player GetNext()
        {
            return GetNextFor(this.ActorNumber);
        }

        /*
        /// <summary>Gets a Player's next Player, as sorted by ActorNumber (Player.ID). Wraps around.</summary>
        /// <remarks>Useful when you pass something to the next player. For example: passing the turn to the next player.</remarks>
        /// <param name="currentPlayer">The Player for which the next is being needed.</param>
        /// <returns>Player or null.</returns>
        */

        /// <summary> ActorNumber (Player.ID)에 의해 정렬 된 Player의 다음 Player를 가져옵니다. 둘러보기. </summary>
        /// <remarks> 다음 플레이어에게 무언가를 전달할 때 유용합니다. 예 : 다음 플레이어에게 턴을 넘깁니다. </remarks>
        /// <param name = "currentPlayer"> 다음에 필요한 플레이어. </param>
        /// <returns> Player 또는 null. </returns>
        public Player GetNextFor(Player currentPlayer)
        {
            if (currentPlayer == null)
            {
                return null;
            }
            return GetNextFor(currentPlayer.ActorNumber);
        }

        /*
        /// <summary>Gets a Player's next Player, as sorted by ActorNumber (Player.ID). Wraps around.</summary>
        /// <remarks>Useful when you pass something to the next player. For example: passing the turn to the next player.</remarks>
        /// <param name="currentPlayerId">The ActorNumber (Player.ID) for which the next is being needed.</param>
        /// <returns>Player or null.</returns>
        */

        /// <summary> ActorNumber (Player.ID)에 의해 정렬 된 Player의 다음 Player를 가져옵니다. 둘러보기. </summary>
        /// <remarks> 다음 플레이어에게 무언가를 전달할 때 유용합니다. 예 : 다음 플레이어에게 턴을 넘깁니다. </remarks>
        /// <param name = "currentPlayerId"> 다음에 필요한 ActorNumber (Player.ID). </param>
        /// <returns> Player 또는 null. </returns>
        public Player GetNextFor(int currentPlayerId)
        {
            if (this.RoomReference == null || this.RoomReference.Players == null || this.RoomReference.Players.Count < 2)
            {
                return null;
            }

            Dictionary<int, Player> players = this.RoomReference.Players;
            int nextHigherId = int.MaxValue;    // we look for the next higher ID
            int lowestId = currentPlayerId;     // if we are the player with the highest ID, there is no higher and we return to the lowest player's id

            foreach (int playerid in players.Keys)
            {
                if (playerid < lowestId)
                {
                    lowestId = playerid;        // less than any other ID (which must be at least less than this player's id).
                }
                else if (playerid > currentPlayerId && playerid < nextHigherId)
                {
                    nextHigherId = playerid;    // more than our ID and less than those found so far.
                }
            }

            //UnityEngine.Debug.LogWarning("Debug. " + currentPlayerId + " lower: " + lowestId + " higher: " + nextHigherId + " ");
            //UnityEngine.Debug.LogWarning(this.RoomReference.GetPlayer(currentPlayerId));
            //UnityEngine.Debug.LogWarning(this.RoomReference.GetPlayer(lowestId));
            //if (nextHigherId != int.MaxValue) UnityEngine.Debug.LogWarning(this.RoomReference.GetPlayer(nextHigherId));
            return (nextHigherId != int.MaxValue) ? players[nextHigherId] : players[lowestId];
        }


        /*
        /// <summary>Caches properties for new Players or when updates of remote players are received. Use SetCustomProperties() for a synced update.</summary>
        /// <remarks>
        /// This only updates the CustomProperties and doesn't send them to the server.
        /// Mostly used when creating new remote players, where the server sends their properties.
        /// </remarks>
        */

        /// <summary> 새로운 플레이어 또는 원격 플레이어의 업데이트를받을 때의 캐시 속성. 동기화 된 업데이트에는 SetCustomProperties ()를 사용하십시오. </summary>
        /// <remarks>
        ///이 명령은 CustomProperties 만 업데이트하고 서버로 보내지 않습니다.
        /// 서버가 속성을 보내는 새로운 원격 플레이어를 만들 때 주로 사용됩니다.
        /// </remarks>
        public virtual void InternalCacheProperties(Hashtable properties)
        {
            if (properties == null || properties.Count == 0 || this.CustomProperties.Equals(properties))
            {
                return;
            }

            if (properties.ContainsKey(ActorProperties.PlayerName))
            {
                string nameInServersProperties = (string)properties[ActorProperties.PlayerName];
                if (nameInServersProperties != null)
                {
                    if (this.IsLocal)
                    {
                        // the local playername is different than in the properties coming from the server
                        // so the local nickName was changed and the server is outdated -> update server
                        // update property instead of using the outdated nickName coming from server
                        if (!nameInServersProperties.Equals(this.nickName))
                        {
                            this.SetPlayerNameProperty();
                        }
                    }
                    else
                    {
                        this.NickName = nameInServersProperties;
                    }
                }
            }
            if (properties.ContainsKey(ActorProperties.UserId))
            {
                this.UserId = (string)properties[ActorProperties.UserId];
            }
            if (properties.ContainsKey(ActorProperties.IsInactive))
            {
                this.IsInactive = (bool)properties[ActorProperties.IsInactive]; //TURNBASED new well-known propery for players
            }

            this.CustomProperties.MergeStringKeys(properties);
            this.CustomProperties.StripKeysWithNullValues();
        }

        /// Brief summary string of the Player. Includes name or player.ID and if it's the Master Client.

        /// <summary>
        /// 플레이어 요약 문자열. 이름 또는 player.ID 및 마스터 클라이언트 인 경우 ID를 포함합니다.
        /// </summary>
        public override string ToString()
        {
            return (string.IsNullOrEmpty(this.NickName) ? this.ActorNumber.ToString() : this.nickName) + " " + SupportClass.DictionaryToString(this.CustomProperties);
        }

        /*
        /// <summary>
        /// String summary of the Player: player.ID, name and all custom properties of this user.
        /// </summary>
        /// <remarks>
        /// Use with care and not every frame!
        /// Converts the customProperties to a String on every single call.
        /// </remarks>
        */

        /// <summary>
        /// Player의 문자열 요약 :이 사용자의 player.ID, 이름 및 모든 사용자 정의 속성.
        /// </summary>
        /// <remarks>
        /// 모든 프레임이 아닌주의해서 사용하십시오!
        /// 매 호출마다 customProperties를 String으로 변환합니다.
        /// </remarks> 
        public string ToStringFull()
        {
            return string.Format("#{0:00} '{1}'{2} {3}", this.ActorNumber, this.NickName, this.IsInactive ? " (inactive)" : "", this.CustomProperties.ToStringFull());
        }

        // If players are equal (by GetHasCode, which returns this.ID).
        
        /// <summary>
        /// 플레이어가 동일한 경우 (GetHasCode에 의해 this.ID가 반환 됨).
        /// </summary>
        public override bool Equals(object p)
        {
            Player pp = p as Player;
            return (pp != null && this.GetHashCode() == pp.GetHashCode());
        }

        // Accompanies Equals, using the ID (actorNumber) as HashCode to return.
        
        /// <summary>
        /// HashCode로 ID(actorNumber)를 사용하여 반환합니다.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ActorNumber;
        }

        // Used internally, to update this client's playerID when assigned (doesn't change after assignment).

        /// <summary>
        /// 할당 된 경우이 클라이언트의 playerID를 업데이트하기 위해 내부적으로 사용됩니다(할당 후에 변경되지 않음).
        /// </summary>
        protected internal void ChangeLocalID(int newID)
        {
            if (!this.IsLocal)
            {
                //Debug.LogError("ERROR You should never change Player IDs!");
                return;
            }

            this.actorNumber = newID;
        }


        /*
        /// <summary>
        /// Updates and synchronizes this Player's Custom Properties. Optionally, expectedProperties can be provided as condition.
        /// </summary>
        /// <remarks>
        /// Custom Properties are a set of string keys and arbitrary values which is synchronized
        /// for the players in a Room. They are available when the client enters the room, as
        /// they are in the response of OpJoin and OpCreate.
        ///
        /// Custom Properties either relate to the (current) Room or a Player (in that Room).
        ///
        /// Both classes locally cache the current key/values and make them available as
        /// property: CustomProperties. This is provided only to read them.
        /// You must use the method SetCustomProperties to set/modify them.
        ///
        /// Any client can set any Custom Properties anytime (when in a room).
        /// It's up to the game logic to organize how they are best used.
        ///
        /// You should call SetCustomProperties only with key/values that are new or changed. This reduces
        /// traffic and performance.
        ///
        /// Unless you define some expectedProperties, setting key/values is always permitted.
        /// In this case, the property-setting client will not receive the new values from the server but
        /// instead update its local cache in SetCustomProperties.
        ///
        /// If you define expectedProperties, the server will skip updates if the server property-cache
        /// does not contain all expectedProperties with the same values.
        /// In this case, the property-setting client will get an update from the server and update it's
        /// cached key/values at about the same time as everyone else.
        ///
        /// The benefit of using expectedProperties can be only one client successfully sets a key from
        /// one known value to another.
        /// As example: Store who owns an item in a Custom Property "ownedBy". It's 0 initally.
        /// When multiple players reach the item, they all attempt to change "ownedBy" from 0 to their
        /// actorNumber. If you use expectedProperties {"ownedBy", 0} as condition, the first player to
        /// take the item will have it (and the others fail to set the ownership).
        ///
        /// Properties get saved with the game state for Turnbased games (which use IsPersistent = true).
        /// </remarks>
        /// <param name="propertiesToSet">Hashtable of Custom Properties to be set. </param>
        /// <param name="expectedValues">If non-null, these are the property-values the server will check as condition for this update.</param>
        /// <param name="webFlags">Defines if this SetCustomProperties-operation gets forwarded to your WebHooks. Client must be in room.</param>
        */

        /// <summary>
        /// 이 Player의 사용자 정의 속성을 업데이트하고 동기화합니다.선택적으로 expectedProperties가 조건으로 제공 될 수 있습니다.
        /// </summary>
        /// <remarks>
        /// 사용자 정의 속성은 문자열 키와 동기화되는 임의의 값의 집합입니다.
        /// 방 안에있는 플레이어들에게. 고객이 방에 들어가면 사용할 수 있습니다.
        /// 그들은 OpJoin과 OpCreate의 응답에 있습니다.
        ///
        /// 사용자 정의 속성은 (현재) 룸 또는 해당 룸의 플레이어와 관련됩니다.
        ///
        /// 두 클래스 모두 현재 키 / 값을 로컬로 캐시하고 다음 키로 사용할 수 있도록합니다.
        /// 속성 : CustomProperties. 이것은 읽기 전용입니다.
        /// SetCustomProperties 메서드를 사용하여 설정 / 수정해야합니다.
        ///
        /// 모든 클라이언트는 언제든지 (사용자가 방에있을 때) 사용자 지정 속성을 설정할 수 있습니다.
        ///
        /// 새로운 또는 변경된 키 / 값으로 만 SetCustomProperties를 호출해야합니다. 이렇게하면
        /// 교통 및 성능.
        ///
        /// expectedProperties를 정의하지 않으면 키 / 값 설정이 항상 허용됩니다.
        /// 이 경우 속성 설정 클라이언트는 서버에서 새 값을받지 않지만
        /// 대신 SetCustomProperties에서 로컬 캐시를 업데이트하십시오.
        ///
        /// expectedProperties를 정의하면 서버 property-cache
        /// 는 동일한 값을 가진 모든 expectedProperties를 포함하지 않습니다.
        /// 이 경우 속성 설정 클라이언트는 서버에서 업데이트를 가져와 업데이트합니다.
        /// 캐시 된 키 / 값은 다른 모든 사람들과 거의 같은 시간에 캐시됩니다.
        ///
        /// expectedProperties를 사용하면 한 클라이언트 만이 키를 성공적으로 설정할 수 있습니다.
        /// 한 가치를 다른 것으로 알려준다.
        /// 예 : 사용자 지정 속성 "ownedBy"에 항목을 소유 한 사용자를 저장합니다. 처음에는 0입니다.
        /// 여러 플레이어가 항목에 도달하면 모두 "ownedBy"를 0에서 자신의 값으로 변경하려고 시도합니다.
        /// actorNumber. expectedProperties { "ownedBy", 0}를 조건으로 사용하면 첫 번째 플레이어가
        /// 아이템을 가져 가면 (그리고 다른 것들은 소유권을 설정하지 못합니다).
        ///
        /// 속성은 Turnbased 게임 (IsPersistent = true 사용)의 게임 상태와 함께 저장됩니다.
        /// </remarks>
        /// <param name="propertiesToSet"> 설정할 수있는 사용자 정의 속성의 Hashtable입니다. </param>
        /// <param name="expectedValues">null가 아닌 경우는, 서버가 이 갱신의 조건으로서 체크하는 프로퍼티 - 값입니다.</param>
        /// <param name="webFlags">이 SetCustomProperties 작업이 WebHooks로 전달되는지 여부를 정의합니다. 고객이 방에 있어야합니다.</param>
        public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, WebFlags webFlags = null)
        {
            if (propertiesToSet == null)
            {
                return;
            }

            Hashtable customProps = propertiesToSet.StripToStringKeys() as Hashtable;
            Hashtable customPropsToCheck = expectedValues.StripToStringKeys() as Hashtable;


            // no expected values -> set and callback
            bool noCas = customPropsToCheck == null || customPropsToCheck.Count == 0;


            if (noCas)
            {
                this.CustomProperties.Merge(customProps);
                this.CustomProperties.StripKeysWithNullValues();
            }

            if (this.RoomReference != null)
            {
                if (this.RoomReference.IsOffline)
                {
                    // 콜백 호출하기
                    // invoking callbacks
                    this.RoomReference.LoadBalancingClient.InRoomCallbackTargets.OnPlayerPropertiesUpdate(this, customProps);
                }
                else
                {
                    // 온라인 방에서 이러한 새로운 값을 전송 (동기화)하십시오.
                    // send (sync) these new values if in online room
                    this.RoomReference.LoadBalancingClient.LoadBalancingPeer.OpSetPropertiesOfActor(this.actorNumber, customProps, customPropsToCheck, webFlags);
                }
            }
        }

        // Uses OpSetPropertiesOfActor to sync this player's NickName (server is being updated with this.NickName).
        /// <summary>OpSetPropertiesOfActor를 사용하여이 플레이어의 NickName(서버가 this.NickName으로 업데이트 됨)을 동기화합니다.</summary>
        private void SetPlayerNameProperty()
        {
            if (this.RoomReference != null)
            {
                Hashtable properties = new Hashtable();
                properties[ActorProperties.PlayerName] = this.nickName;
                this.RoomReference.LoadBalancingClient.LoadBalancingPeer.OpSetPropertiesOfActor(this.ActorNumber, properties);
            }
        }
    }
}