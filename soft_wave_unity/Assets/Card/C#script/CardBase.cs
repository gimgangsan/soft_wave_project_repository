using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    [HideInInspector]
    public int index;       // 오브젝트가 나타내는 카드의 카드 고유 번호
    public string name;
    public string description;
    public Sprite icon;
    public bool inDeck;     // 카드의 덱 포함 여부
}
