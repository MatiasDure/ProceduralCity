using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Roof
{
    [SerializeField]
    RoofType _type;

    public RoofType Type  => _type; 

    public Roof(RoofType pType = RoofType.Flat)
    {
        _type = pType;
    }

    public override string ToString()
    {
        return string.Format("Roof: {0}", _type.ToString());
    }
}

[Serializable]
public enum RoofType
{
    Point,
    Flat
}
