using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;
using UnityEngine.Events;

public class CardManager : MonoBehaviour
{
    public UnityEvent<int> whenCasting; // ī�� ���� �̺�Ʈ (ī�� ID�� �Ű������� ����)
    public UnityEvent<int> whenHit; // �������� �� �� �̺�Ʈ (�� ������ ���� �Ű������� ����)

    public List<int> inventory = new List<int>();
    public List<int> deck;            // �÷��̾ ������ ��
    public int[] hands = new int[5];                    // �÷��̾ �տ� ��� �ִ� ��
    public int drawIndex = 0;                           // �̹� ��ο쿡�� ���� ī�� �ε���
    public int peekIndex = 0;
    public Image[] staminaCircle;                       // ���¹̳��� ������ UI
    public float stamina;                               // ���� ���¹̳�

    private static CardManager _instance;       // �̱��� ���� ������
    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        
        inventory = new List<int>() { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 };    // �׽�Ʈ �������� ������ ���� �����ϵ��� ��
        inventory.Sort();
        deck = new List<int>(inventory);

        // ���׹̳� �ʱ�ȭ
        foreach (Image circle in staminaCircle) 
            circle.fillAmount = 0;
    }

    void Update()
    {
        stamina = Mathf.Min(stamina+Time.deltaTime, staminaCircle.Length);      // ���¹̳� ȸ��

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

        for (int i = 0; i < staminaCircle.Length; i++)      // ���¹̳� UI ����
            staminaCircle[i].fillAmount = stamina - i;        
    }

    // ���� ī�� �߰�
    public void addToDeck(int cardIndex)
    {
        deck.Add(cardIndex);        // cardIndex�� ����Ű�� ī�带 ���� �ǵڿ� �߰��Ѵ�
        if (CardInfo.cardInfo[cardIndex].effects != null) CardInfo.cardInfo[cardIndex].effects.OnAcquire();     // ī�� �߰� ȿ�� ȣ��
        Debug.Log(deck.ToString());
    }

    public void addToInventory(int cardIndex)
    {
        inventory.Add(cardIndex);
        inventory.Sort();
    }

    public void removeFromInventory(int cardIndex)
    {
        inventory.Remove(cardIndex);
        if(deck.Contains(cardIndex))   
            removeFromDeck(cardIndex);
    }

    // ������ cardIndex�� ����Ű�� ī�� ����
    public void removeFromDeck(int cardIndex)
    {
        if (CardInfo.cardInfo[cardIndex].effects != null) CardInfo.cardInfo[cardIndex].effects.OnRemove();
        deck.Remove(cardIndex);
    }

    // ������ deckIndex+1��° ī�� ����
    public void removeFromDeckAt(int deckIndex)
    {
        if (CardInfo.cardInfo[deck[deckIndex]].effects != null) CardInfo.cardInfo[deck[deckIndex]].effects.OnRemove(); // ī�� ���� ȿ�� ȣ��
        deck.RemoveAt(deckIndex);   // �� ������ (deckIndex+1)��° ī�带 �����Ѵ�
    }

    // ī�� ���
    void UseCard(int handIndex)
    {
        if (CardUIManager.Instance.cardsInHands[handIndex] == null) return;     // �и� ������ �ִ��� �˻�
        if (stamina < 1) return;                                                // ���¹̳��� ������� �˻�
        if (CardInfo.cardInfo[hands[handIndex]].effects != null) CardInfo.cardInfo[hands[handIndex]].effects.OnUse();  // ī�� ��� ȿ�� ȣ��

        CardUIManager.Instance.Discard(handIndex);      // UI�� ī�� ��� �Լ��� ȣ��

        stamina -= 1;       // ���¹̳� �Ҹ�
    }

    // ī�� �̱�
    void DrawCard(int handIndex)
    {
        CardUIManager.Instance.DrawCard(handIndex, deck[drawIndex]);  // UI�� ī�� �̱� �Լ��� ȣ��
        hands[handIndex] = deck[drawIndex++];       // �տ� �� �� ���� ��, drawIndex ����
        if (drawIndex == deck.Count())
        {
            ShuffleDeck();
            drawIndex = 0;
        }
        CardUIManager.Instance.UpdatePeekCard(deck[drawIndex]);
        if (CardInfo.cardInfo[hands[handIndex]].effects != null) CardInfo.cardInfo[hands[handIndex]].effects.OnDraw();  // ī�� ��ο� ȿ�� ȣ��
    }

    // �� ����
    // �Ǽ�-������(Fisher-Yate) ���� ������� ����
    void ShuffleDeck()
    {
        for (int i = deck.Count() - 1; i > 1; i--) {
            int rand = Random.Range(0, i + 1);
            int temp = deck[rand];
            deck[rand] = deck[i];
            deck[i] = temp;
        }
    }
}
