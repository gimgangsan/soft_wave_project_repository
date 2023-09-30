using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/* 10/1 00:50 수정사항 - 송경민
 * 정수배열이었던 deck을 딕셔너리로 변경
 * deck의 key는 카드의 고유 ID (CardDB.csv 참고)
 * deck의 value는 해당 카드의 개수
 * Update에서 랜덤한 카드를 드로우하도록 변경
 * UseCard메소드의 카드 개수 증가 코드를 수정
 * DrawCard메소드의 카드 개수 감소 코드를 수정, 사용하지 않았던 hands배열을 사용하도록 수정
 * GetRandomIndex의 코드 대폭 수정
 */
public class CardManager : MonoBehaviour
{
    [SerializeField] public Dictionary<int, int> deck;   // 플레이어가 소유한 덱
    public int[] hands = new int[5];            // 플레이어가 손에 들고 있는 패

    private static CardManager _instance;       // 싱글턴 패턴 구현부

    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;

        deck = new Dictionary<int, int> { { 1, 10 }, { 2, 10 }, { 3, 10 } };       // 테스트 목적으로 임의의 덱을 소유하도록 함
    }

    void Update()
    {
        // 테스트 목적
        // 1-5 키를 누르면 손에 카드를 드로우
        // 6-0 키를 누르면 손에 든 카드를 냄
        if (Input.GetKeyDown(KeyCode.Alpha1)) DrawCard(0, GetRandomIndex());
        if (Input.GetKeyDown(KeyCode.Alpha2)) DrawCard(1, GetRandomIndex());
        if (Input.GetKeyDown(KeyCode.Alpha3)) DrawCard(2, GetRandomIndex());
        if (Input.GetKeyDown(KeyCode.Alpha4)) DrawCard(3, GetRandomIndex());
        if (Input.GetKeyDown(KeyCode.Alpha5)) DrawCard(4, GetRandomIndex());

        if (Input.GetKeyDown(KeyCode.Alpha6)) UseCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha7)) UseCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha8)) UseCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha9)) UseCard(3);
        if (Input.GetKeyDown(KeyCode.Alpha0)) UseCard(4);
    }

    // 카드 사용 예제
    void UseCard(int handIndex)
    {
        CardUIManager.Instance.Discard(handIndex);              // UI에 카드 사용 함수를 호출
        deck[hands[handIndex]] = deck[hands[handIndex]] +1;     // 현재 손에 있는 카드를 덱에 반환
    }

    // 카드 뽑기 예제
    void DrawCard(int handIndex, int cardIndex)
    {
        if (!(deck[cardIndex] > 0))     // 소지하지 않은 카드를 뽑으려고 할 경우
        {
            Debug.Log("소지하지 않은 카드를 뽑으려고 함.");
            return;
        }

        deck[cardIndex] = deck[cardIndex] - 1;                      // 덱에서 카드를 하나 가져옴
        CardUIManager.Instance.DrawCard(handIndex, cardIndex);      // UI에 카드 뽑기 함수를 호출
        hands[handIndex] = cardIndex;
        Debug.Log($"{deck[1]}, {deck[2]}, {deck[3]}");              // 딕셔너리는 인스펙터창에 표시가 안돼서 만든 코드
    }

    // 가진 카드의 수를 고려해 랜덤한 인덱스를 반환하는 함수
    // 뽑을 카드의 인덱스를 정할 때 사용할 수 있음
    public int GetRandomIndex()
    {
        int total = 0;                              // total에는 덱에 있는 모든 카드의 개수가 저장됨
        foreach(int value in deck.Values)           // 카드의 개수가 저장된 deck.Values를 순회하며 total에 모든 카드의 개수를 저장
            total += value;

        int rand = Random.Range(1, total + 1);      // 1~모든카드의개수 사이의 정수 하나를 선택
        foreach(int key in deck.Keys)               // deck을 순회하며 선택한 정수에서 value를 뺌
        {
            if (rand <= deck[key])                  // 마침내 rand를 value보다 작게 만든 key가 선택됨
                return key;
            rand -= deck[key];
        }

        return -1;                                  // 모든 카드를 소모하면 오류 발생
    }
}
