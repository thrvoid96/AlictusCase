using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectArea : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectedText;
    [SerializeField] private Transform particleTransform;
    [SerializeField] private float scaleIncrease;
    [SerializeField] private List<Collectable> collectedObjects;
    

    public int getCollectedCount => collectedObjects.Count;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            if (!collectedObjects.Contains(collectable))
            {
                collectable.SwitchLayers(LevelManager.Instance.goldenLayer);
                collectable.SetColor(LevelManager.Instance.collectableColors[0]);
                collectable.ConnectSpringJointTo(GetComponent<Rigidbody>());
                collectable.isCollected = true;
                collectable.transform.SetParent(transform);
                collectedObjects.Add(collectable);
                particleTransform.localScale += (Vector3.one * scaleIncrease);
                CollectableSpawner.Instance.RemoveFromList(collectable);
                UpdateScoreText();
            }
        }
    }

    private void UpdateScoreText()
    {
        collectedText.text = collectedObjects.Count.ToString();
    }
}
