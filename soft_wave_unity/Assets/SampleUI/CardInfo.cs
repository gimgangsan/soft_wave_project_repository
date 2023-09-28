using UnityEngine;
using UnityEngine.UI;

// 카드 정보를 담은 구조체
public struct Card
{
    public string name;
    public Sprite image;
    public string desc;

    // 카드 구조체 생성자
    public Card(string name, Sprite image, string desc)
    {
        this.name = name;
        this.image = image;
        this.desc = desc;
    }
}

public static class CardInfo
{
    // 카드 번호를 enum으로 저장
    public enum CARD { ATTACK, DEFENSE, ROLL }

    // 카드 번호에 상응되는 정보를 저장
    public static Card[] cardInfo = { new Card("Attack", null, "Inflict 2 Damage"),     // 000
                                      new Card("Defense", null, "Defend 2 Damage"),     // 001
                                      new Card("Roll", null, "Roll Forward")            // 002
                                    };

    // 전체 카드의 수를 저장
    public static int size = cardInfo.Length;
}
