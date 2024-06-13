using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BuildingTypeData", menuName = "ScriptableObjects/Building/BuildingTypeData")]
public class BuildingTypeData : ScriptableObject
{
    public BuildingType TypeBuilding;
    [Tooltip("Minimum and Maximum stories for the building")]
    public Vector2Int StoriesRange = new Vector2Int();
    //public RoofType TypeRoof;
    public Transform[] GroundWindowsPrefabs;
    [Tooltip("Window prefabs for windows that are above ground floor (story 2 and up)")]
    public Transform[] HigherWindowsPrefabs;
    public Transform[] GroundDoorPrefabs;
    public Transform[] BalconyDoorPrefabs;
    public Material[] BuildingMaterial;
    [Tooltip("Details for windows that are above ground floor (i.e balcony)")]
    public Transform[] HigherDoorDetailPrefabs;
    public Transform[] WindowDetailPrefabs;
    public Transform[] RoofDetailPrefabs;
    public Transform RoofPrefab;
    public bool HasParapet;
    [Range(0, 100)]
    public int DoorChance = 30;
    [Range(0, 100)]
    public int UpperWindowChance = 75;
    [Range(0, 100)]
    public int GroundWindowChance = 70;
    [Range(0, 100)]
    public int WindowDetailsChance = 40;
}

[Serializable]
public enum BuildingType
{
    RegularLarge,
    RegularSmall,
    Office,
    Store,
    ClockTower,
    Garage,
    HighClassHotel,
    MiddleClassHotel,
    LowClassHotel,
    WhiteBuilding,
    OutsideScopeBuilding,
}
