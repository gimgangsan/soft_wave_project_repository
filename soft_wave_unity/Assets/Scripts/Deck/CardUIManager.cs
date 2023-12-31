using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public GameObject cardPrefab;                       // 정보가 입력되지 않은 카드 프리팹
    public Transform[] handsPos;                        // 뽑은 카드가 이동할 위치들
    public GameObject peekCard;
    public GameObject[] cardsInHands;                   // 현재 손에 있는 카드에 대한 레퍼런스를 저장
    public TMP_FontAsset smoothFont;

    private static CardUIManager _instance; // 싱글턴 패턴 구현
    public static CardUIManager Instance
    { get { return _instance; } }

    void Awake()
    {
        _instance = this;
    }

    // 카드 뽑기 애니메이션 구현
    public void DrawCard(int handIndex, int cardIndex)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);    // 프리팹으로 카드 오브젝트 생성
        foreach (Transform obj in newCard.transform)
        {
            if (obj.name == "Name" || obj.name == "Description")
            {
                obj.GetComponent<TMP_Text>().font = smoothFont;
            }
        }
        UpdateCard(newCard, cardIndex);                             // 카드 정보에 맞게 외형 업데이트
        cardsInHands[handIndex] = newCard;                          // 그 위치에 있던 카드를 현재 카드로 갱신
        StartCoroutine(DrawnCardMove(newCard, handIndex));          // 카드 이동 코루틴
    }

    // 카드 사용 애니메이션 구현
    public void Discard(int handIndex)
    {
        if (!cardsInHands[handIndex]) return;       // 해당 위치에 아무 카드가 없을 경우

        cardsInHands[handIndex].GetComponent<Animator>().SetTrigger("Discard"); // 카드를 사용하는 애니메이션 실행
        StartCoroutine(UpwardMove(cardsInHands[handIndex]));                   // 애니메이션 종료 후 마무리 코루틴 호출
    }

    // card가 가리키는 카드 오브젝트에 cardIndex에 해당하는 정보를 적용
    public void UpdateCard(GameObject card, int cardIndex)
    {
        foreach (Transform obj in card.GetComponent<Transform>())   // 카드 오브젝트의 자식 오브젝트들 중에서 검색
        {
            if (obj.name == "Image")                                                                                // 그림 갱신
            {
                obj.GetComponent<Image>().sprite = CardManager.Instance.cardsList[cardIndex].GetComponent<CardBase>().icon;
                obj.GetComponent<Image>().color = Color.white;
            }
            if (obj.name == "Name") obj.GetComponent<TMP_Text>().text = CardManager.Instance.cardsList[cardIndex].GetComponent<CardBase>().name;          // 이름 갱신
            if (obj.name == "Description") obj.GetComponent<TMP_Text>().text = CardManager.Instance.cardsList[cardIndex].GetComponent<CardBase>().description;   // 설명 갱신
        }
    }

    // 미리보기 카드를 cardIndex에 해당하는 카드로 갱신
    public void UpdatePeekCard(int cardIndex)
    {
        UpdateCard(peekCard, cardIndex);
    }

    // 카드 뽑기/사용 애니메이션 후 마무리 코루틴
    IEnumerator UpwardMove(GameObject card)
    {
        Vector3 currentPos = card.transform.position;    // 카드UI의 위치

        for(int i = 0; i < 100; i++)
        {
            currentPos.y += Screen.height/200;
            card.transform.position = currentPos;
            yield return new WaitForSeconds(0.001f);
        }

        Destroy(card);       // 기존에 그 위치에 있던 카드는 파괴
        yield break;
    }

    //카드 이동 코루틴
    IEnumerator DrawnCardMove(GameObject newCard, int handIndex)
    {
        Vector3 currentPos;
        Vector3 StartPos = currentPos = peekCard.transform.position;
        Vector3 EndPos = handsPos[handIndex].position;
        float a = Screen.height * 0.6f / Mathf.Pow((EndPos.x - StartPos.x), 2);

        newCard.transform.position = StartPos; 
        while (currentPos.x < EndPos.x)
        {
            currentPos.x += Screen.width / 200 * (handIndex + 1);
            currentPos.y = -a * ((currentPos.x - StartPos.x) * (currentPos.x - EndPos.x)) + StartPos.y;  // 포물선 궤적을 그리며 움직임

            newCard.transform.position = currentPos;
            yield return new WaitForSeconds(0.001f);      // 다음 프레임때 반복문을 이어하기
        }
        newCard.transform.position = EndPos;   // 카드위치 조정

        yield break;
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