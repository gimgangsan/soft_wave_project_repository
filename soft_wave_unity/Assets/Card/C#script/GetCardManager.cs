using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetCardManager : MonoBehaviour
{
    public Transform[] cardsPos;        // 카드가 표시될 위치

    [HideInInspector]
    public int[] cardsIndices;          // 표시되고 있는 카드들의 카드 번호
    [HideInInspector]
    public int selectedIndex;           // 현재 선택된 카드 번호

    [HideInInspector]
    public GameObject[] cardsRef;       // 표시되고 있는 카드 오브젝트들에 대한 참조
    public GameObject getCardUI;        // 카드 선택 UI 캔버스에 대한 참조
    public GameObject[] buttonsRef;     // UI 상에서 클릭 입력을 받는 버튼들에 대한 참조

    public bool mustChoose;             // 반드시 카드를 골라야 하는지 여부
    public bool isClosing;              // 닫는 도중에 조작할 수 없도록 하기 위한 변수

    private static GetCardManager _instance;       // 싱글턴 패턴 구현부
    public static GetCardManager Instance
    { get { return _instance; } }

    void Start()
    {
        _instance = this;
        cardsRef = new GameObject[5];
        cardsIndices = new int[5];
    }

    void Update()
    {
        // 테스트 목적: F1-F2 키를 누르면 카드 선택 UI를 표시
        if (Input.GetKeyDown(KeyCode.F1)) GetCard(new int[1] { 1 });
        if (Input.GetKeyDown(KeyCode.F2)) GetCard(new int[2] { 1, 2 });
        if (Input.GetKeyDown(KeyCode.F3)) GetCard(new int[3] { 1, 2, 3 });
    }

    // 외부에서 함수 호출 시 이용하기 위한 함수
    // GetCardManager.Instance.GetCard(int []) 형식으로 호출
    // 인자 배열에는 1~3개의 카드 번호를 넣는다
    public void GetCard(int[] cardsToChoose)
    {
        General.Instance.isPause = true;    //일시정지
        getCardUI.SetActive(true);
        Initialize();
        DisplayCards(cardsToChoose);
    }

    // 카드 선택 UI 초기화
    private void Initialize()
    {
        foreach (GameObject button in buttonsRef)   // 클릭 입력을 받는 모든 버튼은 우선 비활성화 
        {
            button.SetActive(false);
        }

        foreach (GameObject card in cardsRef)       // 기존에 존재하던 카드 오브젝트들 모두 파괴
        {
            if (card != null) Destroy(card);
        }
        selectedIndex = -1;                         // 기본 상태는 아무것도 선택하지 않은 상태
        isClosing = false;
    }

    // 카드 선택 UI에 카드를 표시하는 함수
    private void DisplayCards(int[] cardsToChoose)
    {
        if (cardsToChoose.Length > 3) return;       // 카드는 한 번에 3개 까지만 표시할 수 있다

        bool[] activeSlot;                          // 5개의 슬롯에서 카드가 표시될 위치를 지정하는 부분

        if (cardsToChoose.Length == 3)              // 세 카드를 표시할 때는 10101 형식으로 표시
        {
            activeSlot = new bool[5] { true, false, true, false, true };
        }
        else if (cardsToChoose.Length == 2)         // 두 카드를 표시할 때는 01010 형식으로 표시
        {
            activeSlot = new bool[5] { false, true, false, true, false };
        }
        else                                        // 한 카드를 표시할 때는 00100 형식으로 표시
        {
            activeSlot = new bool[5] { false, false, true, false, false };
            if (mustChoose) selectedIndex = 2;
        }

        int currentIndex = 0;                       // cardsToChoose[currentIndex]가 가리키는 카드를 표시하는 중
        for (int i = 0; i < 5; i++)                 // 다섯 개의 슬롯을 돌면서...
        {
            if (activeSlot[i])                      // 현재 슬롯에 카드를 넣어야 한다면
            {
                buttonsRef[i].SetActive(true);      // 클릭을 받는 버튼을 활성화
                cardsRef[i] = Instantiate(CardUIManager.Instance.cardPrefab, cardsPos[i]);   // 카드 오브젝트 생성
                CardUIManager.Instance.UpdateCard(cardsRef[i], cardsToChoose[currentIndex]); // 카드 오브젝트 내용 갱신
                cardsIndices[i] = cardsToChoose[currentIndex];              // 현재 표시된 카드의 번호를 별도로 저장
                cardsRef[i].transform.localScale = Vector3.one * 2;         // 카드 크기를 두 배로 키움
                cardsRef[i].GetComponent<Animator>().SetTrigger("Fade On"); // 카드 등장 애니메이션 재생
                currentIndex++;
            }
        }
    }

    // 카드 클릭 시 발생하는 이벤트
    // selectIndex 인자는 인스펙터에서 설정
    public void OnCardSelect(int selectIndex)
    {
        if (isClosing) return;              // 카드 선택 UI를 닫고 있는 중이라면 아무것도 하지 않는다

        if (selectedIndex == selectIndex && !mustChoose)    // 현재 선택한 카드와 기존에 선택된 카드가 같다면
        {
            selectedIndex = -1;                             // 카드 선택을 해제
        }
        else {
            selectedIndex = selectIndex;                    // 그 외의 경우 현재 선택한 카드 갱신
        }

        for (int i = 0; i < 5; i++)                         // 각 카드를 돌면서 현재 선택된 카드만 밝게 표시
        {
            if (cardsRef[i] == null) continue;

            if (selectedIndex == -1 || i == selectedIndex)
            {
                cardsRef[i].GetComponent<Animator>().SetTrigger("Set Focus");
            }
            else
            {
                cardsRef[i].GetComponent<Animator>().SetTrigger("Lose Focus");
            }
        }
    }

    // 확인 버튼 클릭 시 발생하는 이벤트
    public void OnComplete()
    {
        if (selectedIndex == -1 && mustChoose)      // 반드시 하나를 골라야 하는 상황에서 선택한 카드가 없다면 그대로 반환
        {
            return;
        }
        else                                        // 현재 선택한 카드를 덱에 추가
        {
            if (selectedIndex != -1) CardManager.Instance.addToInventory(cardsIndices[selectedIndex]);
        }

        isClosing = true;
        getCardUI.GetComponent<Animator>().SetTrigger("Fade Off");          // 카드 선택 UI 배경을 지움
        for (int i = 0; i < 5; i++)                                         // 각 카드를 돌면서 사라지는 애니메이션 재생
        {
            if (cardsRef[i] == null) continue;
            cardsRef[i].GetComponent<Animator>().SetTrigger("Fade Off");
        }
        StartCoroutine(FadeOff());                                          // 0.3초 뒤에 카드 선택 UI를 비활성화

        General.Instance.isPause = false;
    }

    // 덱 수정 버튼 클릭 시 발생하는 이벤트
    public void OnManagerDeck()
    {
        isClosing = true;
        getCardUI.GetComponent<Animator>().SetTrigger("Fade Off");          // 카드 선택 UI 배경을 지움
        for (int i = 0; i < 5; i++)                                         // 각 카드를 돌면서 사라지는 애니메이션 재생
        {
            if (cardsRef[i] == null) continue;
            cardsRef[i].GetComponent<Animator>().SetTrigger("Fade Off");
        }
        StartCoroutine(FadeOff());                                          // 0.3초 뒤에 카드 선택 UI를 비활성화
        GetComponent<DeckManager>().Initialize();                           // 덱 관리자 호출
    }

    // 일정 시간 후 카드 선택 UI를 비활성화하기 위한 함수
    IEnumerator FadeOff()
    {
        yield return new WaitForSeconds(0.3f);
        getCardUI.SetActive(false);
        isClosing = false;
    }
}
