using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using MVC.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelView : BaseUIView
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI remainingAmountText;
    [SerializeField] private Image fillImage;

    private float startAmount;
    public void Setup(LevelData levelData)
    {
        levelText.text = "Level " + LevelManager.Instance.getData.Level;
        fillImage.fillAmount = 0f;
        
        if (!levelData.isTimerLevel)
        {
            startAmount = CollectableSpawner.Instance.getAvailableCollectablesList.Count;
            remainingAmountText.text = 0 + " / " + startAmount;
            
            EventManager.Instance.playerCollectedEvent.AddListener(CalculateCollected);
            EventManager.Instance.aiCollectedEvent.AddListener(CalculateCollected);
        }
        else
        {
            startAmount = levelData.timeToBeatlevel;
            remainingAmountText.text = startAmount.ToString(CultureInfo.InvariantCulture);
            
            EventManager.Instance.levelStartEvent.AddListener(StartTimer);
        }
    }

    private void StartTimer()
    {
        float remainingTime = startAmount;
        DOTween.To(() => remainingTime, x => remainingTime = x, 0f, startAmount).SetEase(Ease.Linear).OnUpdate(() =>
        {
            fillImage.fillAmount = remainingTime / startAmount;
            remainingAmountText.text = remainingTime.ToString("0.0");
        }).OnComplete(() =>
        {
            EventManager.Instance.levelCompleteEvent.Invoke();
        });
    }

    private void CalculateCollected()
    {
        var final = 0;
        for (int i = 0; i < LevelManager.Instance.getActorsInScene.Count; i++)
        {
            final += LevelManager.Instance.getActorsInScene[i].getScore;
        }
        
        remainingAmountText.text = final + " / " + startAmount;
        fillImage.fillAmount = final / startAmount;
    }
}