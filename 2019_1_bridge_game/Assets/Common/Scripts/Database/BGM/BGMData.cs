using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BGMData : ScriptableObject
{
    public int id;
    public string name = "";
    public AudioClip bgm;
}
