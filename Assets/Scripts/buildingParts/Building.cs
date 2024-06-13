using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building
{
    private Vector2Int _size;
    [SerializeField]
    private Wing _wing;
    [SerializeField]
    private BuildingType _typeBuilding;

    public Vector2Int Size { get => _size; set => _size = value; }
    public Wing BuildingWing { get => _wing; set => _wing = value; }
    public BuildingType TypeBuilding { get => _typeBuilding; set => _typeBuilding = value; }

    public Building(int pSizeX, int pSizeY, Wing pWing, BuildingType pBuildingType)
    {
        _size = new Vector2Int(pSizeX, pSizeY);
        _wing = pWing;
        _typeBuilding = pBuildingType;
    }

    public override string ToString()
    {
        string building = string.Format("Building: (size: {0})\n", _size.ToString());
        
        building += string.Format("\t{0}\n", _wing.ToString());
        
        return building;
    }
}
