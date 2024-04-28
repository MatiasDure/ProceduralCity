using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingRendering : MonoBehaviour
{
    //------------------------- Move all of these into a scriptableObject and pass that instead -----------------------
    [SerializeField] private Transform[] _wallPrefabs;
    [SerializeField] private Transform[] _smallWindowPrefabs;
    [SerializeField] private Transform[] _bigWindowPrefabs;
    [SerializeField] private Transform[] _windowDetailsAbove;
    [SerializeField] private Transform[] _doorPrefabs;
    [SerializeField] private Transform[] _roofPrefabs;
    [SerializeField] private Transform[] _roofDetails;
    [SerializeField] private Material[] _wallMaterials;
    [SerializeField] private Transform _parapet;
    Transform buildingObject;
    //add these to a scriptable object
    int doorChance = 30;
    int smallWindowChance = 75;
    int bigWindowChance = 70;

    private Material _buildingMaterial = null;

    public void RenderBuilding(Building pBuilding, Transform pBuldingBlock)
    {
        //Debug.Log(pBuilding.BuildingWing);
        if(_wallMaterials.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _wallMaterials.Length);
            _buildingMaterial = _wallMaterials[randomIndex];
        }

        buildingObject = new GameObject("Building").transform;
        buildingObject.SetParent(pBuldingBlock);
        RenderWing(pBuilding.BuildingWing);

    }

    private void RenderWing(Wing pWing)
    {
        Transform wingObject = new GameObject("Wing").transform;
        wingObject.SetParent(buildingObject);
        foreach(Story story in pWing.Stories)
        {
            RenderStory(story, pWing, wingObject);
        }

        RenderRoof(pWing, wingObject);
    }

    private void RenderRoof(Wing pWing, Transform pWingObj)
    {
        for (float x = pWing.Bounds.min.x; x < pWing.Bounds.max.x; x++)
        {
            for (float z = pWing.Bounds.min.y; z < pWing.Bounds.max.y; z++)
            {
                PlaceRoof(x, z, pWing.Stories.Length, pWingObj, pWing.GetRoof.Type);
            }
        }
    }

    private void PlaceRoof(float pX, float pZ, int pLevel, Transform pWingObj, RoofType pType)
    {
        Transform roof = Instantiate(
            _roofPrefabs[0],
            pWingObj.TransformPoint(
                new Vector3(
                  pX + .5f,
                  pLevel,
                  pZ + .5f
                )
            ),
            Quaternion.identity
        );
        roof.SetParent(pWingObj);

        int ranNum = UnityEngine.Random.Range(0, 10);

        if(ranNum < 2) placeRoofDetails(pX, pZ, pLevel, roof);
    }

    private void RenderStory(Story pStory, Wing pWing, Transform pWingObj)
    {
        Transform storyObj = new GameObject("Story").transform;
        storyObj.SetParent(pWingObj);

        for (float x = pWing.Bounds.min.x; x < pWing.Bounds.max.x; x++)
        {
            for(float z = pWing.Bounds.min.y; z < pWing.Bounds.max.y; z++)
            {
                Wall wallType = DecideWallType(pStory);

                Transform prefab = DecidePrefab(wallType);

                bool isLastStory = pStory.Level == pWing.Stories.Length - 1;

                Vector3 position = new Vector3(x + .5f, pStory.Level, z + .5f);

                //south wall
                if (z == pWing.Bounds.min.y)
                {
                    Quaternion rotation = Quaternion.Euler(0, 180f, 0);

                    Transform instWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    //Transform instWall = Instantiate(prefab, new Vector3(x + .5f, pStory.Level, z + .5f), Quaternion.Euler(0, 180f, 0)); 
                    //instWall.SetParent(storyObj);
                    //PlaceWallDetail(x, z, pStory.Level, Quaternion.Euler(0, 180f, 0),  instWall, wallType);
                    //SetWallRandomMaterial(instWall);
                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instWall);
                }

                //east wall
                if (x == pWing.Bounds.min.x + pWing.Bounds.size.x - 1f)
                {
                    Quaternion rotation = Quaternion.Euler(0, 90f, 0);

                    Transform instWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    //Transform instWall = Instantiate(prefab, new Vector3(x + .5f, pStory.Level, z + .5f), Quaternion.Euler(0, 90f, 0)); 
                    //instWall.SetParent(storyObj);
                    //PlaceWallDetail(x, z, pStory.Level, Quaternion.Euler(0, 90f, 0), instWall, wallType);
                    //SetWallRandomMaterial(instWall);
                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instWall);
                    
                }

                //north wall
                if (z == pWing.Bounds.min.y + pWing.Bounds.size.y - 1f)
                {
                    Quaternion rotation = Quaternion.identity;
                    Transform instWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    //Transform instWall = Instantiate(prefab, new Vector3(x + .5f, pStory.Level, z + .5f), Quaternion.identity); 
                    //instWall.SetParent(storyObj);
                    //PlaceWallDetail(x, z, pStory.Level, Quaternion.identity, instWall, wallType);
                    //SetWallRandomMaterial(instWall);
                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instWall);
                }

                //west wall
                if (x == pWing.Bounds.min.x)
                {
                    Quaternion rotation = Quaternion.Euler(0, -90f, 0);
                    Transform instWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);
                    //Transform instWall = Instantiate(prefab, new Vector3(x + .5f, pStory.Level, z + .5f), Quaternion.Euler(0, -90f, 0));
                    //instWall.SetParent(storyObj);
                    //PlaceWallDetail(x, z, pStory.Level, Quaternion.Euler(0, -90f, 0), instWall, wallType);
                    //SetWallRandomMaterial(instWall);
                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instWall);
                }

            }
        }
    }

    private Transform PlaceWall(Story pStory, Transform pPrefab, Transform pStoryObj, Vector3 pPos, Quaternion pRot, float pX, float pZ, Wall pWallType)
    {
        Transform instantiatedWall = Instantiate(pPrefab, pPos, pRot);
        instantiatedWall.SetParent(pStoryObj);

        PlaceWallDetail(pX, pZ, pStory.Level, pRot, instantiatedWall, pWallType);
        SetWallRandomMaterial(instantiatedWall);

        return instantiatedWall;
    }

    private void SetWallRandomMaterial(Transform pWall)
    {
        if (_buildingMaterial == null) return;

        MeshRenderer wallRenderer = pWall.GetComponentInChildren<MeshRenderer>();

        if (wallRenderer == null) return;

        wallRenderer.material = _buildingMaterial;
    }

    private Transform DecidePrefab(Wall wallType)
    {
        switch (wallType)
        {
            case Wall.Door:
                return _doorPrefabs[UnityEngine.Random.Range(0, _doorPrefabs.Length)];
            case Wall.BigWindow:
                return _bigWindowPrefabs[UnityEngine.Random.Range(0, _bigWindowPrefabs.Length)];
            case Wall.SmallWindow:
                return _smallWindowPrefabs[UnityEngine.Random.Range(0, _smallWindowPrefabs.Length)];
            default:
                return _wallPrefabs[UnityEngine.Random.Range(0, _wallPrefabs.Length)];
        }
    }

    private void PlaceWallDetail(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pWall, Wall wallType)
    {
        //int ranNumber = UnityEngine.Random.Range(0, 4);

        switch (wallType)
        {
            case Wall.Door:
                PlaceBalcony(pX, pZ, pLevel, pRotation, pWall);
                break;
            case Wall.BigWindow:
                // Think about details for big window (i.e big sun cover);
                break;
            case Wall.SmallWindow:
                // find details for small window (i.e small sun cover);
                break;
            default:
                //detail = _windowDetailsAbove[UnityEngine.Random.Range(0, _windowDetailsAbove.Length)];
                break;
        }
    }

    private void PlaceParapet(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pRoof)
    {
        //Transform detail;

        //detail = _roofDetails[UnityEngine.Random.Range(0, _roofDetails.Length)];
        Transform instWall = Instantiate(_parapet, new Vector3(pX + .5f, pLevel, pZ + .5f), pRotation);

        instWall.SetParent(pRoof);
    }

    private void placeRoofDetails(float pX, float pZ, int pLevel, Transform pRoof)
    {
        Transform detail;

        detail = _roofDetails[UnityEngine.Random.Range(0, _roofDetails.Length)];

        int ranNum = UnityEngine.Random.Range(0, 4); 
        Transform instWall = Instantiate(detail, new Vector3(pX + .5f, pLevel, pZ + .5f), Quaternion.Euler(0,90 * ranNum, 0));
        instWall.SetParent(pRoof);
    }

    private void PlaceBalcony(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pWall)
    {
        if (pLevel == 0) return;

        Transform detail;

        detail = _windowDetailsAbove[UnityEngine.Random.Range(0, _windowDetailsAbove.Length)];
        Transform instWall = Instantiate(detail, new Vector3(pX + .5f, pLevel, pZ + .5f), pRotation);

        instWall.SetParent(pWall);
    }

    private Wall DecideWallType(Story pStory)
    {
        int decideType = UnityEngine.Random.Range(0, 101);
        Wall wallType;
        if ((decideType < doorChance && pStory.Level == 0) || decideType < (doorChance / 3)) wallType = Wall.Door;
        else if (decideType < bigWindowChance && pStory.Level == 0) wallType = Wall.BigWindow;
        else if (decideType < smallWindowChance) wallType = Wall.SmallWindow;
        else wallType = Wall.Empty;

        return wallType;
    }

}
