using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Block : ScriptableObject
{
    public BuildingType TypeBuiding;
    public Vector2Int Size;
    public Vector3Int Position;
    public Vector3 Rotation;
    public int BuildingCount;
}

[Serializable]
public class BuildingLot
{
    public Vector2Int Size;
    public Vector3 Position;
    public bool IsDividedHorizontal;

    public BuildingLot(Vector2Int size, Vector3 position, bool isDividedHorizontal = false)
    {
        Size = size;
        Position = position;
        IsDividedHorizontal = isDividedHorizontal;
    }

    public override string ToString()
    {
        return string.Format("\nBuilding Lot -\nSize: {0}\nPosition: {1}", Size, Position);
    }
}