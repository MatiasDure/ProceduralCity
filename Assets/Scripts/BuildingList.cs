using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingList", menuName = "ScriptableObjects/BuildingList")]
public class BuildingList : ScriptableObject
{
    [SerializeField]
   public List<Building> Buildings = new List<Building>();
    [SerializeField]
    public List<BuildingBlock> BuildingBlocks = new List<BuildingBlock>();
}
