using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public GameObject cardPrefab;                       // ������ �Էµ��� ���� ī�� ������
    public Transform[] handsPos;                        // ���� ī�尡 �̵��� ��ġ��
    public GameObject peekCard;
    public GameObject[] cardsInHands;                   // ���� �տ� �ִ� ī�忡 ���� ���۷����� ����

    private static CardUIManager _instance; // �̱��� ���� ����
    public static CardUIManager Instance
    { get { return _instance; } }

    void Awake()
    {
        _instance = this;
    }

    // ī�� �̱� �ִϸ��̼� ����
    public void DrawCard(int handIndex, int cardIndex)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);    // ���������� ī�� ������Ʈ ����
        UpdateCard(newCard, cardIndex);                             // ī�� ������ �°� ���� ������Ʈ
        cardsInHands[handIndex] = newCard;                          // �� ��ġ�� �ִ� ī�带 ���� ī��� ����
        StartCoroutine(DrawnCardMove(newCard, handIndex));          // ī�� �̵� �ڷ�ƾ
    }

    // ī�� ��� �ִϸ��̼� ����
    public void Discard(int handIndex)
    {
        if (!cardsInHands[handIndex]) return;       // �ش� ��ġ�� �ƹ� ī�尡 ���� ���

        cardsInHands[handIndex].GetComponent<Animator>().SetTrigger("Discard"); // ī�带 ����ϴ� �ִϸ��̼� ����
        StartCoroutine(UpwardMove(cardsInHands[handIndex]));                   // �ִϸ��̼� ���� �� ������ �ڷ�ƾ ȣ��
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
        UpdateCard(peekCard, cardIndex);
    }

    // ī�� �̱�/��� �ִϸ��̼� �� ������ �ڷ�ƾ
    IEnumerator UpwardMove(GameObject card)
    {
        Vector3 currentPos = card.transform.position;    // ī��UI�� ��ġ

        for(int i = 0; i < 100; i++)
        {
            currentPos.y += Screen.height/200;
            card.transform.position = currentPos;
            yield return null;
        }

        Destroy(card);       // ������ �� ��ġ�� �ִ� ī��� �ı�
        yield break;
    }

    //ī�� �̵� �ڷ�ƾ
    IEnumerator DrawnCardMove(GameObject newCard, int handIndex)
    {
        Vector3 currentPos;
        Vector3 StartPos = currentPos = peekCard.transform.position;
        Vector3 EndPos = handsPos[handIndex].position;
        float a = Screen.height * 0.6f / Mathf.Pow((EndPos.x - StartPos.x), 2);

        newCard.transform.position = StartPos; 
        while (currentPos.x < EndPos.x)
        {
            currentPos.x += Screen.width/200;
            currentPos.y = -a * ((currentPos.x - StartPos.x) * (currentPos.x - EndPos.x)) + StartPos.y;  // ������ ������ �׸��� ������

            newCard.transform.position = currentPos;
            yield return null;      // ���� �����Ӷ� �ݺ����� �̾��ϱ�
        }
        newCard.transform.position = EndPos;   // ī����ġ ����

        yield break;
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