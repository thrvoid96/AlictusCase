using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameData", menuName = "CustomSO/GameData", order = 1)]
public class GameData : JsonSerialisableScriptableObject<GameData>
{
    public int Level, RandomLevel;
}