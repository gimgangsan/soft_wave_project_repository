using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class CardManager : MonoBehaviour
{
    public List<int> deck = new List<int>();            // 플레이어가 소지한 덱
    public int[] hands = new int[5];                    // 플레이어가 손에 들고 있는 패
    public int drawIndex = 0;                           // 이번 드로우에서 뽑을 카드 인덱스
    public Image[] staminaCircle;                       // 스태미나를 보여줄 UI
    public float stamina;                               // 실제 스태미나
    [SerializeField] public Slider[] handCooltime;      // 패의 쿨타임(카드를 사용한 후 다시 뽑기까지의 시간)을 보여줄 슬라이더

    private static CardManager _instance;       // 싱글턴 패턴 구현부

    public static CardManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        
        deck = new List<int>() { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 };    // 테스트 목적으로 임의의 덱을 소유하도록 함

        // 스테미나 초기화
        foreach (Image circle in staminaCircle) 
            circle.fillAmount = 0;
    }

    void Update()
    {
        stamina = Mathf.Min(stamina+Time.deltaTime, staminaCircle.Length);      // 스태미나 회복

        // 테스트 목적
        // 1-5 키를 누르면 손에 든 카드를 냄
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseCard(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) UseCard(4);

        // 각 패의 쿨타임 감소 && 쿨 찼으면 드로우
        for (int handIndex = 0; handIndex < 5; handIndex++)                     
        {
            if (handCooltime[handIndex].value > 0) handCooltime[handIndex].value -= Time.deltaTime;     // 쿨타임 감소
            else if (CardUIManager.Instance.cardsInHands[handIndex] == null) DrawCard(handIndex);       // 빈 패를 확인하고 드로우
        }

        for (int i = 0; i < staminaCircle.Length; i++)      // 스태미나 UI 관리
            staminaCircle[i].fillAmount = stamina - i;        
    }

    // 카드 사용 예제
    void UseCard(int handIndex)
    {
        if (CardUIManager.Instance.cardsInHands[handIndex] == null) return;     // 패를 가지고 있는지 검사
        if (stamina < 1) return;                                                // 스태미나가 충분한지 검사

        CardUIManager.Instance.Discard(handIndex);      // UI에 카드 사용 함수를 호출

        handCooltime[handIndex].value           // 쿨타임 적용
            = handCooltime[handIndex].maxValue
            = CardInfo.cardInfo[hands[handIndex]].cooltime;

        stamina -= 1;       // 스태미나 소모
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
