using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "CustomSO/LevelData", order = 0)]
public class LevelData : JsonSerialisableScriptableObject<LevelData>
{
    public float timeToBeatlevel;
    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;
    public float playerScale = 1f;
    public float aiSpeed = 5;
    public float aiTurnSpeed = 360f;
    public float aiAcceleration = 8f;
    public bool hasAI;
}