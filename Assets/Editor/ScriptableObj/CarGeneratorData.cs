using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarGeneratorData", menuName = "ScriptableObjects/CarGeneratorData")]

public class CarGeneratorData : ScriptableObject
{
    public bool SpawnRandomCars;
    public GameObject[] CarPrefabs;
    [Tooltip("Values in percent, 0 being 0% and 10 being 100%")]
    [Range(0, 10)]
    public int SpawnRate;

}
