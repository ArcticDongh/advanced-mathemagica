using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public SingleLevelInfo[] data;
}
[System.Serializable]
public class SingleLevelInfo
{
    public string name;
    public bool isAvailable;
}

public static class FixedLevelData
{

}