using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CityMapData", menuName = "Custom/City Block Data", order = 2)]
public class CityBlockData : ScriptableObject
{
    [SerializeField]
    public List<Block> Blocks = new List<Block>();
}
