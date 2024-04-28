using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class BuildingGenerator
{
    private static string CONFIG_FIlE_PATH = "Assets/Config/MetaData.asset";
    private static MetaData _configFile = AssetDatabase.LoadAssetAtPath<MetaData>(CONFIG_FIlE_PATH);

    public static Building Generate (Vector2Int pSize, Vector3 pPosition)
    {
        float width = pSize.x;
        float height = pSize.y;
        int randomStories = Random.Range(_configFile.minimumStories, _configFile.maximumStories + 1);
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
        return new Building(pSize.x, pSize.y, wing);
    }
}
