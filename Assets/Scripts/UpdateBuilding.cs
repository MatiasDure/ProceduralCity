using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class UpdateBuilding : MonoBehaviour, IClickable
{
    private Building _building;
    private Transform _buildingObj;
    private BuildingType _nextBuildingType;
    private BuildingType _currentBuildingType;
    public Building CurrentBuilding { get => _building; set { 
            _building = value;
            _nextBuildingType = value.TypeBuilding;
            _currentBuildingType = value.TypeBuilding;
        } }
    public BuildingType NextBuildingType { get => _nextBuildingType; set => _nextBuildingType = value; }
    public Transform BuildingObj { get => _buildingObj; set => _buildingObj = value; }
    public void HandleClick()
    {
        if (_nextBuildingType == _currentBuildingType) return;

        _building = BuildingGenerator.ReGenerate(_nextBuildingType, _building.BuildingWing);
        BuildingRendering.Singleton.ReRenderBuilding(_building, BuildingObj);

        _currentBuildingType = _nextBuildingType;
    }
}
