using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    private Vector2Int _size;
    private BuildingBlock[] _buildingBlocks;

    public Vector2Int Size => _size;
    public BuildingBlock[] BuildingBlocks => _buildingBlocks;

    public City (int pX, int pY, BuildingBlock[] pBuildingBlocks)
    {
        _buildingBlocks = pBuildingBlocks;
        _size = new Vector2Int(pX, pY);
    }


}
