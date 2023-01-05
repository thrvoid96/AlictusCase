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

    private void Start()
    {
        EventManager.Instance.levelCompleteEvent.AddListener(DisableArea);
    }

    public int getCollectedCount => collectedObjects.Count;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out var collectable))
        {
            if (!collectable.isCollected)
            {
                collectable.isCollected = true;
                collectable.isBeingHeld = false;
                
                collectable.SwitchLayers(LevelManager.Instance.goldenLayer);
                collectable.SetColor(LevelManager.Instance.collectableColors[0]);
                collectable.ConnectSpringJointTo(GetComponent<Rigidbody>());
                collectable.transform.SetParent(transform);
                
                particleTransform.localScale += (Vector3.one * scaleIncrease);
                
                collectedObjects.Add(collectable);
                CollectableSpawner.Instance.RemoveFromList(collectable);

                transform.parent.GetChild(0).GetComponent<Actor>().GetCollectEvent().Invoke();
                UpdateScoreText();
                
                CompleteOnAllCollected();
            }
        }
    }

    private void UpdateScoreText()
    {
        collectedText.text = collectedObjects.Count.ToString();
    }

    private void DisableArea()
    {
        transform.GetChild(0).GetComponent<Collider>().enabled = false;
    }
    
    private void CompleteOnAllCollected()
    {
        if (CollectableSpawner.Instance.getAvailableCollectablesList.Count == 0) 
        {
            EventManager.Instance.levelCompleteEvent.Invoke();
        }
    }
}
