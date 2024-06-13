using UnityEditor;
using UnityEngine;

public static class BuildingGenerator
{
    //private static string CONFIG_FIlE_PATH = "Assets/Config/MetaData.asset";
    private static string BUILDING_TYPES_CONFIG_FILE_PATH = "Assets/Scripts/BuildingData/BuildingTypeDatas.asset";
    //private static MetaData _configFile = AssetDatabase.LoadAssetAtPath<MetaData>(CONFIG_FIlE_PATH);
    private static BuildingTypeDatas _buildingTypeDatas = AssetDatabase.LoadAssetAtPath<BuildingTypeDatas>(BUILDING_TYPES_CONFIG_FILE_PATH);

    public static Building Generate (Vector2Int pSize, Vector3 pPosition, BuildingType pBuildingType)
    {
        BuildingTypeData buildingData = _buildingTypeDatas.GetBuildingData (pBuildingType);
        float width = pSize.x;
        float height = pSize.y;
        //int randomStories = Random.Range(_configFile.minimumStories, _configFile.maximumStories + 1);
        int randomStories = Random.Range(buildingData.StoriesRange[0], buildingData.StoriesRange[1] + 1);
        Wall[] walls = new Wall[(pSize.x + pSize.y) * 2];
        Story[] stories = new Story[randomStories];
        for(int i = 0; i < randomStories; i++)
        {
            stories[i] = new Story(i, walls);
        }

        Wing wing = new Wing(
            new Rect(pPosition.x - width / 2, pPosition.z - height / 2, width, height),
            stories,
            new Roof());

        Debug.Log(wing);
        Debug.Log(stories[0].ToString());
        return new Building(pSize.x, pSize.y, wing, pBuildingType);
    }

    public static Building ReGenerate(BuildingType pBuildingType, Wing pCurrentWing)
    {
        BuildingTypeData buildingData = _buildingTypeDatas.GetBuildingData(pBuildingType);
        //int randomStories = Random.Range(_configFile.minimumStories, _configFile.maximumStories + 1);
        int randomStories = Random.Range(buildingData.StoriesRange[0], buildingData.StoriesRange[1] + 1);
        Wall[] walls = new Wall[((int) pCurrentWing.Bounds.width + (int) pCurrentWing.Bounds.height) * 2];
        Story[] stories = new Story[randomStories];
        // creating each story and their walls
        for (int i = 0; i < randomStories; i++)
        {
            stories[i] = new Story(i, walls);
        }

        Wing wing = new Wing(
            pCurrentWing.Bounds,
            stories,
            new Roof());

        return new Building((int)pCurrentWing.Bounds.width, (int)pCurrentWing.Bounds.height, wing, pBuildingType);
    }
}
