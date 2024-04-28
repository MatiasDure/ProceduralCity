using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Story
{
    [SerializeField]
    private int _level;
    [SerializeField]
    private Wall[] _walls;
    public int Level => _level;
    public Wall[] Walls => _walls;

    public Story(int pLevel, Wall[] pWalls)
    {
        _level = pLevel;
        _walls = pWalls;
    }

    public override string ToString()
    {
        string story = string.Format("Story {0}\n\t\tWalls: ", _level);

        foreach (Wall wall in _walls) 
        {
            story += wall.ToString() + ",";
        }

        return story;
    }

}
