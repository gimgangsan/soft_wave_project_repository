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
using System.Text;

public class CardManager : MonoBehaviour
{
    public GameObject[] cardsList;
    public UnityEvent<int> whenCasting; // ī�� ���� �̺�Ʈ (ī�� ID�� �Ű������� ����)
    public UnityEvent<int> whenHit; // �������� �� �� �̺�Ʈ (�� ������ ���� �Ű������� ����)

    public List<GameObject> inventory;
    public List<GameObject> deck;       // �÷��̾ ������ ��
    public GameObject[] hands;          // �÷��̾ �տ� ��� �ִ� ��
    public int drawIndex = 0;           // �̹� ��ο쿡�� ���� ī�� �ε���
    public Slider mana;                 // ����
    public float manaRestoreSpeed;
    public int manaConsume;

    private static CardManager _instance;       // �̱��� ���� ������
    public static CardManager Instance
    { get { return _instance; } }

    void Awake()
    {
        _instance = this;
        hands = new GameObject[5];
        inventory = new List<GameObject>();
        deck = new List<GameObject>(inventory);

        int[] initInven = { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 }; // �׽�Ʈ �������� ������ ���� �����ϵ��� ��
        bool[] isDeck = { true, true, true, true, false, true, true, true, true, false };
        LoadData(initInven, isDeck);
    }

    void Update()
    {
        mana.value += manaRestoreSpeed * Time.deltaTime;      // ���¹̳� ȸ��

        // �׽�Ʈ ����
        // 1-4 Ű�� ������ �տ� �� ī�带 ��
        if (!General.Instance.isPause)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) UseCard(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) UseCard(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) UseCard(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) UseCard(3);
        }

        // �� �и� Ȯ���ϰ� ��ο�
        for (int handIndex = 0; handIndex < CardUIManager.Instance.cardsInHands.Length; handIndex++)
            if (CardUIManager.Instance.cardsInHands[handIndex] == null) DrawCard(handIndex);       
    }

    // ���� ī�� �߰�
    public void addToDeck(GameObject obj)
    {
        deck.Add(obj);                                          // �κ��丮�� �ִ� obj ī�带 ���� �߰��Ѵ�
        int index = IndexFromObject(obj);

        obj.GetComponent<ICard>()?.OnAcquire();                  // ī�� �߰� ȿ�� ȣ��

        obj.GetComponent<CardBase>().inDeck = true;             // ī�尡 deck�� �ִٰ� ǥ��
    }

    // �κ��丮�� ī�� �߰�
    public void addToInventory(int cardIndex)
    {
        GameObject obj = InstantiateCard(cardIndex);            // ī�� ������Ʈ ����
        inventory.Add(obj);                                     // �κ��丮�� ī�� ������Ʈ �߰�
        inventory.Sort((a, b) => IndexFromObject(a) - IndexFromObject(b)) ; // ����
    }

    // �κ��丮���� ī�� ����
    public void removeFromInventory(GameObject obj)
    {
        inventory.Remove(obj);                          // �κ��丮���� ī�� ����
        if (deck.Contains(obj)) removeFromDeck(obj);    // ������ ī�尡 �ִٸ� �������� ����
        obj.GetComponent<ICard>()?.OnRemove();           // ī�� ���� ȿ�� ȣ��
        Destroy(obj);                                   // ī�� ������Ʈ �ı�
    }

    // ������ ī�� ����
    public void removeFromDeck(GameObject obj)
    {
        int objIndexInDeck = -1;                        // �� ������ ������ ī���� �ε����� ���Ѵ�
        for(int i = 0; i < deck.Count(); i++)
        {
            if (deck[i] == obj) objIndexInDeck = i;
        }
        if (objIndexInDeck == -1) return;               // �� ���� ī�尡 ������ ����

        int cardIndex = IndexFromObject(obj);
        deck.Remove(obj);                               // ������ ī�� ����
        obj.GetComponent<CardBase>().inDeck = false;

        if (drawIndex >= deck.Count)                    // ������ ���� �ε����� ������ ��� ������
        {
            ShuffleDeck();                              // ���� �� �ٽ� ��ο� ����
            drawIndex = 0;
            CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex]));
        }
        else
        {
            if (objIndexInDeck == drawIndex)            // ������ ���� �ε������� ī�带 �����ߴٸ�
            {
                CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex]));    // �̸����� ����
            }
            else if (objIndexInDeck < drawIndex)        // drawIndex �տ� �ִ� ī�带 �����ߴٸ�
            {
                drawIndex--;                            // drawIndex�� �ϳ� �ٿ� ���������� ����ǵ��� ��
            }
        }

        for(int i = 0; i < hands.Length; i++)           // ������ ī�尡 ���п� �־��ٸ�
        {
            if (hands[i] == obj)
            {
                CardUIManager.Instance.Discard(i);      // ī�带 ���п��� �� ��
                DrawCard(i);                            // ���� ī�� ��ο�
            }
        }
    } 

    // ī�� ���
    void UseCard(int handIndex)
    {
        if (CardUIManager.Instance.cardsInHands[handIndex] == null) return;     // �и� ������ �ִ��� �˻�
        if (mana.value < manaConsume) return;                                   // ���¹̳��� ������� �˻�
        Debug.Log(handIndex + "/" + hands.Length);
        int index = IndexFromObject(hands[handIndex]);
        AimInfo CurrentAimInfo = new(General.Instance.script_player.transform, General.Instance.MousePos());
        hands[handIndex].GetComponent<ICard>()?.OnUse(CurrentAimInfo);     // ī�� ��� ȿ�� ȣ��
        hands[handIndex] = null;
        CardUIManager.Instance.Discard(handIndex);      // UI�� ī�� ��� �Լ��� ȣ��

        mana.value -= manaConsume;       // ���¹̳� �Ҹ�
    }

    // ī�� �̱�
    void DrawCard(int handIndex)
    {
        CardUIManager.Instance.DrawCard(handIndex, IndexFromObject(deck[drawIndex]));  // UI�� ī�� �̱� �Լ��� ȣ��
        hands[handIndex] = deck[drawIndex];       // �տ� �� �� ���� ��, drawIndex ����
        while (drawIndex < deck.Count() && hands.Contains(deck[drawIndex])) drawIndex++;

        if (drawIndex >= deck.Count())
        {
            do {
                ShuffleDeck();
            }
            while (hands.Contains(deck[0]));
            drawIndex = 0;
        }

        CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex]));
        hands[handIndex].GetComponent<ICard>()?.OnDraw(); // ī�� ��ο� ȿ�� ȣ��
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

    // ī�� ������Ʈ ����
    GameObject InstantiateCard(int cardIndex)
    {
        GameObject obj = Instantiate(cardsList[cardIndex]);
        obj.transform.SetParent(transform);
        obj.GetComponent<CardBase>().index = cardIndex;
        obj.transform.position = new Vector3(-100.5f, -18.25f, 0);

        return obj;
    }

    // ����� ������ �ҷ��� �κ��丮/���� �����Ѵ�
    // inventory �Ű��������� �� ī���� ���� ��ȣ�� �迭�� �����Ѵ�.
    // isDeck �Ű��������� �� ī���� �� ���� ���θ� �����Ѵ�. (�� �迭�� �ϴ��� ����)
    void LoadData(int[] inventory, bool[] isDeck)
    {
        if (inventory.Length != isDeck.Length) return;          // ũ�Ⱑ �ٸ� ��� ����

        for(int i = 0; i < inventory.Length; i++)
        {
            addToInventory(inventory[i]);                       // �κ��丮�� ī�� �߰�
            if (isDeck[i]) { addToDeck(this.inventory[i]); }    // �߰��� ī�尡 ���� ���ԵǾ�� �Ѵٸ� ������ �߰�
        }
    }

    // ������Ʈ�κ��� ī�� ���� ��ȣ�� �ҷ��´�
    public int IndexFromObject(GameObject obj)
    {
        return obj.GetComponent<CardBase>().index;
    }

    // �κ��丮 �ε����κ��� ī�� ���� ��ȣ�� �ҷ��´�
    public int IndexFromInven(int i)
    {
        return inventory[i].GetComponent<CardBase>().index;
    }

    // �� �ε����κ��� ī�� ���� ��ȣ�� �ҷ��´�
    public int IndexFromDeck(int i)
    {
        return deck[i].GetComponent<CardBase>().index;
    }
}
