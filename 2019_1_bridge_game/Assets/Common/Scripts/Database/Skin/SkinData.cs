using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class SkinData : ScriptableObject
{   
    public string character_name;
    public string skin_name;
    public Sprite sprite;
}
