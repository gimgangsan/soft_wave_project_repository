using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetCardManager : MonoBehaviour
{
    public Transform[] cardsPos;        // ī�尡 ǥ�õ� ��ġ

    [HideInInspector]
    public int[] cardsIndices;          // ǥ�õǰ� �ִ� ī����� ī�� ��ȣ
    [HideInInspector]
    public int selectedIndex;           // ���� ���õ� ī�� ��ȣ

    [HideInInspector]
    public GameObject[] cardsRef;       // ǥ�õǰ� �ִ� ī�� ������Ʈ�鿡 ���� ����
    public GameObject getCardUI;        // ī�� ���� UI ĵ������ ���� ����
    public GameObject[] buttonsRef;     // UI �󿡼� Ŭ�� �Է��� �޴� ��ư�鿡 ���� ����

    public bool mustChoose;             // �ݵ�� ī�带 ���� �ϴ��� ����
    public bool isClosing;              // �ݴ� ���߿� ������ �� ������ �ϱ� ���� ����

    private static GetCardManager _instance;       // �̱��� ���� ������
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
        // �׽�Ʈ ����: F1-F2 Ű�� ������ ī�� ���� UI�� ǥ��
        if (Input.GetKeyDown(KeyCode.F1)) GetCard(new int[1] { 1 });
        if (Input.GetKeyDown(KeyCode.F2)) GetCard(new int[2] { 1, 2 });
        if (Input.GetKeyDown(KeyCode.F3)) GetCard(new int[3] { 1, 2, 3 });
    }

    // �ܺο��� �Լ� ȣ�� �� �̿��ϱ� ���� �Լ�
    // GetCardManager.Instance.GetCard(int []) �������� ȣ��
    // ���� �迭���� 1~3���� ī�� ��ȣ�� �ִ´�
    public void GetCard(int[] cardsToChoose)
    {
        General.Instance.isPause = true;    //�Ͻ�����
        getCardUI.SetActive(true);
        Initialize();
        DisplayCards(cardsToChoose);
    }

    // ī�� ���� UI �ʱ�ȭ
    private void Initialize()
    {
        foreach (GameObject button in buttonsRef)   // Ŭ�� �Է��� �޴� ��� ��ư�� �켱 ��Ȱ��ȭ 
        {
            button.SetActive(false);
        }

        foreach (GameObject card in cardsRef)       // ������ �����ϴ� ī�� ������Ʈ�� ��� �ı�
        {
            if (card != null) Destroy(card);
        }
        selectedIndex = -1;                         // �⺻ ���´� �ƹ��͵� �������� ���� ����
        isClosing = false;
    }

    // ī�� ���� UI�� ī�带 ǥ���ϴ� �Լ�
    private void DisplayCards(int[] cardsToChoose)
    {
        if (cardsToChoose.Length > 3) return;       // ī��� �� ���� 3�� ������ ǥ���� �� �ִ�

        bool[] activeSlot;                          // 5���� ���Կ��� ī�尡 ǥ�õ� ��ġ�� �����ϴ� �κ�

        if (cardsToChoose.Length == 3)              // �� ī�带 ǥ���� ���� 10101 �������� ǥ��
        {
            activeSlot = new bool[5] { true, false, true, false, true };
        }
        else if (cardsToChoose.Length == 2)         // �� ī�带 ǥ���� ���� 01010 �������� ǥ��
        {
            activeSlot = new bool[5] { false, true, false, true, false };
        }
        else                                        // �� ī�带 ǥ���� ���� 00100 �������� ǥ��
        {
            activeSlot = new bool[5] { false, false, true, false, false };
            if (mustChoose) selectedIndex = 2;
        }

        int currentIndex = 0;                       // cardsToChoose[currentIndex]�� ����Ű�� ī�带 ǥ���ϴ� ��
        for (int i = 0; i < 5; i++)                 // �ټ� ���� ������ ���鼭...
        {
            if (activeSlot[i])                      // ���� ���Կ� ī�带 �־�� �Ѵٸ�
            {
                buttonsRef[i].SetActive(true);      // Ŭ���� �޴� ��ư�� Ȱ��ȭ
                cardsRef[i] = Instantiate(CardUIManager.Instance.cardPrefab, cardsPos[i]);   // ī�� ������Ʈ ����
                CardUIManager.Instance.UpdateCard(cardsRef[i], cardsToChoose[currentIndex]); // ī�� ������Ʈ ���� ����
                cardsIndices[i] = cardsToChoose[currentIndex];              // ���� ǥ�õ� ī���� ��ȣ�� ������ ����
                cardsRef[i].transform.localScale = Vector3.one * 2;         // ī�� ũ�⸦ �� ��� Ű��
                cardsRef[i].GetComponent<Animator>().SetTrigger("Fade On"); // ī�� ���� �ִϸ��̼� ���
                currentIndex++;
            }
        }
    }

    // ī�� Ŭ�� �� �߻��ϴ� �̺�Ʈ
    // selectIndex ���ڴ� �ν����Ϳ��� ����
    public void OnCardSelect(int selectIndex)
    {
        if (isClosing) return;              // ī�� ���� UI�� �ݰ� �ִ� ���̶�� �ƹ��͵� ���� �ʴ´�

        if (selectedIndex == selectIndex && !mustChoose)    // ���� ������ ī��� ������ ���õ� ī�尡 ���ٸ�
        {
            selectedIndex = -1;                             // ī�� ������ ����
        }
        else {
            selectedIndex = selectIndex;                    // �� ���� ��� ���� ������ ī�� ����
        }

        for (int i = 0; i < 5; i++)                         // �� ī�带 ���鼭 ���� ���õ� ī�常 ��� ǥ��
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

    // Ȯ�� ��ư Ŭ�� �� �߻��ϴ� �̺�Ʈ
    public void OnComplete()
    {
        if (selectedIndex == -1 && mustChoose)      // �ݵ�� �ϳ��� ���� �ϴ� ��Ȳ���� ������ ī�尡 ���ٸ� �״�� ��ȯ
        {
            return;
        }
        else                                        // ���� ������ ī�带 ���� �߰�
        {
            if (selectedIndex != -1) CardManager.Instance.addToInventory(cardsIndices[selectedIndex]);
        }

        isClosing = true;
        getCardUI.GetComponent<Animator>().SetTrigger("Fade Off");          // ī�� ���� UI ����� ����
        for (int i = 0; i < 5; i++)                                         // �� ī�带 ���鼭 ������� �ִϸ��̼� ���
        {
            if (cardsRef[i] == null) continue;
            cardsRef[i].GetComponent<Animator>().SetTrigger("Fade Off");
        }
        StartCoroutine(FadeOff());                                          // 0.3�� �ڿ� ī�� ���� UI�� ��Ȱ��ȭ

        General.Instance.isPause = false;
    }

    // �� ���� ��ư Ŭ�� �� �߻��ϴ� �̺�Ʈ
    public void OnManagerDeck()
    {
        isClosing = true;
        getCardUI.GetComponent<Animator>().SetTrigger("Fade Off");          // ī�� ���� UI ����� ����
        for (int i = 0; i < 5; i++)                                         // �� ī�带 ���鼭 ������� �ִϸ��̼� ���
        {
            if (cardsRef[i] == null) continue;
            cardsRef[i].GetComponent<Animator>().SetTrigger("Fade Off");
        }
        StartCoroutine(FadeOff());                                          // 0.3�� �ڿ� ī�� ���� UI�� ��Ȱ��ȭ
        GetComponent<DeckManager>().Initialize();                           // �� ������ ȣ��
    }

    // ���� �ð� �� ī�� ���� UI�� ��Ȱ��ȭ�ϱ� ���� �Լ�
    IEnumerator FadeOff()
    {
        yield return new WaitForSeconds(0.3f);
        getCardUI.SetActive(false);
        isClosing = false;
    }
}
