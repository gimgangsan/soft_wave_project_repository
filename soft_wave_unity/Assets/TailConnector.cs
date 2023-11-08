using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TailConnector : MonoBehaviour, IBeginDragHandler,IEndDragHandler,IDragHandler
{
    public RawImage Image {  get; private set; }
    public RectTransform RectTransform { get; set; }
    public LineRenderer LineRenderer { get; set; }
    CanvasGroup CanvasGroup;

    public Vector3 InitialPos;
    public bool IsConnected = false;

    private void Awake()
    {
        Image = GetComponent<RawImage>();
        RectTransform = GetComponent<RectTransform>();
        InitialPos = RectTransform.localPosition;
        CanvasGroup = GetComponent<CanvasGroup>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.SetPosition(0, RectTransform.position);
        LineRenderer.SetPosition(1, RectTransform.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Image.color = Color.red;
        CanvasGroup.alpha = 0.6f;
        CanvasGroup.blocksRaycasts = false;
        IsConnected = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform.localPosition += (Vector3)eventData.delta;
        LineRenderer.SetPosition(1, RectTransform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup.alpha = 1.0f;
        CanvasGroup.blocksRaycasts = true;
        if (IsConnected == false)
        {
            Image.color = Color.white;
            RectTransform.localPosition = InitialPos;
            LineRenderer.SetPosition(1, RectTransform.position);
        }
    }
}
