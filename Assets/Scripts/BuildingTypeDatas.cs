using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BuildingTypeDatas", menuName = "ScriptableObjects/Building/BuildingTypeDatas")]
public class BuildingTypeDatas : ScriptableObject
{
    [SerializeField]
    public List<BuildingDataGrouping> TypeBuildingDatas = new List<BuildingDataGrouping>();

    public BuildingTypeData GetBuildingData(BuildingType pBuildingType)
    {
        foreach(BuildingDataGrouping grouping in TypeBuildingDatas)
        {
            if (grouping.TypeBuilding == pBuildingType) return grouping.TypeBuildingData;
        }

        return null;
    }
}

[Serializable]
public class BuildingDataGrouping
{
    public BuildingType TypeBuilding;
    public BuildingTypeData TypeBuildingData;
}
