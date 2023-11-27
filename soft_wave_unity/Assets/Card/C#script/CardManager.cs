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
    public UnityEvent<int> whenCasting; // 카드 사용시 이벤트 (카드 ID를 매개변수로 전달)
    public UnityEvent<int> whenHit; // 데미지를 줄 때 이벤트 (준 데미지 량을 매개변수로 전달)

    public List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> deck;       // 플레이어가 소지한 덱
    public GameObject[] hands;          // 플레이어가 손에 들고 있는 패
    public int drawIndex = 0;           // 이번 드로우에서 뽑을 카드 인덱스
    public Slider mana;                 // 마나
    public float manaRestoreSpeed;
    public int manaConsume;

    private static CardManager _instance;       // 싱글턴 패턴 구현부
    public static CardManager Instance
    { get { return _instance; } }

    void Awake()
    {
        _instance = this;
        hands = new GameObject[5];
        deck = new List<GameObject>(inventory);

        int[] initInven = { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 }; // 테스트 목적으로 임의의 덱을 소유하도록 함
        bool[] isDeck = { true, true, true, true, false, true, true, true, false, false };
        LoadData(initInven, isDeck);
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

        // 빈 패를 확인하고 드로우
        for (int handIndex = 0; handIndex < CardUIManager.Instance.cardsInHands.Length; handIndex++)
            if (CardUIManager.Instance.cardsInHands[handIndex] == null) DrawCard(handIndex);       
    }

    // 덱에 카드 추가
    public void addToDeck(GameObject obj)
    {
        deck.Add(obj);                                          // 인벤토리에 있는 obj 카드를 덱에 추가한다
        int index = IndexFromObject(obj);
        if (CardInfo.cardInfo[index].script != null)
        {
            Type script = CardInfo.cardInfo[index].script;
            ((ICard)(obj.GetComponent(script))).OnAcquire();    // 카드 추가 효과 호출
        }

        obj.GetComponent<CardBase>().inDeck = true;             // 카드가 deck에 있다고 표시
    }

    // 인벤토리에 카드 추가
    public void addToInventory(int cardIndex)
    {
        GameObject obj = InstantiateCard(cardIndex);            // 카드 오브젝트 생성
        inventory.Add(obj);                                     // 인벤토리에 카드 오브젝트 추가
        inventory.Sort((a, b) => IndexFromObject(a) - IndexFromObject(b)) ; // 정렬
    }

    // 인벤토리에서 카드 제거
    public void removeFromInventory(GameObject obj)
    {
        inventory.Remove(obj);                          // 인벤토리에서 카드 제거
        if (deck.Contains(obj)) removeFromDeck(obj);    // 덱에도 카드가 있다면 덱에서도 제거

        Destroy(obj);                                   // 카드 오브젝트 파괴
    }

    // 덱에서 카드 제거
    public void removeFromDeck(GameObject obj)
    {
        int objIndexInDeck = -1;                        // 덱 내에서 제거할 카드의 인덱스를 구한다
        for(int i = 0; i < deck.Count(); i++)
        {
            if (deck[i] == obj) objIndexInDeck = i;
        }
        if (objIndexInDeck == -1) return;               // 덱 내에 카드가 없으면 리턴

        int cardIndex = IndexFromObject(obj);
        if (CardInfo.cardInfo[cardIndex].script != null)
        {
            Type script = CardInfo.cardInfo[cardIndex].script;
            ((ICard)(obj.GetComponent(script))).OnRemove();     // 카드 제거 효과 호출
        }
        deck.Remove(obj);                               // 덱에서 카드 제거
        obj.GetComponent<CardBase>().inDeck = false;

        if (drawIndex >= deck.Count)                    // 다음에 뽑을 인덱스가 범위를 벗어나 있으면
        {
            ShuffleDeck();                              // 셔플 후 다시 드로우 시작
            drawIndex = 0;
            CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex]));
        }
        else
        {
            if (objIndexInDeck == drawIndex)            // 다음에 뽑을 인덱스에서 카드를 제거했다면
            {
                CardUIManager.Instance.UpdatePeekCard(IndexFromObject(deck[drawIndex]));    // 미리보기 갱신
            }
            else if (objIndexInDeck < drawIndex)        // drawIndex 앞에 있는 카드를 제거했다면
            {
                drawIndex--;                            // drawIndex를 하나 줄여 정상적으로 진행되도록 함
            }
        }

        for(int i = 0; i < hands.Length; i++)           // 제거한 카드가 손패에 있었다면
        {
            if (hands[i] == obj)
            {
                CardUIManager.Instance.Discard(i);      // 카드를 손패에서 뺀 후
                DrawCard(i);                            // 새로 카드 드로우
            }
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
            ((ICard)(hands[handIndex].GetComponent(script))).OnUse(new AimInfo(Vector2.zero, Vector2.right));     // 카드 사용 효과 호출
        }

        CardUIManager.Instance.Discard(handIndex);      // UI에 카드 사용 함수를 호출

        mana.value -= manaConsume;       // 스태미나 소모
    }

    // 카드 뽑기
    void DrawCard(int handIndex)
    {
        CardUIManager.Instance.DrawCard(handIndex, IndexFromObject(deck[drawIndex]));  // UI에 카드 뽑기 함수를 호출
        hands[handIndex] = deck[drawIndex++];       // 손에 든 패 갱신 후, drawIndex 증가
        if (drawIndex >= deck.Count())
        {
            ShuffleDeck();
            drawIndex = 0;
        }
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

    // 카드 오브젝트 생성
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

    // 저장된 정보를 불러와 인벤토리/덱을 생성한다
    // inventory 매개변수에는 각 카드의 고유 번호를 배열로 저장한다.
    // isDeck 매개변수에는 각 카드의 덱 포함 여부를 저장한다. (위 배열과 일대일 대응)
    void LoadData(int[] inventory, bool[] isDeck)
    {
        if (inventory.Length != isDeck.Length) return;          // 크기가 다를 경우 리턴

        for(int i = 0; i < inventory.Length; i++)
        {
            addToInventory(inventory[i]);                       // 인벤토리에 카드 추가
            if (isDeck[i]) { addToDeck(this.inventory[i]); }    // 추가된 카드가 덱에 포함되어야 한다면 덱에도 추가
        }
    }

    // 오브젝트로부터 카드 고유 번호를 불러온다
    public int IndexFromObject(GameObject obj)
    {
        return obj.GetComponent<CardBase>().index;
    }

    // 인벤토리 인덱스로부터 카드 고유 번호를 불러온다
    public int IndexFromInven(int i)
    {
        return inventory[i].GetComponent<CardBase>().index;
    }

    // 덱 인덱스로부터 카드 고유 번호를 불러온다
    public int IndexFromDeck(int i)
    {
        return deck[i].GetComponent<CardBase>().index;
    }
}
