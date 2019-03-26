// ----------------------------------------------------------------------------
// <copyright file="Enums.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Wraps up several enumerations for PUN.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    /*
    /// <summary>Which PhotonNetwork method was called to connect (which influences the regions we want pinged).</summary>
    /// <remarks>PhotonNetwork.ConnectUsingSettings will call either ConnectToMaster, ConnectToRegion or ConnectToBest, depending on the settings.</remarks>
    */
    /// <summary> 연결하기 위해 호출 된 PhotonNetwork 메소드 (ping 할 영역에 영향을 미침). </summary>
    /// <remarks> PhotonNetwork.ConnectUsingSettings는 설정에 따라 ConnectToMaster, ConnectToRegion 또는 ConnectToBest 중 하나를 호출합니다. </remarks>
    public enum ConnectMethod { NotCalled, ConnectToMaster, ConnectToRegion, ConnectToBest }


    /// <summary>Used to define the level of logging output created by the PUN classes. Either log errors, info (some more) or full.</summary>
    /// \ingroup publicApi
    public enum PunLogLevel
    {
        /// <summary>Show only errors. Minimal output. Note: Some might be "runtime errors" which you have to expect.</summary>
        ErrorsOnly,

        /// <summary>Logs some of the workflow, calls and results.</summary>
        Informational,

        /// <summary>Every available log call gets into the console/log. Only use for debugging.</summary>
        Full
    }


    // <summary>Enum of "target" options for RPCs. These define which remote clients get your RPC call. </summary>
    /// <summary> RPC의 "대상"옵션 열거. RPC 호출을받는 원격 클라이언트를 정의합니다. </summary>
    /// \ingroup publicApi
    public enum RpcTarget
    {
        // <summary> Sends the RPC to everyone else and executes it immediately on this client. Player who join later will not execute this RPC.</summary>
        /// <summary> RPC를 다른 모든 사용자에게 보내고 이 클라이언트에서 즉시 RPC를 실행합니다. 나중에 참여하는 플레이어는 이 RPC를 실행하지 않습니다. </summary>
        All,

        // <summary> Sends the RPC to everyone else. This client does not execute the RPC. Player who join later will not execute this RPC.</summary>
        /// <summary> RPC를 다른 모든 사용자에게 보냅니다. 이 클라이언트는 RPC를 실행하지 않습니다. 나중에 참여하는 플레이어는 이 RPC를 실행하지 않습니다. </summary>
        Others,

        // <summary> Sends the RPC to MasterClient only. Careful: The MasterClient might disconnect before it executes the RPC and that might cause dropped RPCs.</summary>
        /// <summary> RPC를 MasterClient에만 보냅니다. 주의 : MasterClient가 RPC를 실행하기 전에 연결이 끊어지면 RPC가 삭제 될 수 있습니다. </summary>
        MasterClient,

        // <summary> Sends the RPC to everyone else and executes it immediately on this client. New players get the RPC when they join as it's buffered (until this client leaves).</summary>
        /// <summary> RPC를 다른 모든 사용자에게 보내고 이 클라이언트에서 즉시 RPC를 실행합니다. 새로운 플레이어는 버퍼링 될 때 (이 클라이언트가 나가기 전까지) 가입 할 때 RPC를받습니다. </summary>
        AllBuffered,

        // <summary> Sends the RPC to everyone. This client does not execute the RPC. New players get the RPC when they join as it's buffered (until this client leaves).</summary>
        /// <summary> RPC를 모든 사용자에게 보냅니다. 이 클라이언트는 RPC를 실행하지 않습니다. 새로운 플레이어는 버퍼링 될 때 (이 클라이언트가 나가기 전까지) 가입 할 때 RPC를받습니다. </summary>
        OthersBuffered,

        /*
        /// <summary>Sends the RPC to everyone (including this client) through the server.</summary>
        /// <remarks>
        /// This client executes the RPC like any other when it received it from the server.
        /// Benefit: The server's order of sending the RPCs is the same on all clients.
        /// </remarks>
        */ 
        /// <summary> 서버를 통해 모든 사람 (이 클라이언트 포함)에게 RPC를 보냅니다. </summary>
        /// <remarks>
        /// 이 클라이언트는 서버에서 서버를 받으면 다른 것과 마찬가지로 RPC를 실행합니다.
        /// 이점 : RPC를 보내는 서버의 순서는 모든 클라이언트에서 동일합니다.
        /// </remarks>
        AllViaServer,

        /*
        /// <summary>Sends the RPC to everyone (including this client) through the server and buffers it for players joining later.</summary>
        /// <remarks>
        /// This client executes the RPC like any other when it received it from the server.
        /// Benefit: The server's order of sending the RPCs is the same on all clients.
        /// </remarks>
        */
        /// <summary> 서버를 통해 모든 클라이언트 (이 클라이언트 포함)에게 RPC를 보내고 나중에 합류하는 플레이어를 위해 버퍼링합니다. </summary>
        /// <remarks>
        /// 이 클라이언트는 서버에서 서버를 받으면 다른 것과 마찬가지로 RPC를 실행합니다.
        /// 이점 : RPC를 보내는 서버의 순서는 모든 클라이언트에서 동일합니다.
        /// </remarks>
        AllBufferedViaServer
    }


    public enum ViewSynchronization { Off, ReliableDeltaCompressed, Unreliable, UnreliableOnChange }


    /// <summary>
    /// Options to define how Ownership Transfer is handled per PhotonView.
    /// </summary>
    /// <remarks>
    /// This setting affects how RequestOwnership and TransferOwnership work at runtime.
    /// </remarks>
    public enum OwnershipOption
    {
        /// <summary>
        /// Ownership is fixed. Instantiated objects stick with their creator, scene objects always belong to the Master Client.
        /// </summary>
        Fixed,
        /// <summary>
        /// Ownership can be taken away from the current owner who can't object.
        /// </summary>
        Takeover,
        /// <summary>
        /// Ownership can be requested with PhotonView.RequestOwnership but the current owner has to agree to give up ownership.
        /// </summary>
        /// <remarks>The current owner has to implement IPunCallbacks.OnOwnershipRequest to react to the ownership request.</remarks>
        Request
    }
}