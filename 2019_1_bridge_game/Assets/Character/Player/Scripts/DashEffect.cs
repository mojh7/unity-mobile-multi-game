using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner;
using Photon.Pun.UtilityScripts;

public class DashEffect : MonoBehaviour
{
    [SerializeField] private MultiPlayer player;
    [SerializeField] private GameObject parentObj;

    private const string PLAYER = "Player";
    private PunTeams.Team team;

    public GameObject GetDashEffectObj() { return parentObj; }

    public void Init(PunTeams.Team team)
    {
        this.team = team;
        if (PunTeams.Team.RED == team)
        {
            gameObject.layer = LayerMask.NameToLayer(InGameManager.RED_TEAM_PLAYER);
        }
        else if (PunTeams.Team.BLUE == team)
        {
            gameObject.layer = LayerMask.NameToLayer(InGameManager.BLUE_TEAM_PLAYER);
        }
        Debug.Log("dasheffect team : " + team);
        parentObj.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Collision(ref collision);
    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log(2);
    //    Collision(collision.collider);
    //}

    /// <summary> Trigger </summary>
    public void Collision(ref Collider2D coll)
    {
        // TODO : 논리식 생각 점 더하기
        if ((PunTeams.Team.RED == team && UtilityClass.CheckLayer(coll.gameObject.layer, InGameManager.BLUE_TEAM_PLAYER) && coll.CompareTag(MultiPlayer.PLAYER)) ||
            PunTeams.Team.BLUE == team && UtilityClass.CheckLayer(coll.gameObject.layer, InGameManager.RED_TEAM_PLAYER) && coll.CompareTag(MultiPlayer.PLAYER))
        {
            Debug.Log("대쉬 충돌");
            coll.GetComponent<MultiPlayer>().HitDash(player.GetPosition(), player.GetDirVector());
            player.StopBehavior(UBZ.Owner.CharacterInfo.BehaviorState.DASH);
        }
    }
}
