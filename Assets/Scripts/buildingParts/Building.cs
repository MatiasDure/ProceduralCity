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

    public Vector2Int Size { get => _size; set => _size = value; }
    public Wing BuildingWing { get => _wing; set => _wing = value; }

    public Building(int pSizeX, int pSizeY, Wing pWing)
    {
        _size = new Vector2Int(pSizeX, pSizeY);
        _wing = pWing;
    }

    public override string ToString()
    {
        string building = string.Format("Building: (size: {0})\n", _size.ToString());
        
        building += string.Format("\t{0}\n", _wing.ToString());
        
        return building;
    }
}
