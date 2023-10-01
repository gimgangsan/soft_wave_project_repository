using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Threading;

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
    public List<int> deck = new List<int>();    // 플레이어가 소지한 덱
    public int[] hands = new int[5];            // 플레이어가 손에 들고 있는 패
    public int drawIndex = 0;                   // 이번 드로우에서 뽑을 카드 인덱스

    private static CardManager _instance;       // 싱글턴 패턴 구현부

    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;

        deck = new List<int>(new[] { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 });    // 테스트 목적으로 임의의 덱을 소유하도록 함
    }

    void Update()
    {
        // 테스트 목적
        // 1-5 키를 누르면 손에 카드를 드로우
        // 6-0 키를 누르면 손에 든 카드를 냄
        if (Input.GetKeyDown(KeyCode.Alpha1)) DrawCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) DrawCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) DrawCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) DrawCard(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) DrawCard(4);

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
    }

    // 카드 뽑기 예제
    void DrawCard(int handIndex)
    {
        if (drawIndex == deck.Count())              // 뽑을 인덱스가 마지막에 도달한 경우
        {
            ShuffleDeck();                          // 덱을 셔플한 후
            drawIndex = 0;                          // 처음부터 다시 시작
        }

        CardUIManager.Instance.DrawCard(handIndex, deck[drawIndex]);  // UI에 카드 뽑기 함수를 호출
        hands[handIndex] = deck[drawIndex++];       // 손에 든 패 갱신 후, drawIndex 증가
    }

    // 덱 셔플
    // 피셔-예이츠(Fisher-Yate) 셔플 방식으로 구현
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
