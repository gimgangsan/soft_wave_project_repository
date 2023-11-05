using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public GameObject cardPrefab;                       // ������ �Էµ��� ���� ī�� ������
    public Transform[] handsPos;                        // ���� ī�尡 �̵��� ��ġ��
    public Transform peekPos;
    private GameObject peekCard;
    public GameObject[] cardsInHands;                   // ���� �տ� �ִ� ī�忡 ���� ���۷����� ����

    private static CardUIManager _instance; // �̱��� ���� ����
    public static CardUIManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        cardsInHands = new GameObject[5];
    }

    // ī�� �̱� �ִϸ��̼� ����
    public void DrawCard(int handIndex, int cardIndex)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);                // ���������� ī�� ������Ʈ ����
        UpdateCard(newCard, cardIndex);                                         // ī�� ������ �°� ���� ������Ʈ
        newCard.GetComponent<Animator>().SetInteger("HandIndex", handIndex);    // �־��� ��ȣ�� ��ġ�� �̵��ϴ� �ִϸ��̼� ����
        cardsInHands[handIndex] = newCard;                                      // �� ��ġ�� �ִ� ī�带 ���� ī��� ����
    }

    // ī�� ��� �ִϸ��̼� ����
    public void Discard(int handIndex)
    {
        if (!cardsInHands[handIndex]) return;       // �ش� ��ġ�� �ƹ� ī�尡 ���� ���

        cardsInHands[handIndex].GetComponent<Animator>().SetTrigger("Discard"); // ī�带 ����ϴ� �ִϸ��̼� ����
        StartCoroutine(UpdateHandWithDelay(handIndex, null));                   // �ִϸ��̼� ���� �� ������ �ڷ�ƾ ȣ��
    }

    // card�� ����Ű�� ī�� ������Ʈ�� cardIndex�� �ش��ϴ� ������ ����
    public void UpdateCard(GameObject card, int cardIndex)
    {
        foreach (Transform obj in card.GetComponent<Transform>())   // ī�� ������Ʈ�� �ڽ� ������Ʈ�� �߿��� �˻�
        {
            if (obj.name == "Image")                                                                                // �׸� ����
            {
                obj.GetComponent<Image>().sprite = CardInfo.cardInfo[cardIndex].image;
                obj.GetComponent<Image>().color = Color.white;
            }
            if (obj.name == "Name") obj.GetComponent<TMP_Text>().text = CardInfo.cardInfo[cardIndex].name;          // �̸� ����
            if (obj.name == "Description") obj.GetComponent<TMP_Text>().text = CardInfo.cardInfo[cardIndex].desc;   // ���� ����
        }
    }

    // �̸����� ī�带 cardIndex�� �ش��ϴ� ī��� ����
    public void UpdatePeekCard(int cardIndex)
    {
        if (peekCard == null)
        {
            peekCard = Instantiate(cardPrefab, peekPos);
        }
        UpdateCard(peekCard, cardIndex);
    }

    // ī�� �̱�/��� �ִϸ��̼� �� ������ �ڷ�ƾ
    IEnumerator UpdateHandWithDelay(int handIndex, GameObject newCard)
    {
        yield return new WaitForSeconds(.2f);   // .2�� ��...

        Destroy(cardsInHands[handIndex]);       // ������ �� ��ġ�� �ִ� ī��� �ı�
        cardsInHands[handIndex] = newCard;      // �� ��ġ�� �ִ� ī�带 ���� ī��� ����
    }

    // ��� �ִ� ��� �и� ���� (���̺�/�ε� � Ȱ��)
    public void UpdateHands(int[] cardIndexes)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform);
            UpdateCard(newCard, cardIndexes[i]);
            newCard.transform.position = handsPos[i].position;
        }
    }
}