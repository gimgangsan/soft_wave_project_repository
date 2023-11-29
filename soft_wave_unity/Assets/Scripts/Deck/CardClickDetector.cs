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
    public GameObject highlight;

    // �� �Ŵ������� ī�带 ���� Ŭ���ϸ� �ش� ī���� �� ���� ���θ� ����Ѵ�
    public void OnPointerClick(PointerEventData eventData)
    {
        cardBase = card.GetComponent<CardBase>();

        if (eventData.clickCount == 2)  // ���� Ŭ���̶��
        {
            if (cardBase.inDeck)        // �� ���� ���� ���
            {
                if (CardManager.Instance.deck.Count < 9) return;
                CardManager.Instance.removeFromDeck(card);
                highlight.SetActive(false);
            }
            else
            {
                CardManager.Instance.addToDeck(card);
                highlight.SetActive(true);
            }
        }
    }
}
