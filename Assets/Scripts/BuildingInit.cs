using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class BuildingInit : MonoBehaviour
{
    [SerializeField] BuildingList _buildingList;

    private BuildingList _currentList;
    private BuildingRendering _buildingRendering;
    //private List<Transform> _cities = new List<Transform>();
    private Transform _currentCity;
    private Dictionary<BuildingList, Transform> _cities = new Dictionary<BuildingList, Transform>();
    // Start is called before the first frame update
    void Start()
    {
        if (_buildingList == null)
        {
            Debug.LogWarning("No city passed to the CityInit class");
            return;
        }

        _buildingRendering = GetComponent<BuildingRendering>();
        RenderCity(_buildingList);
    }

    private void Update()
    {
        if (_buildingList == null || _currentList == _buildingList) return;
        
        RenderCity(_buildingList);
    }

    private void RenderCity(BuildingList pListToRender)
    {
        _currentList = pListToRender;

        if(_currentCity != null) EnableCity(false, _currentCity);

        Transform existingCity = FindCity(pListToRender);

        if (existingCity != null)
        {
            EnableCity(true, existingCity);
            _currentCity = existingCity;
            return;
        }

        RenderNewCity(pListToRender);
        //Transform newCity = new GameObject("city").transform;
        //_cities.Add(pListToRender, newCity);

        //_currentCity = newCity;

        ////List<BuildingBlock> buildingBlocks = new List<BuildingBlock>();
        ////List<Transform> buildingBlockss = new List<Transform>();

        //foreach (BuildingBlock block in pListToRender.BuildingBlocks) {
        //    Transform buildingBlock = new GameObject("building_block").transform;
        //    buildingBlock.SetParent(newCity);
        //    foreach (Building building in block.Buildings)
        //    {
        //        _buildingRendering.RenderBuilding(building, buildingBlock);
        //    }
        //}

        //foreach (Building building in pListToRender.Buildings)
        //{
        //    if (building.BlockNumber > buildingBlockss.Count) buildingBlockss.Add(new GameObject("building_block").transform);

        //    Transform buildingBlock = buildingBlockss[building.BlockNumber - 1];

        //    //Debug.Log(building.Size);
        //    _buildingRendering.RenderBuilding(building, buildingBlock);
        //}
    }

    private void RenderNewCity(BuildingList pListToRender)
    {
        Transform newCity = new GameObject("city").transform;
        _cities.Add(pListToRender, newCity);

        _currentCity = newCity;

        //List<BuildingBlock> buildingBlocks = new List<BuildingBlock>();
        //List<Transform> buildingBlockss = new List<Transform>();

        foreach (BuildingBlock block in pListToRender.BuildingBlocks)
        {
            Transform buildingBlock = new GameObject("building_block").transform;
            buildingBlock.SetPositionAndRotation(block.Position, Quaternion.identity);
            //buildingBlock.SetPositionAndRotation(block.Position, Quaternion.Euler(block.Rotation));
            buildingBlock.SetParent(newCity);
            foreach (Building building in block.Buildings)
            {
                _buildingRendering.RenderBuilding(building, buildingBlock);
            }
            buildingBlock.Rotate(block.Rotation);
        }
    }

    private void EnableCity(bool pEnable, Transform pCity) => pCity.gameObject.SetActive(pEnable);

    private Transform FindCity(BuildingList pBuildingList) => _cities.ContainsKey(pBuildingList) ? _cities[pBuildingList] : null;
}
