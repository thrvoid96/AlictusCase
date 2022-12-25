using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour,IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError("test");
    }
}
