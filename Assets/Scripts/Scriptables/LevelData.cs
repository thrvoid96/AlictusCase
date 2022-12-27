using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "CustomSO/LevelData", order = 0)]
public class LevelData : JsonSerialisableScriptableObject<LevelData>
{
    [Header("Player variables")]
    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;
    public float playerScale = 1f;
    
    [Header("AI level")]
    public bool hasAI;
    public float aiSpeed = 5;
    public float aiTurnSpeed = 360f;
    public float aiAcceleration = 8f;
    
    [Header("Timer level")] 
    public bool isTimerLevel;
    public float timeToBeatlevel;
    
    [Header("Random Collect level")] 
    public bool isRandomCollectLevel;
    public float spawnRate;
    public int maxAmount;
}