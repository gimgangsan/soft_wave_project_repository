using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Linq;
using UnityEngine.EventSystems;

// �� ���� UI�� �����ϴ� �Ŵ��� ��ũ��Ʈ
public class DeckManager : MonoBehaviour
{
    public GameObject deckManagerUI;    // �� ���� UI ĵ����
    public GameObject contentBox;       // ī����� ǥ�õ� ��� ����

    public GameObject cardPrefab;       // �⺻ ī�� ������
    public GameObject dropdownMenu;     // ����/���� �� �۾� ��Ӵٿ� �޴�
    public GameObject craftButton;      // ī�� ���� ��ư
    public GameObject removeButton;     // ī�� ���� ��ư
    public GameObject removeMessage;    // ī�� ���� �� Ȯ�� �޽���

    private Vector2 dropdownUpperPos;
    private Vector2 dropdownLowerPos;
    const int horizontalGap = 380;      // ī�� ������ ���� �Ÿ�
    const int verticalGap = 520;        // ī�� ������ ���� �Ÿ�

    GameObject[] cardList;              // ǥ�õǰ� �ִ� ī��鿡 ���� ���۷���

    public int selectedCard;            // ���� ���õ� ī��
    bool isClosing;                     // ���� ȭ���� �ݰ� �ִ��� Ȯ��
    bool canClose = true;               // ���� ȭ���� ���� �� �մ��� Ȯ��

    public void Awake()
    {
        dropdownUpperPos = craftButton.GetComponent<RectTransform>().localPosition;
        dropdownLowerPos = removeButton.GetComponent<RectTransform>().localPosition;
    }

    // ī�� ȹ�� ȭ�鿡�� ���� ��ư Ŭ�� �� �� �Լ� ȣ��
    // �� ���� ���� UI�� �ʱ�ȭ�Ѵ�
    public void Initialize()
    {
        deckManagerUI.SetActive(true);                                  // ĵ���� Ȱ��ȭ
        deckManagerUI.GetComponent<Animator>().SetTrigger("Fade In");   // ���̵��� �ִϸ��̼� ���

        RefreachCardList();                                             // ī�� ��� �ʱ�ȭ
    }

    // ī�� ����� �ʱ�ȭ�ϴ� �Լ�
    void RefreachCardList()
    {
        if (cardList != null)               // ������ ������ ī�� ������Ʈ�� ������...
        {
            foreach (var card in cardList)  // ��� �����Ѵ�
            {
                Destroy(card);
            }
        }

        int size = CardManager.Instance.inventory.Count;     // ���� ũ��
        cardList = new GameObject[size];                // ������Ʈ�� ���� �迭 �ʱ�ȭ
        int height = size / 4 + 1;                      // ��Ͽ� ǥ�õ� ���� ����

        float boxHeight = height * 550;                 // ī�带 ���� ������ ���� ���
        RectTransform rectTransform = contentBox.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, boxHeight);    // ����� ���̸� ��� ���ڿ� ����

        int currentX = 200;     // ���� ī�带 ��ġ�� (x, y) ��ǥ�� �ʱ�ȭ
        int currentY = -280;

        for (int i = 0; i < size; i++)  // ������ ��� ī�忡 ����
        {
            cardList[i] = Instantiate(cardPrefab);                  // ī�� ������Ʈ ���� �� �迭�� ����
            cardList[i].transform.SetParent(contentBox.transform);  // ��� ������ �ڽ� ������Ʈ�� ����

            RectTransform cardTransform = cardList[i].GetComponent<RectTransform>();    // ��ġ ����
            cardTransform.anchorMin = new Vector2(0, 1);
            cardTransform.anchorMax = new Vector2(0, 1);
            cardTransform.anchoredPosition = new Vector2(currentX, currentY);
            cardTransform.localScale = Vector3.one * 1.8f;

            CardClickDetector clickDetector = cardList[i].AddComponent<CardClickDetector>();
            clickDetector.card = CardManager.Instance.inventory[i];
            Button button = cardList[i].AddComponent<Button>();     // ī�� ������Ʈ�� ��ư ������Ʈ�� �߰��� ��
            int temp = i;                                           // ���� ������ ���� �ӽ� ����
            button.onClick.AddListener(() => OnClickCard(temp));    // ī�� Ŭ���� ��Ӵٿ� �޴��� ǥ���ϴ� ������ �߰�

            foreach (Transform obj in cardList[i].transform)        // ī�� ������Ʈ�� �ڽ� ��...
            {
                if (obj.name == "Tint")
                {
                    obj.GetComponent<Image>().raycastTarget = true; // ƾƮ ������Ʈ�� ����ĳ��Ʈ Ÿ�� ���� ����
                }
            }

            CardUIManager.Instance.UpdateCard(cardList[i], CardManager.Instance.inventory[i].GetComponent<CardBase>().index);  // ī�� ���� ����
            Outline outline = cardList[i].AddComponent<Outline>();
            outline.effectDistance = new Vector2(3, -3);

            if (CardManager.Instance.inventory[i].GetComponent<CardBase>().inDeck)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }

            if (i % 4 == 3)                 // ���� ǥ�� ���� ī���� ��ȣ�� ���� ���� ��ġ�� ����
            {
                currentX = 200;
                currentY -= verticalGap;
            }
            else
            {
                currentX += horizontalGap;
            }
        }

        dropdownMenu.transform.SetAsLastSibling();  // ��Ӵٿ� �޴��� ������ ǥ�õǵ��� ���� ����
    }

    // ī�� Ŭ�� �� ȣ��
    // ��Ӵٿ� �޴� ǥ��, ���õ� ī�� ��ȣ ����
    public void OnClickCard(int i) {
        if (isClosing || !canClose) return; // UI�� �ݴ� ��, �Ǵ� ���� Ȯ�� â�� �� �����̸� �ٷ� ����

        selectedCard = i;               // ���õ� ī�� ��ȣ ����
        dropdownMenu.SetActive(true);   // ��Ӵٿ� �޴� Ȱ��ȭ
        RectTransform menuTransform = dropdownMenu.GetComponent<RectTransform>();
        menuTransform.anchoredPosition = cardList[i].GetComponent<RectTransform>().anchoredPosition;
        menuTransform.anchoredPosition += new Vector2(-771, 80);    // ��Ӵٿ� �޴� ��ġ ����

        if (CardManager.Instance.inventory[selectedCard].GetComponent<CardBase>().index == 4) {
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

    // ī�� ���� ��ư Ŭ�� �� ȣ��
    public void OnCraft()
    {
        if (isClosing || !canClose) return; // UI�� �ݴ� ��, �Ǵ� ���� Ȯ�� â�� �� �����̸� �ٷ� ����

        // ī�� ���� ���� ����...
    }

    // ī�� ���� ��ư Ŭ�� �� ȣ��
    public void OnRemove()
    {
        if (isClosing || !canClose) return; // UI�� �ݴ� ��, �Ǵ� ���� Ȯ�� â�� �� �����̸� �ٷ� ����
        if (CardManager.Instance.inventory.Count < 6) return;    // �ּ����� ī�忡�� �� ������ �� ������ ����
        if (CardManager.Instance.deck.Count < 6 && CardManager.Instance.inventory[selectedCard].GetComponent<CardBase>().inDeck) return;

        canClose = false;               // ���� Ȯ�� â�� �� ���¿����� �۾� ����
        removeMessage.SetActive(true);  // ���� Ȯ�� â Ȱ��ȭ

        foreach(Transform obj in removeMessage.transform)
        {
            if (obj.name == "Remove Text")
            {
                string text = "Remove?\n" + CardInfo.cardInfo[CardManager.Instance.inventory[selectedCard].GetComponent<CardBase>().index].name;
                obj.GetComponent<TextMeshProUGUI>().text = text;    // ���� Ȯ�� â�� ǥ���� �ؽ�Ʈ ����
            }
        }
    }
    
    // �� ���� �Ϸ� ��ư Ŭ�� �� ȣ��
    public void OnComplete()
    {
        if (isClosing || !canClose) return; // UI�� �ݴ� ��, �Ǵ� ���� Ȯ�� â�� �� �����̸� �ٷ� ����
        isClosing = true;                   // �ݴ� ������ ����

        dropdownMenu.SetActive(false);      // ��Ӵٿ� �޴� ��Ȱ��ȭ
        foreach(var card in cardList)       // ��� ī�� ������Ʈ ����
        {
            Destroy(card);
        }

        deckManagerUI.GetComponent<Animator>().SetTrigger("Fade Out");  // ���̵� �ƿ� �ִϸ��̼� ���
        StartCoroutine("FadeOff");                                      // UI ��Ȱ��ȭ�� ���� �ڷ�ƾ ȣ��
    }

    // ���� Ȯ�� ��ư Ŭ�� �� ȣ��
    public void OnConfirmRemove()
    {
        CardManager.Instance.removeFromInventory(CardManager.Instance.inventory[selectedCard]);  // ������ ī�� ����
        RefreachCardList();                                             // ī�� ��� ���ΰ�ħ

        dropdownMenu.SetActive(false);      // ��Ӵٿ� �޴� ��Ȱ��ȭ
        removeMessage.SetActive(false);     // ���� Ȯ�� â ��Ȱ��ȭ
        canClose = true;
    }

    // ���� ��� ��ư Ŭ�� �� ȣ��
    public void OnCancelRemove()
    {
        removeMessage.SetActive(false);     // ���� Ȯ�� â ��Ȱ��ȭ
        canClose = true;
    }

    // ���� �ð� �� UI�� ��Ȱ��ȭ�ϱ� ���� �Լ�
    IEnumerator FadeOff()
    {
        yield return new WaitForSeconds(0.3f);
        deckManagerUI.SetActive(false);
        isClosing = false;
    }
}
