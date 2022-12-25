using System.Collections;
using System.Collections.Generic;
using MVC.Views;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelChooseView : BaseUIView
{
    [SerializeField] private ScrollRect scrollView;
    public void Setup()
    {
        for (int i = 1; i < LevelManager.Instance.levelAsset.levelPrefabs.Count + 1; i++)
        {
            var spawnedButton = ObjectPool.Instance.SpawnFromPool("LevelChooseButton", Vector3.zero, Quaternion.identity, scrollView.content).GetComponent<LevelButton>();
            spawnedButton.SetupButton(i);
        }
    }
}