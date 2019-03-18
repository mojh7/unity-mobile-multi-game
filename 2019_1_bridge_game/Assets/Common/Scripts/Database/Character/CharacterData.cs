using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class CharacterData : ScriptableObject
{
    public int id;
    public string name = "";
    public int gold;
    public Sprite sprite;
}
