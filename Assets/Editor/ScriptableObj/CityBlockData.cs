using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CityMapData", menuName = "Custom/City Block Data", order = 2)]
public class CityBlockData : ScriptableObject
{
    public List<Block> Blocks = new List<Block>();
}
