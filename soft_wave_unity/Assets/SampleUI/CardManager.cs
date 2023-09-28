using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int[] deck = new int[CardInfo.size]; // 플레이어가 소유한 덱
    public int[] hands = new int[5];            // 플레이어가 손에 들고 있는 패

    private static CardManager _instance;       // 싱글턴 패턴 구현부

    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;

        deck = new int[3] { 10, 10, 10 };       // 테스트 목적으로 임의의 덱을 소유하도록 함
    }

    void Update()
    {   
        // 테스트 목적
        // 1-5 키를 누르면 손에 카드를 드로우
        // 6-0 키를 누르면 손에 든 카드를 냄
        if (Input.GetKeyDown(KeyCode.Alpha1)) DrawCard(0, (int)CardInfo.CARD.ATTACK);
        if (Input.GetKeyDown(KeyCode.Alpha2)) DrawCard(1, (int)CardInfo.CARD.DEFENSE);
        if (Input.GetKeyDown(KeyCode.Alpha3)) DrawCard(2, (int)CardInfo.CARD.ROLL);
        if (Input.GetKeyDown(KeyCode.Alpha4)) DrawCard(3, (int)CardInfo.CARD.ATTACK);
        if (Input.GetKeyDown(KeyCode.Alpha5)) DrawCard(4, (int)CardInfo.CARD.DEFENSE);

        if (Input.GetKeyDown(KeyCode.Alpha6)) UseCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha7)) UseCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha8)) UseCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha9)) UseCard(3);
        if (Input.GetKeyDown(KeyCode.Alpha0)) UseCard(4);
    }

    // 카드 사용 예제
    void UseCard(int handIndex)
    {
        CardUIManager.Instance.Discard(handIndex);  // UI에 카드 사용 함수를 호출
        deck[hands[handIndex]]++;                   // 현재 손에 있는 카드를 덱에 반환
    }

    // 카드 뽑기 예제
    void DrawCard(int handIndex, int cardIndex)
    {
        if (!(deck[cardIndex] > 0))                 // 소지하지 않은 카드를 뽑으려고 할 경우
        {
            Debug.Log("소지하지 않은 카드를 뽑으려고 함.");
            return;
        }

        deck[cardIndex]--;                                      // 덱에서 카드를 하나 가져옴
        CardUIManager.Instance.DrawCard(handIndex, cardIndex);  // UI에 카드 뽑기 함수를 호출
    }

    // 가진 카드의 수를 고려해 랜덤한 인덱스를 반환하는 함수
    // 뽑을 카드의 인덱스를 정할 때 사용할 수 있음
    public int GetRandomIndex()
    {
        int total = 0;
        for(int i = 0; i < deck.Length; i++)
        {
            total += deck[i];
        }

        int index = 0;
        int rand = Random.Range(1, total + 1);

        while (deck[index] < rand)
        {
            rand -= deck[index];
            index++;
        }

        return index;
    }
}
