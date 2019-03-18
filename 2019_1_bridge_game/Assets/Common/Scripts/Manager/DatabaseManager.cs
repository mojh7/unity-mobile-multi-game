using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviourSingleton<DatabaseManager>
{
    //인게임 데이터들
    public CharacterDatabase characterData;

    public void Initialize()
    {
        if (characterData == null)
        {
            characterData = Resources.Load<CharacterDatabase>("Database/CharacterDatabase");
        }

        characterData.Instantiate();

        Debug.Log("Initialize Game Database Manager ");
    }

}
