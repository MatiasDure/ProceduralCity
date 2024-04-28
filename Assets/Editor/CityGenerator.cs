using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CityGenerator : Editor
{
    private static string PREFAB_LIST_ASSET_PATH = "Assets/Editor/ScriptableObj/PrefabList.asset";
    private static string BUILDING_LISTS_PATH = "Assets/GeneratedCities";
    public static PrefabList _prefabList = AssetDatabase.LoadAssetAtPath<PrefabList>(PREFAB_LIST_ASSET_PATH);

    public static void GenerateCity(List<Block> cityBlocks)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        BuildingList cityBuildingList = ScriptableObject.CreateInstance<BuildingList>();

        List<BuildingBlock> blockList = new List<BuildingBlock>();

        foreach(Block block in cityBlocks)
        {
            blockList.Add(new BuildingBlock(new Vector3Int(block.Position.x, 0, block.Position.z), block.Rotation));
            List<BuildingLot> temp = new List<BuildingLot>();
            temp.AddRange(GenerateBlock(block));
            for(int i = 0; i < temp.Count; i++)
            {
                blockList[blockList.Count - 1].Buildings.Add(BuildingGenerator.Generate(temp[i].Size, temp[i].Position));
            }
        }

        foreach(BuildingBlock block in blockList)
        {
            cityBuildingList.BuildingBlocks.Add(block);
        }


        int cities = 0;
        //----------------Need to find the highest value in the file name or else its possible for file to get overwritten------------------------

        if (!Directory.Exists(BUILDING_LISTS_PATH)) Directory.CreateDirectory(BUILDING_LISTS_PATH);
        else cities = Directory.GetFiles(BUILDING_LISTS_PATH).Length / 2;

        string cityBuildingListAssetPath = Path.Combine(BUILDING_LISTS_PATH, string.Format("CityBuildingList{0}.asset", cities));
        Debug.Log(cityBuildingListAssetPath);
        AssetDatabase.CreateAsset(cityBuildingList, cityBuildingListAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static List<BuildingLot> GenerateBlock(Block pCityBlock)
    {
        return BinarySpacePartition(new BuildingLot(pCityBlock.Size, pCityBlock.Position), pCityBlock.BuildingCount);
    }

    private static List<BuildingLot> BinarySpacePartition(BuildingLot pStartingSpace, int pBuildingCount)
    {
        List<BuildingLot> buildingLots = new List<BuildingLot>
        {
            pStartingSpace
        };

        while (pBuildingCount > buildingLots.Count)
        {
            BuildingLot currentBuildingSize = buildingLots[0];
            buildingLots.RemoveAt(0);

            List<BuildingLot> divisionResult = currentBuildingSize.IsDividedHorizontal ? DivideVertically(currentBuildingSize) : DivideHorizontally(currentBuildingSize);
            buildingLots.AddRange(divisionResult);
        }

        return buildingLots;
    }

    private static List<BuildingLot> DivideHorizontally(BuildingLot pBuilding)
    {
        float currentZ = pBuilding.Position.z;
        float currentX = pBuilding.Position.x;
        int currentHeight = pBuilding.Size.y;
        int currentWidth = pBuilding.Size.x;
        int newHeight = currentHeight / 2;
        float zPosOffset = currentHeight / 4.0f;

        return new List<BuildingLot>() { 
            new BuildingLot(new Vector2Int(currentWidth, newHeight), new Vector3(currentX, 1, currentZ + zPosOffset), true), 
            new BuildingLot(new Vector2Int(currentWidth, newHeight), new Vector3(currentX, 1, currentZ - zPosOffset), true), 
        };
    }

    private static List<BuildingLot> DivideVertically(BuildingLot pBuilding)
    {
        float currentZ = pBuilding.Position.z;
        float currentX = pBuilding.Position.x;
        int currentHeight = pBuilding.Size.y;
        int currentWidth = pBuilding.Size.x;
        int newWidth = currentWidth / 2;
        float xPosOffset = currentWidth / 4.0f;

        return new List<BuildingLot>() {
            new BuildingLot(new Vector2Int(newWidth, currentHeight), new Vector3(currentX + xPosOffset, 1, currentZ)),
            new BuildingLot(new Vector2Int(newWidth, currentHeight), new Vector3(currentX - xPosOffset, 1, currentZ)),
        };
    }

    public void OnEnable()
    {
        Debug.Log(_prefabList);

        if( _prefabList == null ) {
            Debug.LogError("Couldnt find prefab list");
        }

    }
}
