using System.Collections;
using System.Collections.Generic;
using MVC.Views;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VictoryPanelView : BaseUIView
{
    [SerializeField] private TextMeshProUGUI playerScoreText,aiScoreText;
    [SerializeField] private Button chooseLevelButton, nextLevelButton;

    public void Setup(LevelData levelData)
    {
        if (!levelData.hasAI)
        {
            aiScoreText.enabled = false;
        }
        
        EventManager.Instance.levelFailEvent.AddListener(ShowScores);
        EventManager.Instance.levelWinEvent.AddListener(ShowScores);
    }

    private void ShowScores()
    {
        playerScoreText.text = "Your score: " + PlayerController.Instance.getScore;
        aiScoreText.text = "AI score: " + AIController.Instance.getScore;
    }
    
    public void ChooseLevelButtonClicked()
    {
        EventManager.Instance.chooseLevelButtonEvent?.Invoke();
    }
    
    public void NextLevelButtonClicked()
    {
        EventManager.Instance.nextLevelButtonEvent?.Invoke();
    }
}