using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private int handsIndex;

    private void Start()
    {
        handsIndex = transform.GetSiblingIndex();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator = CardUIManager.Instance.cardsInHands[handsIndex]?.GetComponent<Animator>();
        animator?.SetTrigger("MouseEnter");
        animator?.SetBool("isEnter", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator?.SetBool("isEnter", false);
    }
}