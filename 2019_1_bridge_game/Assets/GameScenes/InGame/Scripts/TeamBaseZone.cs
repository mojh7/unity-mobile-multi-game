using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class TeamBaseZone : MonoBehaviour
{
    [SerializeField] private PunTeams.Team team;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + ", " + team);
    }
}
