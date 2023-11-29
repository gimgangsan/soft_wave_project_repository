using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Linq;
using UnityEngine.EventSystems;

// 덱 편집 UI를 관리하는 매니저 스크립트
public class DeckManager : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject deckManagerUI;    // 덱 편집 UI 캔버스
    public GameObject contentBox;       // 카드들이 표시될 목록 상자

    public GameObject cardPrefab;       // 기본 카드 프리팹
    public GameObject dropdownMenu;     // 결합/제거 등 작업 드롭다운 메뉴
    public GameObject craftButton;      // 카드 결합 버튼
    public GameObject removeButton;     // 카드 제거 버튼
    public GameObject removeMessage;    // 카드 제거 시 확인 메시지
    public GameObject BackgroundUI;
    public GameObject CardListViewUI;
    public Sprite highlightSprite;

    private Vector2 dropdownUpperPos;
    private Vector2 dropdownLowerPos;
    const int horizontalGap = 380;      // 카드 사이의 가로 거리
    const int verticalGap = 520;        // 카드 사이의 세로 거리

    GameObject[] cardList;              // 표시되고 있는 카드들에 대한 레퍼런스

    public int selectedCard;            // 현재 선택된 카드
    bool isClosing;                     // 현재 화면을 닫고 있는지 확인
    bool canClose = true;               // 현재 화면을 닫을 수 잇는지 확인
    bool isCrafting = false;

    private void Awake()
    {
        dropdownUpperPos = craftButton.GetComponent<RectTransform>().localPosition;
        dropdownLowerPos = removeButton.GetComponent<RectTransform>().localPosition;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && General.Instance.canOpenMenu)
        {
            Initialize();
        }
    }

    // 덱 편집 UI를 호출한다
    public void Initialize()
    {
        General.Instance.isPause = true;
        deckManagerUI.SetActive(true);                                  // 캔버스 활성화
        deckManagerUI.GetComponent<Animator>().SetTrigger("Fade In");   // 페이드인 애니메이션 재생

        RefreachCardList();                                             // 카드 목록 초기화
    }

    // 카드 목록을 초기화하는 함수
    void RefreachCardList()
    {
        if (cardList != null)               // 기존에 생성된 카드 오브젝트가 있으면...
        {
            foreach (var card in cardList)  // 모두 제거한다
            {
                Destroy(card);
            }
        }

        int size = CardManager.Instance.inventory.Count;     // 덱의 크기
        cardList = new GameObject[size];                // 오브젝트를 담을 배열 초기화
        int height = size / 4 + 1;                      // 목록에 표시될 행의 개수

        float boxHeight = height * 550;                 // 카드를 담을 상자의 높이 계산
        RectTransform rectTransform = contentBox.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, boxHeight);    // 계산한 높이를 목록 상자에 적용

        int currentX = 200;     // 현재 카드를 배치할 (x, y) 좌표를 초기화
        int currentY = -280;

        for (int i = 0; i < size; i++)  // 소지한 모든 카드에 대해
        {
            cardList[i] = Instantiate(cardPrefab);                  // 카드 오브젝트 생성 후 배열에 저장
            cardList[i].transform.SetParent(contentBox.transform);  // 목록 상자의 자식 오브젝트로 설정

            RectTransform cardTransform = cardList[i].GetComponent<RectTransform>();    // 위치 설정
            cardTransform.anchorMin = new Vector2(0, 1);
            cardTransform.anchorMax = new Vector2(0, 1);
            cardTransform.anchoredPosition = new Vector2(currentX, currentY);
            cardTransform.localScale = Vector3.one * 1.8f;

            GameObject hl = new GameObject();
            hl.name = "Highlight";
            hl.transform.SetParent(cardList[i].transform, false);
            hl.AddComponent<RectTransform>();
            hl.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 270);
            Image hlImg = hl.AddComponent<Image>();
            hlImg.sprite = highlightSprite;
            hlImg.type = Image.Type.Sliced;

            CardClickDetector clickDetector = cardList[i].AddComponent<CardClickDetector>();
            clickDetector.card = CardManager.Instance.inventory[i];
            clickDetector.highlight = hl;

            Button button = cardList[i].AddComponent<Button>();     // 카드 오브젝트에 버튼 컴포넌트를 추가한 뒤
            int temp = i;                                           // 오류 방지를 위한 임시 변수
            button.onClick.AddListener(() => OnClickCard(temp));    // 카드 클릭시 드롭다운 메뉴를 표시하는 리스너 추가

            foreach (Transform obj in cardList[i].transform)        // 카드 오브젝트의 자식 중...
            {
                if (obj.name == "Tint")
                {
                    obj.GetComponent<Image>().raycastTarget = true; // 틴트 오브젝트의 레이캐스트 타겟 설정 변경
                }
            }

            CardUIManager.Instance.UpdateCard(cardList[i], CardManager.Instance.inventory[i].GetComponent<CardBase>().index);  // 카드 외형 갱신

            if (CardManager.Instance.inventory[i].GetComponent<CardBase>().inDeck)  // 덱 포함 여부에 따라 강조 테두리 온오프
            {
                hl.SetActive(true);
            }
            else
            {
                hl.SetActive(false);
            }

            if (i % 4 == 3)                 // 현재 표시 중인 카드의 번호에 따라 다음 위치를 설정
            {
                currentX = 200;
                currentY -= verticalGap;
            }
            else
            {
                currentX += horizontalGap;
            }
        }

        dropdownMenu.transform.SetAsLastSibling();  // 드롭다운 메뉴가 맨위에 표시되도록 순서 변경
    }

    // 카드 클릭 시 호출
    // 드롭다운 메뉴 표시, 선택된 카드 번호 갱신
    public void OnClickCard(int i) {
        if (isClosing || !canClose) return; // UI를 닫는 중, 또는 제거 확인 창이 뜬 상태이면 바로 리턴

        selectedCard = i;               // 선택된 카드 번호 갱신
        dropdownMenu.SetActive(true);   // 드롭다운 메뉴 활성화
        RectTransform menuTransform = dropdownMenu.GetComponent<RectTransform>();
        menuTransform.anchoredPosition = cardList[i].GetComponent<RectTransform>().anchoredPosition;
        menuTransform.anchoredPosition += new Vector2(-771, 80);    // 드롭다운 메뉴 위치 지정

        if (CardManager.Instance.inventory[selectedCard].GetComponent<CardBase>().index == 6)
        {
            menuTransform.sizeDelta = new Vector2(menuTransform.rect.width, 200);
            craftButton.SetActive(true);
            craftButton.GetComponent<RectTransform>().localPosition = dropdownUpperPos;
            removeButton.GetComponent<RectTransform>().localPosition = dropdownLowerPos;
        }
        else
        {
            menuTransform.sizeDelta = new Vector2(menuTransform.rect.width, 125);
            craftButton.SetActive(false);
            craftButton.GetComponent<RectTransform>().localPosition = dropdownLowerPos;
            removeButton.GetComponent<RectTransform>().localPosition = dropdownUpperPos + new Vector2(0, -35);
        }
    }

    // 카드 결합 버튼 클릭 시 호출
    public void OnCraft()
    {
        if (isClosing || !canClose) return; // UI를 닫는 중, 또는 제거 확인 창이 뜬 상태이면 바로 리턴

        isCrafting = true;
        BackgroundUI.SetActive(false);
        CardListViewUI.SetActive(false);

        // 카드 결합 관련 내용...
        Camera.main.transform.position = new Vector3(-100.5f, -18.25f ,Camera.main.transform.position.z);
        Canvas.SetActive(false);
    }

    public void OnCraftEnd()
    {
        Vector2 PlayerPos = General.Instance.script_player.transform.position;
        Camera.main.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, Camera.main.transform.position.z);
        Canvas.SetActive(true);
        isCrafting = false;
    }

    // 카드 제거 버튼 클릭 시 호출
    public void OnRemove()
    {
        if (isClosing || !canClose) return; // UI를 닫는 중, 또는 제거 확인 창이 뜬 상태이면 바로 리턴
        if (CardManager.Instance.inventory.Count < 6) return;    // 최소한의 카드에서 더 제거할 수 없도록 방지
        if (CardManager.Instance.deck.Count < 6 && CardManager.Instance.inventory[selectedCard].GetComponent<CardBase>().inDeck) return;

        canClose = false;               // 제거 확인 창이 뜬 상태에서는 작업 금지
        removeMessage.SetActive(true);  // 제거 확인 창 활성화

        foreach(Transform obj in removeMessage.transform)
        {
            if (obj.name == "Remove Text")
            {
                string text = "Remove?\n" + CardManager.Instance.inventory[selectedCard].GetComponent<CardBase>().name;
                obj.GetComponent<TextMeshProUGUI>().text = text;    // 제거 확인 창에 표시할 텍스트 갱신
            }
        }
    }
    
    // 덱 편집 완료 버튼 클릭 시 호출
    public void OnComplete()
    {
        if (isClosing || !canClose) return; // UI를 닫는 중, 또는 제거 확인 창이 뜬 상태이면 바로 리턴

        if (isCrafting)
        {
            BackgroundUI.SetActive(true);
            CardListViewUI.SetActive(true);
            OnCraftEnd();
            return;
        }

        isClosing = true;                   // 닫는 중으로 설정

        dropdownMenu.SetActive(false);      // 드롭다운 메뉴 비활성화
        foreach(var card in cardList)       // 모든 카드 오브젝트 제거
        {
            Destroy(card);
        }

        deckManagerUI.GetComponent<Animator>().SetTrigger("Fade Out");  // 페이드 아웃 애니메이션 재생
        StartCoroutine("FadeOff");                                      // UI 비활성화를 위한 코루틴 호출

        General.Instance.isPause = false;
    }

    // 제거 확인 버튼 클릭 시 호출
    public void OnConfirmRemove()
    {
        CardManager.Instance.removeFromInventory(CardManager.Instance.inventory[selectedCard]);  // 덱에서 카드 제거
        RefreachCardList();                                             // 카드 목록 새로고침

        dropdownMenu.SetActive(false);      // 드롭다운 메뉴 비활성화
        removeMessage.SetActive(false);     // 제거 확인 창 비활성화
        canClose = true;
    }

    // 제거 취소 버튼 클릭 시 호출
    public void OnCancelRemove()
    {
        removeMessage.SetActive(false);     // 제거 확인 창 비활성화
        canClose = true;
    }

    // 일정 시간 후 UI를 비활성화하기 위한 함수
    IEnumerator FadeOff()
    {
        yield return new WaitForSeconds(0.3f);
        deckManagerUI.SetActive(false);
        isClosing = false;
    }
}
