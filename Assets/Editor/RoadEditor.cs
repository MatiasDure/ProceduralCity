using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoadEditor : EditorWindow
{
    readonly Vector2Int PREFAB_SIZE = new Vector2Int(4,4);
    readonly string FLOOR_PREFAB_PATH = "Assets/Prefabs/building-self/street/Floor_large.prefab";
    readonly string STREET_PREFAB_PATH = "Assets/Scripts/StreetsScriptableObject/StreetPrefabs.asset";
    private bool _creatingRoad = false;
    Vector2Int gridSize =  new Vector2Int(1,1);
    Vector3Int gridPos = new Vector3Int(0,0,0);

    Vector2 selectedCell = new Vector2Int(-1000, -1000);
    List<RoadCell> selectedCells = new List<RoadCell>();
    RoadCell[,] roadCells;

    [MenuItem("Tools/Road Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<RoadEditor>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        UpdateRoadGrid();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Road Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Toggle Road Creator"))
        {
            _creatingRoad = !_creatingRoad;
        }

        if (GUILayout.Button("Place roads"))
        {
            GameObject streetsParent = new GameObject("Streets");
            GameObject floorsParent = new GameObject("Floors");
            CarGeneratorData carData = AssetDatabase.LoadAssetAtPath<CarGeneratorData>("Assets/Editor/ScriptableObj/CarGeneratorData.asset");

            for (int i = 0; i < roadCells.GetLength(0); i++)
            {
                for (int j = 0; j < roadCells.GetLength(1); j++)
                {
                    RoadCell road = roadCells[i, j];

                    if (road.HasRoad)
                    {
                        int index = road.CheckPrefab();

                        StreetPrefabs roadsData = AssetDatabase.LoadAssetAtPath<StreetPrefabs>(STREET_PREFAB_PATH);

                        if (roadsData.Streets[index] == null) return;

                        GameObject streetInstantiatedPrefab = PrefabUtility.InstantiatePrefab(roadsData.Streets[index].StreetModularMeshPrefab) as GameObject;

                        if (!streetInstantiatedPrefab) return;

                        streetInstantiatedPrefab.transform.position = road.Position + gridPos;
                        streetInstantiatedPrefab.transform.rotation = Quaternion.Euler(0, roadsData.Streets[index].YRotation, 0);
                        streetInstantiatedPrefab.transform.parent = streetsParent.transform;

                        if (carData.SpawnRandomCars && carData.CarPrefabs.Length > 0) SpawnRandomCar(carData, streetInstantiatedPrefab);

                        continue;
                    }

                    GameObject floorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(FLOOR_PREFAB_PATH);

                    if (floorPrefab == null) return;

                    GameObject floorInstantiatedPrefab = PrefabUtility.InstantiatePrefab(floorPrefab) as GameObject;

                    if (!floorInstantiatedPrefab) return;

                    floorInstantiatedPrefab.transform.position = road.Position + gridPos;
                    floorInstantiatedPrefab.transform.parent = floorsParent.transform;

                }
            }

        }

        GridSettingsGUI();
    }

    private void SpawnRandomCar(CarGeneratorData pCarData, GameObject pStreetParent)
    {
        //chance of spawning car on road
        int ranNum = UnityEngine.Random.Range(0, 11);

        if (ranNum <= pCarData.SpawnRate)
        {
            int ranIndex = UnityEngine.Random.Range(0, pCarData.CarPrefabs.Length);

            GameObject carInstantiatedPrefab = PrefabUtility.InstantiatePrefab(pCarData.CarPrefabs[ranIndex]) as GameObject;
            
            carInstantiatedPrefab.transform.parent = pStreetParent.transform;
            carInstantiatedPrefab.transform.localPosition = Vector3.zero;
            int ranYRotation = UnityEngine.Random.Range(0, 360);
            carInstantiatedPrefab.transform.rotation = Quaternion.Euler(0, ranYRotation, 0);
        }
    }

    private void UpdateRoadGrid()
    {
        roadCells = new RoadCell[gridSize.x, gridSize.y];

        InitializeRoadCells();
        GroupRoadCellNeighbors();
    }

    private void GridSettingsGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField("Block", EditorStyles.boldLabel);

        Vector2Int previousGridSize = gridSize;
        Vector3Int previousGridPos = gridPos;

        gridSize = EditorGUILayout.Vector2IntField("Grid size: ", gridSize);
        gridPos = EditorGUILayout.Vector3IntField("Grid position: ", gridPos);

        if(gridSize != previousGridSize || gridPos != previousGridPos) UpdateRoadGrid();

        EditorGUILayout.EndVertical();
    }

    private void InitializeRoadCells()
    {
        // initialize cells
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 cellPos = new Vector3((x*PREFAB_SIZE.x) + PREFAB_SIZE.x / 2 - gridSize.x, 0, (y* PREFAB_SIZE.y) + PREFAB_SIZE.y / 2 - gridSize.y);
                roadCells[x, y] = new RoadCell(cellPos);
            }
        }
    }

    private void GroupRoadCellNeighbors()
    {
        for(int x = 0; x < roadCells.GetLength(0); x++)
        {
            for(int y = 0; y < roadCells.GetLength(1); y++)
            {
                RoadCell cell = roadCells[x, y];

                //left neighbor
                if (x > 0) cell.LeftNeighbor = roadCells[x -1, y];
                //right
                if (x < roadCells.GetLength(0) - 1) cell.RightNeighbor = roadCells[x + 1, y];
                //top 
                if (y < roadCells.GetLength(1) - 1) cell.TopNeighbor = roadCells[x, y + 1];
                //bottom
                if (y > 0) cell.BottomNeighbor = roadCells[x, y - 1];
            }
        }

        //Debug.Log(roadCells[1, 1]);
    }

    private void DrawGrid()
    {
        //reset color
        Handles.color = Color.white;

        for(int i = 0; i < roadCells.GetLength(0); i++)
        {
            for(int j = 0; j < roadCells.GetLength(1); j++)
            {
                RoadCell roadCell = roadCells[i, j];

                bool selected = CheckCellClicked(roadCell.Position + gridPos);

                if (selected)
                {
                    roadCell.HasRoad = !roadCell.HasRoad;
                    
                    if (roadCell.HasRoad) selectedCells.Add(roadCell);
                    else selectedCells.Remove(roadCell);

                    selectedCell = new Vector2(-1000, 1000);
                }

                Handles.color = roadCell.HasRoad ? new Color(0, 1, 0, .6f) : new Color(1, 0, 0, .3f);

                Handles.DrawWireCube(roadCell.Position + gridPos, new Vector3(PREFAB_SIZE.x, 0, PREFAB_SIZE.y));
            }
        }
        //Handles.color = Color.black;
        //Handles.DrawWireCube(gridPos, new Vector3(gridSize.x, 0, gridSize.y));
    }

    private bool CheckCellClicked(Vector3 pCellPos)
    {
        float halfCellWidth = PREFAB_SIZE.x / 2;
        float halfCellHeight = PREFAB_SIZE.y / 2;

        return selectedCell.x <= pCellPos.x + halfCellWidth &&
            selectedCell.x >= pCellPos.x - halfCellWidth &&
            selectedCell.y <= pCellPos.z + halfCellHeight &&
            selectedCell.y >= pCellPos.z - halfCellHeight;
    }

    private void OnSceneGUI(SceneView sceneView)
    {

        if (!_creatingRoad) return;

        // Draw Grid for road placement
        DrawGrid();

        Event e = Event.current;
        if(e.type == EventType.MouseDown && e.button == 0)
        {
            // mouse position to world position
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Plane plane = new Plane(Vector3.up, gridPos);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                selectedCell = new Vector2(hitPoint.x, hitPoint.z);
            }
        }
    }

}
