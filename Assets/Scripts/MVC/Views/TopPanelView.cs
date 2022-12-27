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
    [SerializeField] private TextMeshProUGUI remainingTimeText;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Image fillImage;

    private float startTime;
    public void Setup(LevelData levelData)
    {
        levelText.text = "Level " + LevelManager.Instance.getData.Level;
        
        if (!levelData.isTimerLevel)
        {
            timerObject.SetActive(false);
            return;
        }
        startTime = levelData.timeToBeatlevel;
        remainingTimeText.text = startTime.ToString(CultureInfo.InvariantCulture);
        fillImage.fillAmount = 1f;
        
        EventManager.Instance.levelStartEvent.AddListener(StartTimer);
    }

    private void StartTimer()
    {
        float remainingTime = startTime;
        DOTween.To(() => remainingTime, x => remainingTime = x, 0f, startTime).SetEase(Ease.Linear).OnUpdate(() =>
        {
            fillImage.fillAmount = remainingTime / startTime;
            remainingTimeText.text = remainingTime.ToString("0.0");
        }).OnComplete(() =>
        {
            EventManager.Instance.levelCompleteEvent.Invoke();
        });
    }
}