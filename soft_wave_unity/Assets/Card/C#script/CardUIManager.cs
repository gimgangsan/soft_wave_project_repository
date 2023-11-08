using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public GameObject cardPrefab;                       // 정보가 입력되지 않은 카드 프리팹
    public Transform[] handsPos;                        // 뽑은 카드가 이동할 위치들
    public Transform peekPos;
    private GameObject peekCard;
    public GameObject[] cardsInHands;                   // 현재 손에 있는 카드에 대한 레퍼런스를 저장

    private static CardUIManager _instance; // 싱글턴 패턴 구현
    public static CardUIManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        cardsInHands = new GameObject[5];
    }

    // 카드 뽑기 애니메이션 구현
    public void DrawCard(int handIndex, int cardIndex)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);                // 프리팹으로 카드 오브젝트 생성
        UpdateCard(newCard, cardIndex);                                         // 카드 정보에 맞게 외형 업데이트
        newCard.GetComponent<Animator>().SetInteger("HandIndex", handIndex);    // 주어진 번호의 위치로 이동하는 애니메이션 실행
        cardsInHands[handIndex] = newCard;                                      // 그 위치에 있던 카드를 현재 카드로 갱신
    }

    // 카드 사용 애니메이션 구현
    public void Discard(int handIndex)
    {
        if (!cardsInHands[handIndex]) return;       // 해당 위치에 아무 카드가 없을 경우

        cardsInHands[handIndex].GetComponent<Animator>().SetTrigger("Discard"); // 카드를 사용하는 애니메이션 실행
        StartCoroutine(UpdateHandWithDelay(handIndex, null));                   // 애니메이션 종료 후 마무리 코루틴 호출
    }

    // card가 가리키는 카드 오브젝트에 cardIndex에 해당하는 정보를 적용
    public void UpdateCard(GameObject card, int cardIndex)
    {
        foreach (Transform obj in card.GetComponent<Transform>())   // 카드 오브젝트의 자식 오브젝트들 중에서 검색
        {
            if (obj.name == "Image")                                                                                // 그림 갱신
            {
                obj.GetComponent<Image>().sprite = CardInfo.cardInfo[cardIndex].image;
                obj.GetComponent<Image>().color = Color.white;
            }
            if (obj.name == "Name") obj.GetComponent<TMP_Text>().text = CardInfo.cardInfo[cardIndex].name;          // 이름 갱신
            if (obj.name == "Description") obj.GetComponent<TMP_Text>().text = CardInfo.cardInfo[cardIndex].desc;   // 설명 갱신
        }
    }

    // 미리보기 카드를 cardIndex에 해당하는 카드로 갱신
    public void UpdatePeekCard(int cardIndex)
    {
        if (peekCard == null)
        {
            peekCard = Instantiate(cardPrefab, peekPos);
        }
        UpdateCard(peekCard, cardIndex);
    }

    // 카드 뽑기/사용 애니메이션 후 마무리 코루틴
    IEnumerator UpdateHandWithDelay(int handIndex, GameObject newCard)
    {
        yield return new WaitForSeconds(.2f);   // .2초 후...

        Destroy(cardsInHands[handIndex]);       // 기존에 그 위치에 있던 카드는 파괴
        cardsInHands[handIndex] = newCard;      // 그 위치에 있던 카드를 현재 카드로 갱신
    }

    // 들고 있는 모든 패를 갱신 (세이브/로드 등에 활용)
    public void UpdateHands(int[] cardIndexes)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform);
            UpdateCard(newCard, cardIndexes[i]);
            newCard.transform.position = handsPos[i].position;
        }
    }
}