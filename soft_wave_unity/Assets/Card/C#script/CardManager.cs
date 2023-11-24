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
    public UnityEvent<int> whenCasting; // 카드 사용시 이벤트 (카드 ID를 매개변수로 전달)
    public UnityEvent<int> whenHit; // 데미지를 줄 때 이벤트 (준 데미지 량을 매개변수로 전달)

    public List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> deck;              // 플레이어가 소지한 덱
    public GameObject[] hands;           // 플레이어가 손에 들고 있는 패
    public int drawIndex = 0;           // 이번 드로우에서 뽑을 카드 인덱스
    public int peekIndex = 0;           
    public Slider mana;                 // 마나
    public float manaRestoreSpeed;      // 실제 스태미나
    public int manaConsume;

    private static CardManager _instance;       // 싱글턴 패턴 구현부
    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        hands = new GameObject[5];
        deck = new List<GameObject>(inventory);

        int[] initInven = { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 }; // 테스트 목적으로 임의의 덱을 소유하도록 함
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
        mana.value += manaRestoreSpeed * Time.deltaTime;      // 스태미나 회복

        // 테스트 목적
        // 1-4 키를 누르면 손에 든 카드를 냄
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseCard(3);
        
        for (int handIndex = 0; handIndex < 4; handIndex++)                     
        {
            if (CardUIManager.Instance.cardsInHands[handIndex] == null) DrawCard(handIndex);       // 빈 패를 확인하고 드로우
        }        
    }

    // 덱에 카드 추가
    public void addToDeck(GameObject obj)
    {
        deck.Add(obj);        // cardIndex가 가리키는 카드를 덱의 맨뒤에 추가한다
        int index = IndexFromObject(obj);
        if (CardInfo.cardInfo[index].script != null)
        {
            Type script = CardInfo.cardInfo[index].script;
            ((ICard)(obj.GetComponent(script))).OnAcquire();     // 카드 추가 효과 호출
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

    // 덱에서 cardIndex가 가리키는 카드 제거
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

    // 카드 사용
    void UseCard(int handIndex)
    {
        if (CardUIManager.Instance.cardsInHands[handIndex] == null) return;     // 패를 가지고 있는지 검사
        if (mana.value < manaConsume) return;                                   // 스태미나가 충분한지 검사
        Debug.Log(handIndex + "/" + hands.Length);
        int index = IndexFromObject(hands[handIndex]);
        if (CardInfo.cardInfo[index].script != null)
        {
            Type script = CardInfo.cardInfo[index].script;
            ((ICard)(hands[handIndex].GetComponent(script))).OnUse();     // 카드 사용 효과 호출
        }

        CardUIManager.Instance.Discard(handIndex);      // UI에 카드 사용 함수를 호출

        mana.value -= manaConsume;       // 스태미나 소모
    }

    // 카드 뽑기
    void DrawCard(int handIndex)
    {
        CardUIManager.Instance.DrawCard(handIndex, IndexFromObject(deck[drawIndex]));  // UI에 카드 뽑기 함수를 호출
        hands[handIndex] = deck[drawIndex++];       // 손에 든 패 갱신 후, drawIndex 증가
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
            ((ICard)(hands[handIndex].GetComponent(script))).OnDraw();     // 카드 드로우 효과 호출
        }
    }

    // 덱 셔플
    // 피셔-예이츠(Fisher-Yate) 셔플 방식으로 구현
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
