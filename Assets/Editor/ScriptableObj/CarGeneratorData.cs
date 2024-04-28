using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarGeneratorData", menuName = "ScriptableObjects/CarGeneratorData")]

public class CarGeneratorData : ScriptableObject
{
    public bool SpawnRandomCars;
    public GameObject[] CarPrefabs;
    [Range(0, 10)]
    public int SpawnRate;

}
