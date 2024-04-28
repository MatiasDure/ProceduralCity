using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingBlock
{
    [SerializeField]
    private List<Building> _buildings = new List<Building>();

    [SerializeField]
    private Vector3Int _position;

    [SerializeField]
    private Vector3 _rotation;

    public List<Building> Buildings => _buildings;
    public Vector3Int Position => _position;
    public Vector3 Rotation => _rotation;

    public BuildingBlock(List<Building> pBuildings)
    {
        _buildings = pBuildings;
    }
    public BuildingBlock(Vector3Int pPosition, Vector3 pRotation) 
    {
        _position = pPosition;
        _rotation = pRotation;
    }
}
