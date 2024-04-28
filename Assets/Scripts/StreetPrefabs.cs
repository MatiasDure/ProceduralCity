using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StreetPrefabs", menuName = "ScriptableObjects/StreetPrefabs")]
public class StreetPrefabs: ScriptableObject
{
    [SerializeField]
    public StreetGrouping[] Streets;
}

[Serializable]
public class StreetGrouping
{
    public GameObject StreetModularMeshPrefab;
    public float YRotation;

    public override string ToString()
    {
        return StreetModularMeshPrefab + " with rotation: " + YRotation;
    }
}
