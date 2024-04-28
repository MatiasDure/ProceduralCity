using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CityMapManager : EditorWindow
{
    private string BLOCK_PATH = "Assets/Editor/Blocks";
    private CityBlockData _cityBlockData;
    private Vector2 _scrollPosition = new Vector2(0, 40);
    private bool _rotationHandlesEnabled = true;
    private bool _translationHandlesEnabled = true;
    private bool _blocksVisible = true;

    [MenuItem("Tools/Map Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<CityMapManager>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        _cityBlockData = AssetDatabase.LoadAssetAtPath<CityBlockData>("Assets/Editor/ScriptableObj/CityBlockData.asset");

        if (_cityBlockData == null)
        {
            _cityBlockData = ScriptableObject.CreateInstance<CityBlockData>();
            AssetDatabase.CreateAsset(_cityBlockData, "Assets/Editor/ScriptableObj/CityBlockData.asset");
            AssetDatabase.SaveAssets();
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Block Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Block"))
        {
            Block newBlock = ScriptableObject.CreateInstance<Block>();
            newBlock.Size = new Vector2Int(4, 4);
            newBlock.Position = new Vector3Int(1, 1, 1);
            AssetDatabase.CreateAsset(newBlock, string.Format("{0}/{1}.asset",BLOCK_PATH, Directory.GetFiles(BLOCK_PATH).Length));
            AssetDatabase.SaveAssets();
            _cityBlockData.Blocks.Add(newBlock);
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        for(int i = 0; i < _cityBlockData.Blocks.Count; i++)
        {
            DrawBlockGUI(_cityBlockData.Blocks[i]);
            i = AddDeleteBtn(i);
        }

        EditorGUILayout.EndScrollView();

    }

    private int AddDeleteBtn(int index)
    {
        if (GUILayout.Button("Delete", GUILayout.Width(60)))
        {
            _cityBlockData.Blocks.RemoveAt(index);
            return --index;
        }

        return index;
    }

    private void DrawBlockGUI(Block block)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField("Block", EditorStyles.boldLabel);

        block.Size = EditorGUILayout.Vector2IntField("Size: ", block.Size);
        block.Position = EditorGUILayout.Vector3IntField("Position: ", block.Position);
        block.Rotation = EditorGUILayout.Vector3Field("Rotation: ", block.Rotation);

        //max amount of buildings if all buildings are 1 x 1
        int maxAmountOfBuildings = block.Size.x * block.Size.y; 
        block.BuildingCount = Mathf.Clamp(EditorGUILayout.IntField("Buldings amount", block.BuildingCount), 1, maxAmountOfBuildings);
        
        EditorGUILayout.EndVertical();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        Rect generateBtn = new Rect(60, 10, 80, 40);
        Rect toggleRotationBtn = new Rect(60, 60, 110, 40);
        Rect toggleTranslationBtn = new Rect(60, 110, 110, 40);
        Rect toggleBlocks = new Rect(60, 160, 110, 40);

        if (GUI.Button(generateBtn, "Generate"))
        {
            CityGenerator.GenerateCity(_cityBlockData.Blocks);
        }

        if(GUI.Button(toggleTranslationBtn, "Toggle Move"))
        {
            _translationHandlesEnabled = !_translationHandlesEnabled;
        }

        if (GUI.Button(toggleRotationBtn, "Toggle Rotation"))
        {
            _rotationHandlesEnabled = !_rotationHandlesEnabled;
        }

        if (GUI.Button(toggleBlocks, "Toggle Blocks"))
        {
            _blocksVisible = !_blocksVisible;
        }

        Handles.EndGUI();

        Handles.color = Color.blue;
        foreach(Block block in _cityBlockData.Blocks)
        {
            if (!_blocksVisible) return;

            DrawBlockWireframe(block);

            if (_rotationHandlesEnabled) DrawRotationBlockHandle(block);
            if (_translationHandlesEnabled) DrawTranslationBlockHandle(block);
        }

    }

    private void DrawBlockWireframe(Block block)
    {
        //Used to rotate block wire frame
        Handles.matrix = Matrix4x4.TRS(block.Position, Quaternion.Euler(block.Rotation), Vector3.one);
        Handles.DrawWireCube(Vector3.zero, new Vector3(block.Size.x, 0, block.Size.y));
        Handles.matrix = Matrix4x4.identity; // Reset matrix
    }

    private void DrawTranslationBlockHandle(Block block)
    {
        Vector3 handlePositionRaw = Handles.PositionHandle(block.Position, Quaternion.identity);
        Vector3Int handlePosition = new Vector3Int(Mathf.RoundToInt(handlePositionRaw.x), Mathf.RoundToInt(handlePositionRaw.y), Mathf.RoundToInt(handlePositionRaw.z));

        if (handlePosition != block.Position)
        {
            Undo.RecordObject(block, "Move Block");
            block.Position = handlePosition;
        }
    }
    private void DrawRotationBlockHandle(Block block)
    {
        Quaternion handleRotationRaw = Handles.RotationHandle(Quaternion.Euler(block.Rotation), block.Position);
        if (handleRotationRaw.eulerAngles != block.Rotation)
        {
            Undo.RecordObject(block, "Rotate Block");
            block.Rotation = new Vector3(0, handleRotationRaw.eulerAngles.y, 0);
        }
    }

}
