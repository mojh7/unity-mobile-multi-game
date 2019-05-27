// ----------------------------------------------------------------------------
// <copyright file="PunClasses.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Wraps up smaller classes that don't need their own file.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


#pragma warning disable 1587
/// \defgroup publicApi Public API
/// \brief Groups the most important classes that you need to understand early on.
///
/// \defgroup optionalGui Optional Gui Elements
/// \brief Useful GUI elements for PUN.
///
/// \defgroup callbacks Callbacks
/// \brief Callback Interfaces
#pragma warning restore 1587


/*
 * https://doc.photonengine.com/en-us/pun/v2/getting-started/dotnet-callbacks
 * 
 * IConnectionCallbacks: 연결 관련 콜백.
 * IInRoomCallbacks: 방 안의 콜백.
 * ILobbyCallbacks: 로비 관련 콜백.
 * IMatchmakingCallbacks: 중매 관련 콜백.
 * IOnEventCallback: 수신 된 모든 이벤트에 대한 단일 콜백. 이것은 C # 이벤트와 '동등한'것 LoadBalancingClient.EventReceived입니다.
 * IWebRpcCallback: WebRPC 조작 응답을 수신하기위한 단일 콜백.
 * IPunInstantiateMagicCallback: 인스턴스화 된 PUN 프리 팹에 대한 단일 콜백.
 * IPunObservable: PhotonView 직렬화 콜백.
 * IPunOwnershipCallbacks: PUN 소유권 이전 콜백.
 */


namespace Photon.Pun
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ExitGames.Client.Photon;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Photon.Realtime;
    using SupportClassPun = ExitGames.Client.Photon.SupportClass;


    //// <summary>Replacement for RPC attribute with different name. Used to flag methods as remote-callable.</summary>
    /// <summary> RPC 속성을 다른 이름으로 대체합니다. 메소드를 원격 호출 가능으로 플래그 지정하는 데 사용됩니다. </summary>
    public class PunRPC : Attribute
    {
    }
    /*
    /// <summary>Defines the OnPhotonSeri
    /// alizeView method to make it easy to implement correctly for observable scripts.</summary>
    /// \ingroup callbacks
    */
    /// <summary> OnPhotonSeri를 정의합니다.
    /// alizeView 메소드를 사용하여 관찰 가능한 스크립트에 대해 올바르게 구현할 수 있습니다. </summary>
    /// \ ingroup 콜백
    public interface IPunObservable
    {
        /*
        /// <summary>
        /// Called by PUN several times per second, so that your script can write and read synchronization data for the PhotonView.
        /// </summary>
        /// <remarks>
        /// This method will be called in scripts that are assigned as Observed component of a PhotonView.<br/>
        /// PhotonNetwork.SerializationRate affects how often this method is called.<br/>
        /// PhotonNetwork.SendRate affects how often packages are sent by this client.<br/>
        ///
        /// Implementing this method, you can customize which data a PhotonView regularly synchronizes.
        /// Your code defines what is being sent (content) and how your data is used by receiving clients.
        ///
        /// Unlike other callbacks, <i>OnPhotonSerializeView only gets called when it is assigned
        /// to a PhotonView</i> as PhotonView.observed script.
        ///
        /// To make use of this method, the PhotonStream is essential. It will be in "writing" mode" on the
        /// client that controls a PhotonView (PhotonStream.IsWriting == true) and in "reading mode" on the
        /// remote clients that just receive that the controlling client sends.
        ///
        /// If you skip writing any value into the stream, PUN will skip the update. Used carefully, this can
        /// conserve bandwidth and messages (which have a limit per room/second).
        ///
        /// Note that OnPhotonSerializeView is not called on remote clients when the sender does not send
        /// any update. This can't be used as "x-times per second Update()".
        /// </remarks>
        /// \ingroup publicApi
        /// 
         */
        /// <summary>
        /// PUN이 초당 여러 번 호출하므로 스크립트에서 PhotonView의 동기화 데이터를 읽고 쓸 수 있습니다.
        /// </summary>
        /// <remarks>
        /// 이 메소드는 PhotonView의 Observed 구성 요소로 지정된 스크립트에서 호출됩니다.<br/>
        /// PhotonNetwork.SerializationRate는이 메서드가 호출되는 빈도에 영향을줍니다.<br/>
        /// PhotonNetwork.SendRate는이 클라이언트가 패키지를 보내는 빈도에 영향을줍니다.<br/>
        ///
        /// 이 방법을 구현하면 PhotonView가 정기적으로 동기화 할 데이터를 사용자 정의 할 수 있습니다.
        /// 귀하의 코드는 전송되는 내용 (내용)과 수신하는 클라이언트가 귀하의 데이터를 사용하는 방법을 정의합니다.
        ///
        /// 다른 콜백과 달리 <i> OnPhotonSerializeView는 할당되었을 때만 호출됩니다.
        /// PhotonView.observed 스크립트로 PhotonView </i>로 이동하십시오.
        ///
        /// 이 방법을 사용하려면 PhotonStream이 필수적입니다. 그것은 "쓰기"모드에있을 것입니다.
        /// 클라이언트는 PhotonView (PhotonStream.IsWriting == true)를 제어하고
        /// 제어 클라이언트가 보내는 것을 수신 한 원격 클라이언트.
        ///
        /// 스트림에 값을 쓰지 않으면 PUN이 업데이트를 건너 뜁니다. 주의 깊게 사용하면
        /// 대역폭과 메시지를 보존합니다 (1 회의실 / 초당 제한이 있음).
        ///
        /// OnPhotonSerializeView는 보낸 사람이 보내지 않을 때 원격 클라이언트에서 호출되지 않습니다.
        /// 모든 업데이트. 이것은 "초당 x 번 업데이트 ()"로 사용할 수 없습니다.
        /// </remarks>
        /// \ingroup publicApi
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
    }

    /*
    /// <summary>
    /// This interface is used as definition of all callback methods of PUN, except OnPhotonSerializeView. Preferably, implement them individually.
    /// </summary>
    /// <remarks>
    /// This interface is available for completeness, more than for actually implementing it in a game.
    /// You can implement each method individually in any MonoMehaviour, without implementing IPunCallbacks.
    ///
    /// PUN calls all callbacks by name. Don't use implement callbacks with fully qualified name.
    /// Example: IPunCallbacks.OnConnected won't get called by Unity's SendMessage().
    ///
    /// PUN will call these methods on any script that implements them, analog to Unity's events and callbacks.
    /// The situation that triggers the call is described per method.
    ///
    /// OnPhotonSerializeView is NOT called like these callbacks! It's usage frequency is much higher and it is implemented in: IPunObservable.
    /// </remarks>
    /// \ingroup callbacks
    */


    /// <summary>
    ///이 인터페이스는 OnPhotonSerializeView를 제외한 PUN의 모든 콜백 메소드 정의로 사용됩니다. 바람직하게는 개별적으로 구현하십시오.
    /// </summary>
    /// <remarks>
    ///이 인터페이스는 게임에서 실제로 구현하는 것 이상으로 완벽하게 사용할 수 있습니다.
    /// IPunCallbacks를 구현하지 않고 MonoMehaviour에서 개별적으로 각 메서드를 구현할 수 있습니다.
    ///
    /// PUN은 모든 콜백을 이름으로 호출합니다. 정규화 된 이름으로 구현 콜백을 사용하지 마십시오.
    /// 예제 : IPunCallbacks.OnConnected는 Unity의 SendMessage ()에 의해 호출되지 않습니다.
    ///
    /// PUN은 유니티의 이벤트와 콜백과 유사하게 그것들을 구현하는 모든 스크립트에서이 메소드를 호출 할 것입니다.
    /// 호출을 트리거하는 상황은 메소드별로 설명됩니다.
    ///
    /// OnPhotonSerializeView는 이러한 콜백처럼 호출되지 않습니다! 사용 빈도는 훨씬 높으며 IPunObservable에서 구현됩니다.
    /// </remarks>
    /// \ingroup 콜백
    public interface IPunOwnershipCallbacks
    {
        /// <summary>
        /// 다른 플레이어가 당신 (현재 소유자)의 PhotonView 소유권을 요청하면 호출됩니다.
        /// Called when another player requests ownership of a PhotonView from you (the current owner).
        /// </summary>
        /// <remarks>
        /// viewAndPlayer 매개 변수는 다음을 포함합니다.
        /// The parameter viewAndPlayer contains:
        ///
        /// PhotonView view = viewAndPlayer[0] as PhotonView;
        ///
        /// Player requestingPlayer = viewAndPlayer[1] as Player;
        /// </remarks>
        /// <param name="targetView">소유권이 요청 된 PhotonView. PhotonView for which ownership gets requested.</param>
        /// <param name="requestingPlayer">소유권을 요청하는 플레이어. Player who requests ownership.</param>
        void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer);

        /// <summary>
        /// PhotonView의 소유권이 다른 플레이어에게 이전 될 때 호출됩니다.
        /// Called when ownership of a PhotonView is transfered to another player.
        /// </summary>
        /// <remarks>
        /// viewAndPlayers 매개 변수에는 다음이 포함됩니다.
        /// The parameter viewAndPlayers contains:
        ///
        /// PhotonView view = viewAndPlayers[0] as PhotonView;
        ///
        /// Player newOwner = viewAndPlayers[1] as Player;
        ///
        /// Player oldOwner = viewAndPlayers[2] as Player;
        /// </remarks>
        /// <example>void OnOwnershipTransfered(object[] viewAndPlayers) {} //</example>
        /// <param name="targetView">소유권이 변경된 PhotonView. PhotonView for which ownership changed.</param>
        /// <param name="previousOwner">이전 소유자 인 플레이어 (없으면 null). Player who was the previous owner (or null, if none).</param>
        void OnOwnershipTransfered(PhotonView targetView, Player previousOwner);
    }

    /// \ingroup callbacks
    public interface IPunInstantiateMagicCallback
    {
        void OnPhotonInstantiate(PhotonMessageInfo info);
    }

    /*
    /// <summary>
    /// Defines an interface for object pooling, used in PhotonNetwork.Instantiate and PhotonNetwork.Destroy.
    /// </summary>
    /// <remarks>
    /// To apply your custom IPunPrefabPool, set PhotonNetwork.PrefabPool.
    ///
    /// The pool has to return a valid, disabled GameObject when PUN calls Instantiate.
    /// Also, the position and rotation must be applied.
    ///
    /// Note that Awake and Start are only called once by Unity, so scripts on re-used GameObjects
    /// should make use of OnEnable and or OnDisable. When OnEnable gets called, the PhotonView
    /// is already updated to the new values.
    ///
    /// To be able to enable a GameObject, Instantiate must return an inactive object.
    ///
    /// Before PUN "destroys" GameObjects, it will disable them. 
    ///
    /// If a component implements IPunInstantiateMagicCallback, PUN will call OnPhotonInstantiate
    /// when the networked object gets instantiated. If no components implement this on a prefab,
    /// PUN will optimize the instantiation and no longer looks up IPunInstantiateMagicCallback
    /// via GetComponents.
    /// </remarks>
     */

    /// <summary>
    /// PhotonNetwork.Instantiate 및 PhotonNetwork.Destroy에 사용되는 개체 풀링을위한 인터페이스를 정의합니다.
    /// </summary>
    /// <remarks>
    /// 사용자 지정 IPunPrefabPool을 적용하려면 PhotonNetwork.PrefabPool을 설정하십시오.
    /// PUN이 Instantiate를 호출 할 때 풀에서 유효하지 않은 GameObject를 반환해야합니다.
    /// 또한 위치와 회전을 적용해야합니다.
    /// Awake와 Start는 Unity에 한 번만 호출되므로 재사용 된 GameObjects의 스크립트
    /// OnEnable 및 / 또는 OnDisable을 사용해야합니다. OnEnable이 호출되면 PhotonView 가 이미 새 값으로 업데이트되었습니다.
    ///
    /// GameObject를 활성화하려면 Instantiate가 비활성 객체를 반환해야합니다.
    ///
    /// PUN이 GameObjects를 "파괴"하기 전에, PUN이 게임 오브젝트를 비활성화시킵니다.
    ///
    /// 구성 요소가 IPunInstantiateMagicCallback을 구현하면 PUN은 OnPhotonInstantiate
    /// 네트워크 객체가 인스턴스화되면. 프리 패브에서 이것을 구현하는 컴포넌트가 없다면,
    /// PUN은 인스턴스화를 최적화하고 더 이상 IPunInstantiateMagicCallback을 찾지 않습니다.
    /// GetComponents를 통해.
    /// </remarks>
    public interface IPunPrefabPool
    {
        /// <summary>
        /// 프리 패브의 인스턴스를 가져 오기 위해 호출됩니다.PhotonView로 유효한, 비활성화 된 GameObject를 반환해야합니다.
        /// Called to get an instance of a prefab. Must return valid, disabled GameObject with PhotonView.
        /// </summary>
        /// <param name="prefabId">The id of this prefab.</param>
        /// <param name="position">The position for the instance.</param>
        /// <param name="rotation">The rotation for the instance.</param>
        /// <returns>A disabled instance to use by PUN or null if the prefabId is unknown.</returns>
        GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);

        /// <summary>
        /// 프리 패브의 인스턴스를 파괴 (또는 방금 반환)하기 위해 호출됩니다. 사용할 수 없으며 풀이 재설정되어 나중에 인스턴스화에 사용할 수 있도록 캐시 할 수 있습니다.
        /// Called to destroy (or just return) the instance of a prefab. It's disabled and the pool may reset and cache it for later use in Instantiate.
        /// </summary>
        /// <remarks>
        /// A pool needs some way to find out which type of GameObject got returned via Destroy().
        /// It could be a tag, name, a component or anything similar.
        /// </remarks>
        /// <param name="gameObject">The instance to destroy.</param>
        void Destroy(GameObject gameObject);
    }


    /// <summary>
    /// 이 클래스는 photonView 속성을 추가하고 게임에서 여전히 networkView를 사용할 때 경고를 기록합니다.
    /// This class adds the property photonView, while logging a warning when your game still uses the networkView.
    /// </summary>
    public class MonoBehaviourPun : MonoBehaviour
    {
        /// <summary>Cache field for the PhotonView on this GameObject.</summary>
        private PhotonView pvCache;

        /// <summary>이 GameObject의 PhotonView에 대한 캐시 된 참조.</summary>
        /// <remarks>
        /// 스크립트에서 PhotonView로 작업하려면 보통 this.photonView를 쓰는 것이 더 쉽습니다.
        ///
        /// GameObject에서 PhotonView 구성 요소를 제거하지만이 Photon.MonoBehaviour를 유지하려는 경우,
        /// 이 참조를 피하거나 대신이 코드를 수정하여 PhotonView.Get (obj)를 사용하십시오.
        /// If you intend to work with a PhotonView in a script, it's usually easier to write this.photonView.
        ///
        /// If you intend to remove the PhotonView component from the GameObject but keep this Photon.MonoBehaviour,
        /// avoid this reference or modify this code to use PhotonView.Get(obj) instead.
        /// </remarks>
        public PhotonView photonView
        {
            get
            {
                if (this.pvCache == null)
                {
                    this.pvCache = PhotonView.Get(this);
                }
                return this.pvCache;
            }
        }
    }

    /* 이 클래스는.photonView와 PUN이 호출 할 수있는 모든 콜백 / 이벤트를 제공합니다. 사용할 이벤트 / 메소드를 재정의하십시오.
     * 이 클래스를 확장하면 개별 메서드를 재정의로 구현할 수 있습니다.
     * Visual Studio 및 MonoDevelop는 "재정의"를 시작할 때 메서드 목록을 제공해야합니다.
     */ 

    /// <summary>
    /// This class provides a .photonView and all callbacks/events that PUN can call. Override the events/methods you want to use.
    /// </summary>
    /// <remarks>
    /// By extending this class, you can implement individual methods as override.
    /// Visual Studio and MonoDevelop should provide the list of methods when you begin typing "override". 
    /// <b>Your implementation does not have to call "base.method()".</b>
    ///
    /// This class implements all callback interfaces and extends <see cref="Photon.Pun.MonoBehaviourPun"/>.
    /// </remarks>
    /// \ingroup callbacks
    // the documentation for the interface methods becomes inherited when Doxygen builds it.
    public class MonoBehaviourPunCallbacks : MonoBehaviourPun, IConnectionCallbacks , IMatchmakingCallbacks , IInRoomCallbacks, ILobbyCallbacks
    {
        public virtual void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public virtual void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        /* 원시 연결이 설정되었지만 클라이언트가 서버에서 작업을 호출하기 전에 신호를 보내도록 호출되었습니다.
         * (낮은 수준의 전송) 연결이 설정된 후 클라이언트는 자동으로 인증 작업. 클라이언트가 다른 작업을 호출하기 전에 응답을 받아야합니다.
         * 논리는 OnRegionListReceived 또는 OnConnectedToMaster 중 하나를 기다려야합니다.
         * 이 콜백은 서버에 전혀 도달 할 수 있는지 (기술적으로) 감지하는 데 유용합니다.
         * OnDisconnected ()를 구현하는 것으로 충분합니다.
         * 마스터 서버에서 게임 서버로 전환 할 때 호출되지 않습니다.
         */

        /// <summary>
        /// Called to signal that the raw connection got established but before the client can call operation on the server.
        /// </summary>
        /// <remarks>
        /// After the (low level transport) connection is established, the client will automatically send
        /// the Authentication operation, which needs to get a response before the client can call other operations.
        ///
        /// Your logic should wait for either: OnRegionListReceived or OnConnectedToMaster.
        ///
        /// This callback is useful to detect if the server can be reached at all (technically).
        /// Most often, it's enough to implement OnDisconnected().
        ///
        /// This is not called for transitions from the masterserver to game servers.
        /// </remarks>
        public virtual void OnConnected()
        {
        }

        /// <summary>
        /// Called when the local user/client left a room, so the game's logic can clean up it's internal state.
        /// 로컬 사용자 / 클라이언트가 방을 떠날 때 호출되므로 게임 논리가 내부 상태를 정리할 수 있습니다.
        /// </summary>
        /// <remarks>
        /// When leaving a room, the LoadBalancingClient will disconnect the Game Server and connect to the Master Server.
        /// This wraps up multiple internal actions.
        ///
        /// Wait for the callback OnConnectedToMaster, before you use lobbies and join or create rooms.
        /// </remarks>
        /// 

        /// <summary>
        /// 로컬 사용자 / 클라이언트가 방을 떠날 때 호출되므로 게임 논리가 내부 상태를 정리할 수 있습니다.
        /// </summary>
        /// <remarks>
        /// 방을 떠날 때, LoadBalancingClient는 게임 서버를 분리하고 마스터 서버에 연결합니다.
        /// 여러 개의 내부 동작을 마무리합니다.
        ///
        /// 로비를 사용하고 회의실에 가입하거나 회의실을 만들기 전에 콜백 OnConnectedToMaster를 기다립니다.
        /// </remarks>
        public virtual void OnLeftRoom()
        {
        }

        /* 로컬 사용자 / 클라이언트가 방을 떠날 때 호출되므로 게임 논리가 내부 상태를 정리할 수 있습니다.
         * 방을 떠날 때, LoadBalancingClient는 게임 서버를 분리하고 마스터 서버에 연결합니다.
         * 여러 내부 동작을 마무리합니다.
         * 로비를 사용하고 회의실에 참여하거나 회의실을 만들기 전에 콜백 OnConnectedToMaster를 기다립니다.
         */
        /// <summary>
        /// Called after switching to a new MasterClient when the current one leaves.
        /// </summary>
        /// <remarks>
        /// This is not called when this client enters a room.
        /// The former MasterClient is still in the player list when this method get called.
        /// </remarks>
        public virtual void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        /* 서버가 공간을 생성 할 수 없을 때 호출됩니다 (OpCreateRoom 실패).
         * <비고> 방을 만드는 데 실패하는 가장 일반적인 원인은 제목이 고정 된 방 이름에 의존하고 방이 이미있는 경우입니다.
         */

        /// <summary>
        /// Called when the server couldn't create a room (OpCreateRoom failed).
        /// </summary>
        /// <remarks>
        /// The most common cause to fail creating a room, is when a title relies on fixed room-names and the room already exists.
        /// </remarks>
        /// <param name="returnCode">Operation ReturnCode from the server.</param>
        /// <param name="message">Debug message for the error.</param>
        public virtual void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        /* 이전 OpJoinRoom 호출이 서버에서 실패한 경우 호출됩니다.
         * 
         * 
         */

        /// <summary>
        /// Called when a previous OpJoinRoom call failed on the server.
        /// 가장 일반적인 원인은 방이 가득 찼거나 존재하지 않는다는 것입니다 (누군가 다른 사람이 더 빠르거나 방을 닫았 기 때문에).
        /// </summary>
        /// <remarks>
        /// The most common causes are that a room is full or does not exist (due to someone else being faster or closing the room).
        /// </remarks>
        /// <param name="returnCode">Operation ReturnCode from the server.</param>
        /// <param name="message">Debug message for the error.</param>
        public virtual void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        /* 이 클라이언트가 방을 만들고 입력 할 때 호출됩니다. OnJoinedRoom ()도 호출됩니다.
         * 이 콜백은 방을 만든 클라이언트에서만 호출됩니다 (OpCreateRoom 참조).
         * 모든 클라이언트가 언제든지 연결을 끊을 수 있기 때문에 (또는 연결이 끊어 질 수 있습니다.)
         * 방의 작성자는 OnCreatedRoom을 실행하지 않습니다.
         * 특정 객실 속성 또는 "시작 신호"가 필요한 경우 OnMasterClientSwitched ()를 구현하십시오.
         * 각각의 새로운 MasterClient가 방의 상태를 확인하도록하십시오.
         */

        /// <summary>
        /// Called when this client created a room and entered it. OnJoinedRoom() will be called as well.
        /// </summary>
        /// <remarks>
        /// This callback is only called on the client which created a room (see OpCreateRoom).
        ///
        /// As any client might close (or drop connection) anytime, there is a chance that the
        /// creator of a room does not execute OnCreatedRoom.
        ///
        /// If you need specific room properties or a "start signal", implement OnMasterClientSwitched()
        /// and make each new MasterClient check the room's state.
        /// </remarks>
        public virtual void OnCreatedRoom()
        {
        }


        /* 마스터 서버의 로비에 입장 할 때 호출됩니다. 실제 룸 목록 업데이트는 OnRoomListUpdate를 호출합니다.
         * 로비에있는 동안 룸 목록은 고정 된 간격으로 자동 업데이트됩니다 (공개 클라우드에서는 수정할 수 없음).
         * 방 목록은 OnRoomListUpdate를 통해 사용할 수 있습니다.
         */

        /// <summary>
        /// Called on entering a lobby on the Master Server. The actual room-list updates will call OnRoomListUpdate.
        /// </summary>
        /// <remarks>
        /// While in the lobby, the roomlist is automatically updated in fixed intervals (which you can't modify in the public cloud).
        /// The room list gets available via OnRoomListUpdate.
        /// </remarks>
        public virtual void OnJoinedLobby()
        {
        }

        /* 로비를 떠난 후 호출됩니다.
         * 로비를 나갈 때 [OpCreateRoom] (@ref OpCreateRoom) 및 [OpJoinRandomRoom] (@ref OpJoinRandomRoom)
         * 자동으로 기본 로비를 참조하십시오.
         * 
         */

        /// <summary>
        /// 로비를 떠난 후 호출됩니다.
        /// </summary>
        /// <remarks>
        /// When you leave a lobby, [OpCreateRoom](@ref OpCreateRoom) and [OpJoinRandomRoom](@ref OpJoinRandomRoom)
        /// automatically refer to the default lobby.
        /// </remarks>
        public virtual void OnLeftLobby()
        {
        }

        /* Photon 서버와의 연결을 끊은 후에 호출됩니다. 실패이거나 의도적 일 수 있습니다.
         * 이 연결 끊기 이유는 DisconnectCause로 제공됩니다.
         * 
         */

        /// <summary>
        /// Photon 서버와의 연결을 끊은 후에 호출됩니다.실패이거나 의도적 일 수 있습니다.
        /// </summary>
        /// <remarks>
        /// The reason for this disconnect is provided as DisconnectCause.
        /// </remarks>
        public virtual void OnDisconnected(DisconnectCause cause)
        {
        }

        /* 이름 서버가 귀하의 제목에 대한 지역 목록을 제공했을 때 호출됩니다.
         * 제공된 값을 사용하려면 RegionHandler 클래스 설명을 확인하십시오.
         * 
         */

        /// <summary>
        /// 이름 서버가 귀하의 제목에 대한 지역 목록을 제공했을 때 호출됩니다.
        /// </summary>
        /// <remarks>Check the RegionHandler class description, to make use of the provided values.</remarks>
        /// <param name="regionHandler">The currently used RegionHandler.</param>
        public virtual void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        /* 마스터 서버의 로비 (InLobby)에있는 동안 객실 목록 업데이트가 필요합니다.
         * 각 항목은 사용자 정의 속성을 포함 할 수있는 RoomInfo입니다 (방을 만들 때 로비 목록으로 정의 된 항목이 제공됨).
         * 모든 종류의 로비가 고객에게 객실 목록을 제공하는 것은 아닙니다. 일부는 침묵하고 서버 쪽 매치 메이킹을 전문으로합니다.
         */

        /// <summary>
        /// Called for any update of the room-listing while in a lobby (InLobby) on the Master Server.
        /// </summary>
        /// <remarks>
        /// Each item is a RoomInfo which might include custom properties (provided you defined those as lobby-listed when creating a room).
        /// Not all types of lobbies provide a listing of rooms to the client. Some are silent and specialized for server-side matchmaking.
        /// </remarks>
        public virtual void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        /* LoadBalancingClient가 방을 입력 할 때 호출됩니다.이 클라이언트가 방을 작성했거나 간단하게 합류 한 경우에도 상관 없습니다.
         * 이 메서드를 호출하면 Room.Players의 기존 플레이어, 해당 사용자 지정 속성 및 Room.CustomProperties에 액세스 할 수 있습니다.
         * 이 콜백에서 플레이어 개체를 만들 수 있습니다. 예를 들어 Unity에서 플레이어의 프리 패브를 인스턴스화합니다.
         * 일치 항목을 "적극적으로"시작하려면 사용자가 OpRaiseEvent 또는 사용자 정의 속성을 사용하여 "준비"신호를 보냅니다.
         */
        /// <summary>
        /// Called when the LoadBalancingClient entered a room, no matter if this client created it or simply joined.
        /// </summary>
        /// <remarks>
        /// When this is called, you can access the existing players in Room.Players, their custom properties and Room.CustomProperties.
        ///
        /// In this callback, you could create player objects. For example in Unity, instantiate a prefab for the player.
        ///
        /// If you want a match to be started "actively", enable the user to signal "ready" (using OpRaiseEvent or a Custom Property).
        /// </remarks>
        public virtual void OnJoinedRoom()
        {
        }

        /* 
        /// 원격 플레이어가 방에 들어 왔을 때 호출됩니다. 이 플레이어는 이미 playerlist에 추가되었습니다.
        /// </summary>
        /// <비고>
        /// 게임이 특정 수의 플레이어로 시작한다면,이 콜백은
        /// Room.playerCount를 시작하고 시작할 수 있는지 확인하십시오.
        /// </ remarkarks>
         * 
         * 
         */
        /// <summary>
        /// Called when a remote player entered the room. This Player is already added to the playerlist.
        /// </summary>
        /// <remarks>
        /// If your game starts with a certain number of players, this callback can be useful to check the
        /// Room.playerCount and find out if you can start.
        /// </remarks>
        public virtual void OnPlayerEnteredRoom(Player newPlayer)
        {
        }
        

        /*
        /// <summary>
        /// 원격 플레이어가 방을 떠났거나 비활성 상태가되었을 때 호출됩니다. otherPlayer.IsInactive를 확인하십시오.
        /// </summary>
        /// <비고>
        /// 다른 플레이어가 방을 나가거나 연결이 끊어진 것으로 서버가 감지하면이 콜백은
        /// 게임 논리를 알리는 데 사용됩니다.
        ///
        /// 실내 설정에 따라 플레이어가 비활성 상태가되어 다시 돌아올 수 있습니다.
        /// 그들 방에있는 그들의 자리. 이 경우 Player는 Room.Players 사전에 있습니다.
        ///
        /// 플레이어가 단지 비활성 상태가 아니라면 Room.Players 사전에서 제거됩니다.
        /// 콜백이 호출됩니다.
        /// </ remarkarks>
         */ 
        /// <summary>
        /// Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
        /// </summary>
        /// <remarks>
        /// If another player leaves the room or if the server detects a lost connection, this callback will
        /// be used to notify your game logic.
        ///
        /// Depending on the room's setup, players may become inactive, which means they may return and retake
        /// their spot in the room. In such cases, the Player stays in the Room.Players dictionary.
        ///
        /// If the player is not just inactive, it gets removed from the Room.Players dictionary, before
        /// the callback is called.
        /// </remarks>
        public virtual void OnPlayerLeftRoom(Player otherPlayer)
        {
        }


        /*
        /// <summary>
        /// 이전 OpJoinRandom 호출이 서버에서 실패한 경우 호출됩니다.
        /// </summary>
        /// <비고>
        /// 가장 일반적인 원인은 방이 가득 찼거나 존재하지 않는다는 것입니다 (누군가 다른 사람이 더 빠르거나 방을 닫음으로 인해).
        ///
        /// 여러 로비 (OpJoinLobby 또는 TypedLobby 매개 변수를 통해)를 사용하는 경우 다른 로비에 더 / 피팅 룸이있을 수 있습니다. <br/>
        /// </ remarkarks>
        /// <param name = "returnCode"> 서버의 Operation ReturnCode </ param>
        /// <param name = "message"> 오류에 대한 디버그 메시지입니다. </ param>
         */
        /// <summary>
        /// Called when a previous OpJoinRandom call failed on the server.
        /// </summary>
        /// <remarks>
        /// The most common causes are that a room is full or does not exist (due to someone else being faster or closing the room).
        ///
        /// When using multiple lobbies (via OpJoinLobby or a TypedLobby parameter), another lobby might have more/fitting rooms.<br/>
        /// </remarks>
        /// <param name="returnCode">Operation ReturnCode from the server.</param>
        /// <param name="message">Debug message for the error.</param>
        public virtual void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        /// <summary>
        /// 클라이언트가 마스터 서버에 연결되어 있고 중매 및 기타 작업 준비가 완료되면 호출됩니다.
        /// </summary>
        /// <remarks>
        /// The list of available rooms won't become available unless you join a lobby via LoadBalancingClient.OpJoinLobby.
        /// You can join rooms and create them even without being in a lobby. The default lobby is used in that case.
        /// </remarks>
        public virtual void OnConnectedToMaster()
        {
        }

        /// <summary>
        /// 방의 사용자 정의 속성이 변경되면 호출됩니다. PropertyThatChanged는 Room.SetCustomProperties를 통해 설정된 모든 것을 포함합니다.
        /// </summary>
        /// <remarks>
        /// Since v1.25 this method has one parameter: Hashtable propertiesThatChanged.<br/>
        /// Changing properties must be done by Room.SetCustomProperties, which causes this callback locally, too.
        /// </remarks>
        /// <param name="propertiesThatChanged"></param>
        public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        /// <summary>
        /// 사용자 정의 플레이어 속성이 변경되면 호출됩니다. 플레이어와 변경된 속성은 object []로 전달됩니다.
        /// </summary>
        /// <remarks>
        /// Changing properties must be done by Player.SetCustomProperties, which causes this callback locally, too.
        /// </remarks>
        ///
        /// <param name="targetPlayer">Contains Player that changed.</param>
        /// <param name="changedProps">Contains the properties that changed.</param>
        public virtual void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
        {
        }

        /// <summary>
        /// 서버가 FindFriends 요청에 응답을 보낼 때 호출됩니다.
        /// Called when the server sent the response to a FindFriends request.
        /// </summary>
        /// <remarks>
        /// After calling OpFindFriends, the Master Server will cache the friend list and send updates to the friend
        /// list. The friends includes the name, userId, online state and the room (if any) for each requested user/friend.
        ///
        /// Use the friendList to update your UI and store it, if the UI should highlight changes.
        /// </remarks>
        public virtual void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        /// <summary>
        /// 사용자 정의 인증 서비스가 추가 데이터로 응답 할 때 호출됩니다.
        /// Called when your Custom Authentication service responds with additional data.
        /// </summary>
        /// <remarks>
        /// Custom Authentication services can include some custom data in their response.
        /// When present, that data is made available in this callback as Dictionary.
        /// While the keys of your data have to be strings, the values can be either string or a number (in Json).
        /// You need to make extra sure, that the value type is the one you expect. Numbers become (currently) int64.
        ///
        /// Example: void OnCustomAuthenticationResponse(Dictionary&lt;string, object&gt; data) { ... }
        /// </remarks>
        /// <see cref="https://doc.photonengine.com/en-us/realtime/current/reference/custom-authentication"/>
        public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        /// <summary>
        /// 사용자 지정 인증에 실패하면 호출됩니다. 연결 해제!
        /// Called when the custom authentication failed. Followed by disconnect!
        /// </summary>
        /// <remarks>
        /// Custom Authentication can fail due to user-input, bad tokens/secrets.
        /// If authentication is successful, this method is not called. Implement OnJoinedLobby() or OnConnectedToMaster() (as usual).
        ///
        /// During development of a game, it might also fail due to wrong configuration on the server side.
        /// In those cases, logging the debugMessage is very important.
        ///
        /// Unless you setup a custom authentication service for your app (in the [Dashboard](https://dashboard.photonengine.com)),
        /// this won't be called!
        /// </remarks>
        /// <param name="debugMessage">Contains a debug message why authentication failed. This has to be fixed during development.</param>
        public virtual void OnCustomAuthenticationFailed (string debugMessage)
        {
        }


        // 이것을 구현해야하는지 확인하십시오.
        //TODO: Check if this needs to be implemented
        // in: IOptionalInfoCallbacks
        public virtual void OnWebRpcResponse(OperationResponse response)
        {
        }

        // 이것을 구현해야하는지 확인하십시오.
        //TODO: Check if this needs to be implemented
        // in: IOptionalInfoCallbacks
        public virtual void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }
    }


    /// <summary>
    /// 특정 메시지, RPC 또는 업데이트에 대한 정보를 담는 Container 클래스입니다.
    /// Container class for info about a particular message, RPC or update.
    /// </summary>
    /// \ingroup publicApi
    public struct PhotonMessageInfo
    {
        private readonly int timeInt;
        /// <summary> 메시지 / 이벤트의 발신자입니다.null의 경우가 있습니다. The sender of a message / event. May be null.</summary>
        public readonly Player Sender;
        public readonly PhotonView photonView;

        public PhotonMessageInfo(Player player, int timestamp, PhotonView view)
        {
            this.Sender = player;
            this.timeInt = timestamp;
            this.photonView = view;
        }

        [Obsolete("Use SentServerTime instead.")]
        public double timestamp
        {
            get
            {
                uint u = (uint) this.timeInt;
                double t = u;
                return t / 1000.0d;
            }
        }

        public double SentServerTime
        {
            get
            {
                uint u = (uint)this.timeInt;
                double t = u;
                return t / 1000.0d;
            }
        }

        public int SentServerTimestamp
        {
            get { return this.timeInt; }
        }

        public override string ToString()
        {
            return string.Format("[PhotonMessageInfo: Sender='{1}' Senttime={0}]", this.SentServerTime, this.Sender);
        }
    }



    /// <summary>PUN에서 사용되는 Photon 이벤트 코드를 정의합니다. Defines Photon event-codes as used by PUN.</summary>
    internal class PunEvent
    {
        public const byte RPC = 200;
        public const byte SendSerialize = 201;
        public const byte Instantiation = 202;
        public const byte CloseConnection = 203;
        public const byte Destroy = 204;
        public const byte RemoveCachedRPCs = 205;
        public const byte SendSerializeReliable = 206; // TS: 이것을 추가했지만 더 이상 필요하지 않습니다. added this but it's not really needed anymore 
        public const byte DestroyPlayer = 207; // TS: added to make others remove all GOs of a player
        public const byte OwnershipRequest = 209;
        public const byte OwnershipTransfer = 210;
        public const byte VacantViewIds = 211;
    }

    /*
    /// <summary>
    /// This container is used in OnPhotonSerializeView() to either provide incoming data of a PhotonView or for you to provide it.
    /// </summary>
    /// <remarks>
    /// The IsWriting property will be true if this client is the "owner" of the PhotonView (and thus the GameObject).
    /// Add data to the stream and it's sent via the server to the other players in a room.
    /// On the receiving side, IsWriting is false and the data should be read.
    ///
    /// Send as few data as possible to keep connection quality up. An empty PhotonStream will not be sent.
    ///
    /// Use either Serialize() for reading and writing or SendNext() and ReceiveNext(). The latter two are just explicit read and
    /// write methods but do about the same work as Serialize(). It's a matter of preference which methods you use.
    /// </remarks>
    /// \ingroup publicApi
     * 
     */

    /// <summary>
    /// 이 컨테이너는 OnPhotonSerializeView ()에서 PhotonView의 들어오는 데이터를 제공하거나 사용자가 제공하기 위해 사용됩니다.
    /// </summary>
    /// <remarks>
    /// IsWriting 속성은이 클라이언트가 PhotonView (따라서 GameObject)의 "소유자"이면 참이됩니다.
    /// 스트림에 데이터를 추가하면 서버를 통해 방의 다른 플레이어에게 전송됩니다.
    /// 수신 측에서는 IsWriting이 거짓이고 데이터를 읽어야합니다.
    ///
    /// 가능한 한 데이터를 보내서 연결 품질을 높입니다. 빈 PhotonStream은 전송되지 않습니다.
    ///
    /// Serialize ()를 사용하여 읽기 및 쓰기를 수행하거나 SendNext () 및 ReceiveNext ()를 사용합니다. 후자의 두 가지는 단지 명시 적 읽기 및
    /// 메서드를 작성하지만 Serialize ()와 동일한 작업을 수행합니다. 당신이 사용하는 방법이 선호하는 문제입니다.
    /// </remarks>
    /// \ingroup publicApi
    public class PhotonStream
    {
        private List<object> writeData;
        private object[] readData;
        private byte currentItem; //수신 할 다음 항목을 추적하는 데 사용됩니다. Used to track the next item to receive.

        /// <summary> true의 경우, 이 클라이언트는 스트림에 데이터를 추가하여 전송해야합니다. If true, this client should add data to the stream to send it.</summary>
        public bool IsWriting { get; private set; }

        /// <summary> true이면이 클라이언트는 다른 클라이언트가 보내는 데이터를 읽어야합니다. If true, this client should read data send by another client.</summary>
        public bool IsReading
        {
            get { return !this.IsWriting; }
        }

        /// <summary>스트림의 항목 수입니다. Count of items in the stream.</summary>
        public int Count
        {
            get { return this.IsWriting ? this.writeData.Count : this.readData.Length; }
        }

        /// <summary>
        /// 스트림을 작성해, 초기화합니다. PUN에서 내부적으로 사용합니다.
        /// Creates a stream and initializes it. Used by PUN internally.
        /// </summary>
        public PhotonStream(bool write, object[] incomingData)
        {
            this.IsWriting = write;

            if (!write && incomingData != null)
            {
                this.readData = incomingData;
            }
        }

        public void SetReadStream(object[] incomingData, byte pos = 0)
        {
            this.readData = incomingData;
            this.currentItem = pos;
            this.IsWriting = false;
        }

        internal void SetWriteStream(List<object> newWriteData, byte pos = 0)
        {
            if (pos != newWriteData.Count)
            {
                throw new Exception("SetWriteStream failed, because count does not match position value. pos: "+ pos + " newWriteData.Count:" + newWriteData.Count);
            }
            this.writeData = newWriteData;
            this.currentItem = pos;
            this.IsWriting = true;
        }

        internal List<object> GetWriteStream()
        {
            return this.writeData;
        }


        [Obsolete("Either SET the writeData with an empty List or use Clear().")]
        internal void ResetWriteStream()
        {
            this.writeData.Clear();
        }

        /// <summary> IsReading가 true 일 때 스트림에서 다음 데이터 조각을 읽습니다.
        /// Read next piece of data from the stream when IsReading is true.</summary>
        public object ReceiveNext()
        {
            if (this.IsWriting)
            {
                Debug.LogError("Error: you cannot read this stream that you are writing!");
                return null;
            }

            object obj = this.readData[this.currentItem];
            this.currentItem++;
            return obj;
        }

        /// <summary> 스트림에서 다음 데이터 조각을 읽지 않고 "현재"
        /// Read next piece of data from the stream without advancing the "current" item.</summary>
        public object PeekNext()
        {
            if (this.IsWriting)
            {
                Debug.LogError("Error: you cannot read this stream that you are writing!");
                return null;
            }

            object obj = this.readData[this.currentItem];
            //this.currentItem++;
            return obj;
        }

        /// <summary>IsWriting이 참일 때 보낼 다른 데이터를 추가하십시오.
        /// Add another piece of data to send it when IsWriting is true.</summary>
        public void SendNext(object obj)
        {
            if (!this.IsWriting)
            {
                Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
                return;
            }

            this.writeData.Add(obj);
        }

        [Obsolete("writeData is a list now. Use and re-use it directly.")]
        public bool CopyToListAndClear(List<object> target)
        {
            if (!this.IsWriting) return false;

            target.AddRange(this.writeData);
            this.writeData.Clear();

            return true;
        }

        /// <summary>Turns the stream into a new object[].</summary>
        public object[] ToArray()
        {
            return this.IsWriting ? this.writeData.ToArray() : this.readData;
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref bool myBool)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(myBool);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    myBool = (bool) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref int myInt)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(myInt);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    myInt = (int) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref string value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (string) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref char value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (char) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref short value)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(value);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    value = (short) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref float obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (float) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref Player obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Player) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref Vector3 obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Vector3) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref Vector2 obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Vector2) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }

        /// <summary>
        /// 스트림의 IsWriting 값에 따라 값을 읽거나 씁니다.
        /// Will read or write the value, depending on the stream's IsWriting value.
        /// </summary>
        public void Serialize(ref Quaternion obj)
        {
            if (this.IsWriting)
            {
                this.writeData.Add(obj);
            }
            else
            {
                if (this.readData.Length > this.currentItem)
                {
                    obj = (Quaternion) this.readData[this.currentItem];
                    this.currentItem++;
                }
            }
        }
    }


    public class SceneManagerHelper
    {
        public static string ActiveSceneName
        {
            get
            {
                Scene s = SceneManager.GetActiveScene();
                return s.name;
            }
        }

        public static int ActiveSceneBuildIndex
        {
            get { return SceneManager.GetActiveScene().buildIndex; }
        }


        #if UNITY_EDITOR
        /// <summary>In Editor, we can access the active scene's name.</summary>
        public static string EditorActiveSceneName
        {
            get { return SceneManager.GetActiveScene().name; }
        }
        #endif
    }


    /// <summary>
    /// GameObj PUN을위한 PrefabPool의 기본 구현입니다.실제로 게임 객체를 인스턴스화하고 소멸 시키지만 resource.ect를 풀어 제거합니다.
    /// The default implementation of a PrefabPool for PUN, which actually Instantiates and Destroys GameObjects but pools a resource.
    /// </summary>
    /// <remarks>
    /// 
    /// 이 풀은 나중에 재사용하기 위해 실제로 GameObjects를 저장하지 않습니다.대신, 그것은 중고 GameObjects를 파괴하고 있습니다.
    /// 그러나 프리 팹은 리소스 폴더에서로드되어 캐시되며, 인스턴스화 속도가 약간 빨라집니다.
    /// This pool is not actually storing GameObjects for later reuse. Instead, it's destroying used GameObjects.
    /// However, prefabs will be loaded from a Resources folder and cached, which speeds up Instantiation a bit.
    ///
    /// ResourceCache는 공개되어 있으므로 Resources 폴더에 의존하지 않고 채울 수 있습니다.
    /// The ResourceCache is public, so it can be filled without relying on the Resources folders.
    /// </remarks>
    public class DefaultPool : IPunPrefabPool
    {
        /// <summary>
        /// 인스턴스화 속도를 높이기 위해 프리 패드마다 GameObject를 포함합니다.
        /// Contains a GameObject per prefabId, to speed up instantiation.
        /// </summary>
        public readonly Dictionary<string, GameObject> ResourceCache = new Dictionary<string, GameObject>();

        /// <summary>
        /// PUN이 사용할 네트워크 게임 객체의 비활성 인스턴스를 반환합니다.
        /// Returns an inactive instance of a networked GameObject, to be used by PUN.
        /// </summary>
        /// <param name="prefabId">String identifier for the networked object.</param>
        /// <param name="position">Location of the new object.</param>
        /// <param name="rotation">Rotation of the new object.</param>
        /// <returns></returns>
        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject res = null;
            bool cached = this.ResourceCache.TryGetValue(prefabId, out res);
            if (!cached)
            {
                res = (GameObject)Resources.Load(prefabId, typeof(GameObject));
                if (res == null)
                {
                    Debug.LogError("DefaultPool failed to load \"" + prefabId + "\" . Make sure it's in a \"Resources\" folder.");
                }
                else
                {
                    this.ResourceCache.Add(prefabId, res);
                }
            }

            bool wasActive = res.activeSelf;
            if (wasActive) res.SetActive(false);

            GameObject instance =GameObject.Instantiate(res, position, rotation) as GameObject;

            if (wasActive) res.SetActive(true);
            return instance;
        }

        /// <summary>단순 파괴 Simply destroys a GameObject.</summary>
        /// <param name="gameObject">The GameObject to get rid of.</param>
        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }


    /// <summary>
    /// PUN이 교차 유니티 버전을 쉽게 작업 할 수있게 해주는 확장 메소드의 수가 적습니다.
    /// Small number of extension methods that make it easier for PUN to work cross-Unity-versions.</summary>
    public static class PunExtensions
    {
        public static Dictionary<MethodInfo, ParameterInfo[]> ParametersOfMethods = new Dictionary<MethodInfo, ParameterInfo[]>();

        public static ParameterInfo[] GetCachedParemeters(this MethodInfo mo)
        {
            ParameterInfo[] result;
            bool cached = ParametersOfMethods.TryGetValue(mo, out result);

            if (!cached)
            {
                result = mo.GetParameters();
                ParametersOfMethods[mo] = result;
            }

            return result;
        }

        public static PhotonView[] GetPhotonViewsInChildren(this UnityEngine.GameObject go)
        {
            return go.GetComponentsInChildren<PhotonView>(true) as PhotonView[];
        }

        public static PhotonView GetPhotonView(this UnityEngine.GameObject go)
        {
            return go.GetComponent<PhotonView>() as PhotonView;
        }

        /// <summary>주어진 float 값에 대한 target-second의 제곱 크기를 비교합니다.</summary>
        public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
        {
            return (target - second).sqrMagnitude < sqrMagnitudePrecision; // TODO: inline vector methods to optimize?
        }

        /// <summary>주어진 float 값에 대한 target-second의 제곱 크기를 비교합니다.</summary>
        public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
        {
            return (target - second).sqrMagnitude < sqrMagnitudePrecision; // TODO: inline vector methods to optimize?
        }

        /// <summary>주어진 float 값에 대해 target과 second 사이의 각도를 비교합니다.</summary>
        public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
        {
            return Quaternion.Angle(target, second) < maxAngle;
        }

        /// <summary>는 두 개의 float를 비교하여 그 차이의 true를 반환합니다. </summary>
        public static bool AlmostEquals(this float target, float second, float floatDiff)
        {
            return Mathf.Abs(target - second) < floatDiff;
        }
    }
}
 
 
 
 