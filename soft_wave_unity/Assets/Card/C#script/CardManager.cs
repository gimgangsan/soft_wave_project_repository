using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;
using UnityEngine.Events;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

public class CardManager : MonoBehaviour
{
    public UnityEvent<int> whenCasting; // ī�� ���� �̺�Ʈ (ī�� ID�� �Ű������� ����)
    public UnityEvent<int> whenHit; // �������� �� �� �̺�Ʈ (�� ������ ���� �Ű������� ����)

    public List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> deck;              // �÷��̾ ������ ��
    public GameObject[] hands;           // �÷��̾ �տ� ��� �ִ� ��
    public int drawIndex = 0;           // �̹� ��ο쿡�� ���� ī�� �ε���
    public int peekIndex = 0;           
    public Slider mana;                 // ����
    public float manaRestoreSpeed;      // ���� ���¹̳�
    public int manaConsume;

    private static CardManager _instance;       // �̱��� ���� ������
    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        hands = new GameObject[5];
        deck = new List<GameObject>(inventory);

        int[] initInven = { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 }; // �׽�Ʈ �������� ������ ���� �����ϵ��� ��
        foreach (int i in initInven)
        {
            addToInventory(i);
        }
        foreach(Transform t in transform)
        {
            addToDeck(t.gameObject);
        }
        addToInventory(4);
    }

    void Update()
    {
        mana.value += manaRestoreSpeed * Time.deltaTime;      // ���¹̳� ȸ��

        // �׽�Ʈ ����
        // 1-4 Ű�� ������ �տ� �� ī�带 ��
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseCard(3);
        
        for (int handIndex = 0; handIndex < 4; handIndex++)                     
        {
            if (CardUIManager.Instance.cardsInHands[handIndex] == null) DrawCard(handIndex);       // �� �и� Ȯ���ϰ� ��ο�
        }        
    }

    // ���� ī�� �߰�
    public void addToDeck(GameObject obj)
    {
        deck.Add(obj);        // cardIndex�� ����Ű�� ī�带 ���� �ǵڿ� �߰��Ѵ�
        int index = IndexFromObject(obj);
        if (CardInfo.cardInfo[index].script != null)
        {
            Type script = CardInfo.cardInfo[index].script;
            ((ICard)(obj.GetComponent(script))).OnAcquire();     // ī�� �߰� ȿ�� ȣ��
        }
        Debug.Log(deck.ToString());
    }

    public void addToInventory(int cardIndex)
    {
        GameObject obj = InstantiateCard(cardIndex);
        inventory.Add(obj);
        inventory.Sort((a, b) => IndexFromObject(a) - IndexFromObject(b)) ;
    }

    public void removeFromInventory(GameObject obj)
    {
        inventory.Remove(obj);
        if (deck.Contains(obj)) removeFromDeck(obj);

        Destroy(obj);
    }

    // ������ cardIndex�� ����Ű�� ī�� ����
    public void removeFromDeck(GameObject obj)
    {
        int objIndexInDeck = -1;
        for(int i = 0; i < deck.Count(); i++)
        {
            if (deck[i] == obj) objIndexInDeck = i;
        }
        if (objIndexInDeck == -1) return;

        int cardIndex = IndexFromObject(obj);
        if (CardInfo.cardInfo[cardIndex].script != null)
        {
            Type script = CardInfo.cardInfo[cardIndex].script;
            ((ICard)(obj.GetComponent(script))).OnRemove();
        }
        deck.Remove(obj);

        if (objIndexInDeck < drawIndex)
        {
            drawIndex--;
        }
        else if (objIndexInDeck == drawIndex)
        {
            peekIndex = drawIndex + 1;
            CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex + 1]));
        }
        else if (objIndexInDeck == peekIndex)
        {
            CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex + 1]));
        }

        for(int i = 0; i < hands.Length; i++)
        {
            if (hands[i] == obj) DrawCard(i);
        }
    }

    // ī�� ���
    void UseCard(int handIndex)
    {
        if (CardUIManager.Instance.cardsInHands[handIndex] == null) return;     // �и� ������ �ִ��� �˻�
        if (mana.value < manaConsume) return;                                   // ���¹̳��� ������� �˻�
        Debug.Log(handIndex + "/" + hands.Length);
        int index = IndexFromObject(hands[handIndex]);
        if (CardInfo.cardInfo[index].script != null)
        {
            Type script = CardInfo.cardInfo[index].script;
            ((ICard)(hands[handIndex].GetComponent(script))).OnUse();     // ī�� ��� ȿ�� ȣ��
        }

        CardUIManager.Instance.Discard(handIndex);      // UI�� ī�� ��� �Լ��� ȣ��

        mana.value -= manaConsume;       // ���¹̳� �Ҹ�
    }

    // ī�� �̱�
    void DrawCard(int handIndex)
    {
        CardUIManager.Instance.DrawCard(handIndex, IndexFromObject(deck[drawIndex]));  // UI�� ī�� �̱� �Լ��� ȣ��
        hands[handIndex] = deck[drawIndex++];       // �տ� �� �� ���� ��, drawIndex ����
        if (drawIndex == deck.Count())
        {
            ShuffleDeck();
            drawIndex = 0;
        }
        peekIndex = drawIndex;
        CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex]));
        int index = IndexFromObject(hands[handIndex]);
        if (CardInfo.cardInfo[index].script != null)
        {
            Type script = CardInfo.cardInfo[index].script;
            ((ICard)(hands[handIndex].GetComponent(script))).OnDraw();     // ī�� ��ο� ȿ�� ȣ��
        }
    }

    // �� ����
    // �Ǽ�-������(Fisher-Yate) ���� ������� ����
    void ShuffleDeck()
    {
        for (int i = deck.Count() - 1; i > 1; i--) {
            int rand = UnityEngine.Random.Range(0, i + 1);
            GameObject temp = deck[rand];
            deck[rand] = deck[i];
            deck[i] = temp;
        }
    }

    GameObject InstantiateCard(int cardIndex)
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(transform);

        obj.name = "Card" + cardIndex;
        obj.AddComponent<CardBase>();
        obj.GetComponent<CardBase>().index = cardIndex;
        obj.AddComponent(CardInfo.cardInfo[cardIndex].script);

        return obj;
    }

    public int IndexFromObject(GameObject obj)
    {
        return obj.GetComponent<CardBase>().index;
    }

    public int IndexFromInven(int i)
    {
        return inventory[i].GetComponent<CardBase>().index;
    }

    public int IndexFromDeck(int i)
    {
        return deck[i].GetComponent<CardBase>().index;
    }
}
