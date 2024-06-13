using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildingRendering : MonoBehaviour
{
    //------------------------- Move all of these into a scriptableObject and pass that instead -----------------------
    [SerializeField] private Transform[] _wallPrefabs;
    //[SerializeField] private Transform[] _smallWindowPrefabs;
    //[SerializeField] private Transform[] _bigWindowPrefabs;
    //[SerializeField] private Transform[] _windowDetailsAbove;
    //[SerializeField] private Transform[] _doorPrefabs;
    //[SerializeField] private Transform[] _roofPrefabs;
    //[SerializeField] private Transform[] _roofDetails;
    //[SerializeField] private Material[] _wallMaterials;
    [SerializeField] private Transform _parapet;
    [SerializeField] private BuildingTypeDatas _buildingTypeDatas;
    //Transform buildingObject;
    //add these to a scriptable object
    //int doorChance = 30;
    //int smallWindowChance = 75;
    //int bigWindowChance = 70;
    public static BuildingRendering Singleton {  get; private set; }

    //private string BUILDING_TYPES_CONFIG_FILE_PATH = "Assets/Scripts/BuildingData/BuildingTypeDatas.asset";

    private Material _buildingMaterial = null;
    //private BuildingTypeDatas _buildingTypeDatas;
    private BuildingTypeData _currentBuildingData = null;

    public void Awake()
    {
        //_buildingTypeDatas = AssetDatabase.LoadAssetAtPath<BuildingTypeDatas>(BUILDING_TYPES_CONFIG_FILE_PATH);
        InitSingleton();
    }

    private void InitSingleton()
    {
        if (Singleton == null) Singleton = this;
        else if (Singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void ReRenderBuilding(Building pBuilding, Transform pExistingBuildingObj)
    {
        RemovePreviousModules(pExistingBuildingObj);

        _currentBuildingData = _buildingTypeDatas.GetBuildingData(pBuilding.TypeBuilding);

        AssignRandomMaterial();
        UpdateCollider(pBuilding, pExistingBuildingObj, true);
        RenderWing(pBuilding.BuildingWing, pExistingBuildingObj);
    }

    private static void UpdateCollider(Building pBuilding, Transform pBuildingObject, bool pRerender = false)
    {
        BoxCollider collider = pRerender ? pBuildingObject.GetComponent<BoxCollider>() : pBuildingObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(pBuilding.BuildingWing.Bounds.width, pBuilding.BuildingWing.Stories.Length, pBuilding.BuildingWing.Bounds.height);
        Vector2 buildingBounds = pBuilding.BuildingWing.Bounds.center;
        collider.center = new Vector3(buildingBounds.x, pBuilding.BuildingWing.Stories.Length / 2f, buildingBounds.y);
    }

    private static void RemovePreviousModules(Transform pExistingBuildingObj)
    {
        for (int i = 0; i < pExistingBuildingObj.childCount; i++)
        {
            Destroy(pExistingBuildingObj.GetChild(i).gameObject);
        }
    }

    public void RenderBuilding(Building pBuilding, Transform pBuldingBlock)
    {
        _currentBuildingData = _buildingTypeDatas.GetBuildingData(pBuilding.TypeBuilding);
        AssignRandomMaterial();

        Transform buildingObject = new GameObject("Building").transform;
        UpdateCollider(pBuilding, buildingObject);

        UpdateBuilding buildingUpdator = buildingObject.AddComponent<UpdateBuilding>();
        buildingUpdator.CurrentBuilding = pBuilding;
        buildingUpdator.BuildingObj = buildingObject;

        if (pBuldingBlock != null) buildingObject.SetParent(pBuldingBlock);

        RenderWing(pBuilding.BuildingWing, buildingObject);
    }

    private void AssignRandomMaterial()
    {
        if (_currentBuildingData.BuildingMaterial.Length > 0)
        {
            int randomIndex = Random.Range(0, _currentBuildingData.BuildingMaterial.Length);
            _buildingMaterial = _currentBuildingData.BuildingMaterial[randomIndex];
        }
    }

    private void RenderWing(Wing pWing, Transform pBuildingObj)
    {
        Transform wingObject = new GameObject("Wing").transform;
        wingObject.SetParent(pBuildingObj);
        foreach (Story story in pWing.Stories)
        {
            RenderStory(story, pWing, wingObject);
        }

        RenderRoof(pWing, wingObject);
        ResetTransform(wingObject);
    }

    private static void ResetTransform(Transform pTransform)
    {
        pTransform.localPosition = Vector3.zero;
        pTransform.localRotation = Quaternion.identity;
        pTransform.localScale = Vector3.one;
    }

    private void RenderRoof(Wing pWing, Transform pWingObj)
    {
        for (float x = pWing.Bounds.min.x; x < pWing.Bounds.max.x; x++)
        {
            for (float z = pWing.Bounds.min.y; z < pWing.Bounds.max.y; z++)
            {
                PlaceRoof(x, z, pWing.Stories.Length, pWingObj);
            }
        }
    }

    private void PlaceRoof(float pX, float pZ, int pLevel, Transform pWingObj)
    {
        Transform roof = Instantiate(
            _currentBuildingData.RoofPrefab,
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

        int ranNum = Random.Range(0, 10);

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
                Transform prefab = DecidePrefab(wallType, pStory.Level);
                bool isLastStory = pStory.Level == pWing.Stories.Length - 1;
                Vector3 position = new Vector3(x + .5f, pStory.Level, z + .5f);

                //south wall
                if (z == pWing.Bounds.min.y)
                {
                    Quaternion rotation = Quaternion.Euler(0, 180f, 0);
                    Transform instantiatedWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instantiatedWall);
                }

                //east wall
                if (x == pWing.Bounds.min.x + pWing.Bounds.size.x - 1f)
                {
                    Quaternion rotation = Quaternion.Euler(0, 90f, 0);
                    Transform instantiatedWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instantiatedWall);
                }

                //north wall
                if (z == pWing.Bounds.min.y + pWing.Bounds.size.y - 1f)
                {
                    Quaternion rotation = Quaternion.identity;
                    Transform instantiatedWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instantiatedWall);
                }

                //west wall
                if (x == pWing.Bounds.min.x)
                {
                    Quaternion rotation = Quaternion.Euler(0, -90f, 0);
                    Transform instantiatedWall = PlaceWall(pStory, prefab, storyObj, position, rotation, x, z, wallType);

                    if (isLastStory) PlaceParapet(x, z, pStory.Level + 1, rotation, instantiatedWall);
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

    private Transform DecidePrefab(Wall wallType, int pStoryLevel)
    {
        int randomIndex;
        switch (wallType)
        {
            case Wall.Door:
                if (_currentBuildingData.GroundDoorPrefabs.Length == 0 && _currentBuildingData.BalconyDoorPrefabs.Length == 0) break;
                return pStoryLevel == 0 ? _currentBuildingData.GroundDoorPrefabs[Random.Range(0, _currentBuildingData.GroundDoorPrefabs.Length)] : _currentBuildingData.BalconyDoorPrefabs[Random.Range(0, _currentBuildingData.BalconyDoorPrefabs.Length)];
            case Wall.GroundWindow:
                if (_currentBuildingData.GroundWindowsPrefabs.Length == 0) break;
                randomIndex = Random.Range(0, _currentBuildingData.GroundWindowsPrefabs.Length);
                return _currentBuildingData.GroundWindowsPrefabs[randomIndex];
            case Wall.HigherWindow:
                if (_currentBuildingData.HigherWindowsPrefabs.Length == 0) break;
                randomIndex = Random.Range(0, _currentBuildingData.HigherWindowsPrefabs.Length);
                return _currentBuildingData.HigherWindowsPrefabs[randomIndex];
        }

        return _wallPrefabs[Random.Range(0, _wallPrefabs.Length)];
    }

    private void PlaceWallDetail(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pWall, Wall wallType)
    {
        switch (wallType)
        {
            case Wall.Door:
                PlaceDoorDetail(pX, pZ, pLevel, pRotation, pWall);
                break;
            case Wall.GroundWindow:
            case Wall.HigherWindow:
                PlaceWindowDetail(pX, pZ, pLevel, pRotation, pWall);
                break;
        }
    }

    private void PlaceParapet(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pRoof)
    {
        if (!_currentBuildingData.HasParapet) return;

        Transform instWall = Instantiate(_parapet, new Vector3(pX + .5f, pLevel, pZ + .5f), pRotation);

        instWall.SetParent(pRoof);
    }

    private void placeRoofDetails(float pX, float pZ, int pLevel, Transform pRoof)
    {
        if (_currentBuildingData.RoofDetailPrefabs.Length == 0) return;

        Transform detail = _currentBuildingData.RoofDetailPrefabs[Random.Range(0, _currentBuildingData.RoofDetailPrefabs.Length)];

        int ranNum = Random.Range(0, 4);

        PlaceDetail(pX, pZ, pLevel, Quaternion.Euler(0, 90 * ranNum, 0), detail, pRoof);
    }

    private void PlaceDoorDetail(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pWall)
    {
        if (_currentBuildingData.HigherDoorDetailPrefabs.Length < 1 || pLevel == 0) return;

        Transform detail = _currentBuildingData.HigherDoorDetailPrefabs[Random.Range(0, _currentBuildingData.HigherDoorDetailPrefabs.Length)];
        PlaceDetail(pX, pZ, pLevel, pRotation, detail, pWall);
    }

    private void PlaceWindowDetail(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pWall)
    {
        if (_currentBuildingData.WindowDetailPrefabs.Length < 1 || !RandomChance(_currentBuildingData.WindowDetailsChance)) return;

        Transform detail = _currentBuildingData.WindowDetailPrefabs[Random.Range(0, _currentBuildingData.WindowDetailPrefabs.Length)];
        PlaceDetail(pX, pZ, pLevel, pRotation, detail, pWall);
    }

    private bool RandomChance(int pChancePercent) =>Random.Range(0, 101) <= pChancePercent;

    private void PlaceDetail(float pX, float pZ, int pLevel, Quaternion pRotation, Transform pDetail, Transform pParent)
    {
        Transform instWall = Instantiate(pDetail, new Vector3(pX + .5f, pLevel, pZ + .5f), pRotation);
        instWall.SetParent(pParent);
    }

    private Wall DecideWallType(Story pStory)
    {
        Wall wallType;

        if ((pStory.Level == 0 && RandomChance(_currentBuildingData.DoorChance)) || (_currentBuildingData.HigherDoorDetailPrefabs.Length > 0 && RandomChance(_currentBuildingData.DoorChance / 3))) wallType = Wall.Door;
        else if (RandomChance(_currentBuildingData.GroundWindowChance) && pStory.Level == 0) wallType = Wall.GroundWindow;
        else if (RandomChance(_currentBuildingData.UpperWindowChance) && pStory.Level > 0) wallType = Wall.HigherWindow;
        else wallType = Wall.Empty;

        return wallType;
    }

}
