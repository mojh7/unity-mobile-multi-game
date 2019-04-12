using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class CoinData : ScriptableObject
{
    public int id;
    public string name = "";
    public int coin;
    public Sprite sprite;
}
