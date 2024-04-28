using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "ScriptableObjects/PrefabList", order = 1)]
public class PrefabList : ScriptableObject
{
    public GameObject[] Roofs;
    public GameObject[] Windows;
    public GameObject[] Doors;
}
