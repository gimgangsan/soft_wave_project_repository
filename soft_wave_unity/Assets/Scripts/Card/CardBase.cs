using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    [HideInInspector]
    public int index;       // ������Ʈ�� ��Ÿ���� ī���� ī�� ���� ��ȣ
    public string name;
    public string description;
    public Sprite icon;
    public bool inDeck;     // ī���� �� ���� ����
}
