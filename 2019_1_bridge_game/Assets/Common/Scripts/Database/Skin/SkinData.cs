using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class SkinData : ScriptableObject
{   
    public int id;
    public string skin_name;
    public string characteristic;
    public Sprite sprite;
}
