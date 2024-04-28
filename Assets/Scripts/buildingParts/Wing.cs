using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wing
{
    [SerializeField]
    Rect _bounds;
    [SerializeField]
    Story[] _stories;
    [SerializeField]
    Roof _roof;
    public Rect Bounds => _bounds;
    public Story[] Stories => _stories;
    public Roof GetRoof => _roof;

    public Wing(Rect pBounds, Story[] pStories, Roof pRoof)
    {
        _bounds = pBounds;
        _stories = pStories;
        _roof = pRoof;
    }

    public override string ToString()
    {
        string wing = string.Format("Wing({0}):\n",_bounds);
        foreach (Story story in _stories) 
        {
            wing += string.Format("\t{0}\n", story.ToString());    
        }
        wing += string.Format("\t{0}\n", _roof.ToString());

        return wing;
    }

}
