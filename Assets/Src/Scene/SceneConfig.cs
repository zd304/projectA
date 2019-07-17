using System;
using UnityEngine;
using System.Collections.Generic;

public enum SceneObjVariableType
{
    Int,
    Float,
    String,
    IntArray,
    FloatArray,
    StringArray
}

[Serializable]
public class SceneObjVariable
{
    public string key;
    public SceneObjVariableType type;
    public string value;
}

[Serializable]
public struct SpriteObject
{
    public string path;
    public Vector2 min;
    public Vector2 max;
    public SceneObjVariable[] variables;
}

public class SceneConfig : ScriptableObject
{
    public List<SpriteObject> ornaments = new List<SpriteObject>();
    public List<SpriteObject> obstacles = new List<SpriteObject>();
    public List<SpriteObject> repeats = new List<SpriteObject>();

    public void Sort()
    {
        ornaments.Sort(CompareX);
        obstacles.Sort(CompareX);
        repeats.Sort(CompareX);
    }

    int CompareX(SpriteObject a, SpriteObject b)
    {
        return a.min.x.CompareTo(b.min.x);
    }
}