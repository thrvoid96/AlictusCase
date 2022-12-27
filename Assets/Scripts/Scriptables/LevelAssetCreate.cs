using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "LevelAsset", menuName = "CustomSO/LevelAssetCreate", order = 0)]
public class LevelAssetCreate : ScriptableObject
{
    public List<GameObject> levelPrefabs;
    public List<Color> collectableColors;
}
