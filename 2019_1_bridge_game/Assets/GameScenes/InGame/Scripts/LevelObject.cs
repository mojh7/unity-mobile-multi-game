using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// TODO : 구조 수정, 보완

public abstract class LevelObject : MonoBehaviour
{
}

public abstract class LevelObjectPun : Photon.Pun.MonoBehaviourPun
{
}

public abstract class PickupItem : LevelObjectPun, IPunObservable
{
    [SerializeField] private float secondsBeforeRespawn = 1f;

    /// <summary>The most likely trigger to pick up an item. Set in inspector!</summary>
    /// <remarks>Edit the collider and set collision masks to avoid pickups by random objects.</remarks>
    public bool PickupOnTrigger;

    /// <summary>If the pickup item is currently yours. Interesting in OnPickedUp(PickupItem item).</summary>
    public bool PickupIsMine;

    /// <summary>GameObject to send an event "OnPickedUp(PickupItem item)" to.</summary>
    /// <remarks>
    /// Implement OnPickedUp(PickupItem item) {} in some script on the linked game object.
    /// The item will be "this" and item.PickupIsMine will help you to find if this pickup was done by "this player".
    /// </remarks>
    //public MonoBehaviour OnPickedUpCall;


    // these values are internally used. they are public for debugging only

    /// <summary>If this client sent a pickup. To avoid sending multiple pickup requests before reply is there.</summary>
    public bool SentPickup;

    /// <summary>Timestamp when to respawn the item (compared to PhotonNetwork.time). </summary>
    public double TimeOfRespawn;    // needed when we want to update new players when a PickupItem respawns

    /// <summary></summary>
    public int ViewID { get { return this.photonView.ViewID; } }

    public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();

    public void OnTriggerEnter2D(Collider2D other)
    {
        // we only call Pickup() if "our" character collides with this PickupItem.
        // note: if you "position" remote characters by setting their translation, triggers won't be hit.
        PhotonView otherpv = other.GetComponent<PhotonView>();
        if (this.PickupOnTrigger && otherpv != null && otherpv.IsMine)
        {
            //Debug.Log("OnTriggerEnter() calls Pickup().");
            this.Pickup();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // read the description in SecondsBeforeRespawn

        if (stream.IsWriting && secondsBeforeRespawn <= 0)
        {
            stream.SendNext(this.gameObject.transform.position);
        }
        else
        {
            // this will directly apply the last received position for this PickupItem. No smoothing. Usually not needed though.
            Vector3 lastIncomingPos = (Vector3)stream.ReceiveNext();
            this.gameObject.transform.position = lastIncomingPos;
        }
    }


    public void Pickup()
    {
        if (this.SentPickup)
        {
            // skip sending more pickups until the original pickup-RPC got back to this client
            return;
        }

        this.SentPickup = true;
        this.photonView.RPC("PunPickup", RpcTarget.AllViaServer);
    }


    /// <summary>Makes use of RPC PunRespawn to drop an item (sent through server for all).</summary>
    public void Drop()
    {
        if (this.PickupIsMine)
        {
            this.photonView.RPC("PunRespawn", RpcTarget.AllViaServer);
        }
    }

    /// <summary>Makes use of RPC PunRespawn to drop an item (sent through server for all).</summary>
    public void Drop(Vector3 newPosition)
    {
        if (this.PickupIsMine)
        {
            this.photonView.RPC("PunRespawn", RpcTarget.AllViaServer, newPosition);
        }
    }


    [PunRPC]
    public void PunPickup(PhotonMessageInfo msgInfo)
    {
        // when this client's RPC gets executed, this client no longer waits for a sent pickup and can try again
        if (msgInfo.Sender.IsLocal) this.SentPickup = false;

        // In this solution, picked up items are disabled. They can't be picked up again this way, etc.
        // You could check "active" first, if you're not interested in failed pickup-attempts.
        if (!this.gameObject.activeInHierarchy)
        {
            // optional logging:
            Debug.Log("Ignored PU RPC, cause item is inactive. " + this.gameObject + " SecondsBeforeRespawn: " + secondsBeforeRespawn + " TimeOfRespawn: " + this.TimeOfRespawn + " respawn in future: " + (TimeOfRespawn > PhotonNetwork.Time));
            return;     // makes this RPC being ignored
        }


        // if the RPC isn't ignored by now, this is a successful pickup. this might be "my" pickup and we should do a callback
        this.PickupIsMine = msgInfo.Sender.IsLocal;

        /*
        // call the method OnPickedUp(PickupItem item) if a GameObject was defined as callback target
        if (this.OnPickedUpCall != null)
        {
            // you could also skip callbacks for items that are not picked up by this client by using: if (this.PickupIsMine)
            this.OnPickedUpCall.SendMessage("OnPickedUp", this);
        }
        */

        OnPickedUp();

        // setup a respawn (or none, if the item has to be dropped)
        if (secondsBeforeRespawn <= 0)
        {
            this.PickedUp(0.0f);    // item doesn't auto-respawn. must be dropped
        }
        else
        {
            // how long it is until this item respanws, depends on the pickup time and the respawn time
            double timeSinceRpcCall = (PhotonNetwork.Time - msgInfo.SentServerTime);
            double timeUntilRespawn = secondsBeforeRespawn - timeSinceRpcCall;

            //Debug.Log("msg timestamp: " + msgInfo.timestamp + " time until respawn: " + timeUntilRespawn);

            if (timeUntilRespawn > 0)
            {
                this.PickedUp((float)timeUntilRespawn);
            }
        }
    }

    protected abstract void OnPickedUp();

    internal void PickedUp(float timeUntilRespawn)
    {
        // this script simply disables the GO for a while until it respawns.
        this.gameObject.SetActive(false);
        PickupItem.DisabledPickupItems.Add(this);
        this.TimeOfRespawn = 0;

        if (timeUntilRespawn > 0)
        {
            this.TimeOfRespawn = PhotonNetwork.Time + timeUntilRespawn;
            Invoke("PunRespawn", timeUntilRespawn);
        }
    }


    [PunRPC]
    internal void PunRespawn(Vector3 pos)
    {
        Debug.Log("PunRespawn with Position.");
        this.PunRespawn();
        this.gameObject.transform.position = pos;
    }

    [PunRPC]
    internal void PunRespawn()
    {
#if DEBUG
        // debugging: in some cases, the respawn is "late". it's unclear why! just be aware of this.
        double timeDiffToRespawnTime = PhotonNetwork.Time - this.TimeOfRespawn;
        if (timeDiffToRespawnTime > 0.1f) Debug.LogWarning("Spawn time is wrong by: " + timeDiffToRespawnTime + " (this is not an error. you just need to be aware of this.)");
#endif

        // if this is called from another thread, we might want to do this in OnEnable() instead of here (depends on Invoke's implementation)
        PickupItem.DisabledPickupItems.Remove(this);
        this.TimeOfRespawn = 0;
        this.PickupIsMine = false;

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(true);
        }
    }


}

public abstract class CollisionLevelObject : LevelObject
{
}
