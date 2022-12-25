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

    public UnityEvent chooseLevelButtonEvent, nextLeveLButtonEvent;
    
    public void Setup(int playerScore, int aiScore)
    {
        playerScoreText.text = playerScore.ToString();
        aiScoreText.text = aiScore.ToString();
    }
    
    public void Setup(int playerScore)
    {
        playerScoreText.text = playerScore.ToString();
        aiScoreText.gameObject.SetActive(false);
    }

    public void ChooseLevelButtonClicked()
    {
        chooseLevelButtonEvent?.Invoke();
    }
    
    public void NextLevelButtonClicked()
    {
        nextLeveLButtonEvent?.Invoke();
    }
}