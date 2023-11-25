using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardClickDetector : MonoBehaviour, IPointerClickHandler
{
    public GameObject card;
    public CardBase cardBase;

    // 덱 매니저에서 카드를 더블 클릭하면 해당 카드의 덱 포함 여부를 토글한다
    public void OnPointerClick(PointerEventData eventData)
    {
        cardBase = card.GetComponent<CardBase>();

        if (eventData.clickCount == 2)  // 더블 클릭이라면
        {
            if (cardBase.inDeck)        // 덱 포함 여부 토글
            {
                if (CardManager.Instance.deck.Count < 6) return;
                CardManager.Instance.removeFromDeck(card);
                GetComponent<Outline>().enabled = false;
            }
            else
            {
                CardManager.Instance.addToDeck(card);
                GetComponent<Outline>().enabled = true;
            }
        }
    }
}
