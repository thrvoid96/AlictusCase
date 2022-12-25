using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button button;

    public void SetupButton(int levelToSet)
    {
        button.onClick.AddListener(delegate { LevelManager.Instance.ChangeLevel(levelToSet); });
        levelText.text = "Level " + levelToSet;
    }
}
