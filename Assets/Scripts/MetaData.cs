using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MetaData", menuName = "ScriptableObjects/MetaData")]
public class MetaData : ScriptableObject
{
    public int minimumStories = 1;
    public int maximumStories = 1;
}
