using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBZ.Owner;

public class PlayerManager : MonoBehaviourSingleton<PlayerManager>
{
    #region variable
    private MultiPlayer player;

    #endregion

    #region getter
    public MultiPlayer GetPlayer()
    {
        return player;
    }
    public void SetPlayer(MultiPlayer player)
    {
        this.player = player;
    }
    #endregion

    #region function
    
    #endregion
}
