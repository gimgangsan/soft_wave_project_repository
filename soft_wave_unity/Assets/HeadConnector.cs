using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeadConnector : MonoBehaviour, IDropHandler
{
    public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        TailConnector DroppedObject = eventData.pointerDrag.GetComponent<TailConnector>();
        DroppedObject.RectTransform.position = RectTransform.position;
        DroppedObject.LineRenderer.SetPosition(1, RectTransform.position);
        DroppedObject.IsConnected = true;
        DroppedObject.Image.color = Color.green;
        
    }
}
