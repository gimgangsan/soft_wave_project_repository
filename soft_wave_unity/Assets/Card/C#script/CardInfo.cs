using UnityEngine;
using UnityEngine.UI;

/* 10/1 00:50 수정사항 - 송경민
 * 본 코드를 읽기 전에 CardDBreader.cs를 먼저 읽고 올 것
 * Card 구조체에 쿨타임(카드를 사용한 후 그 빈자리에 드로우되기까지의 시간) 변수 추가
 * 카드 데이터베이스를 CardInfo대신 CardDB.csv파일로 대체하는 것을 제안
 */

// 카드 정보를 담은 구조체
public struct Card
{
    public string name;
    public Sprite image;
    public string desc;
    public float cooltime;

    // 카드 구조체 생성자
    public Card(string name, Sprite image, string desc, float cooltime)
    {
        this.name = name;
        this.image = image;
        this.desc = desc;
        this.cooltime = cooltime;
    }
}

public static class CardInfo
{
    // 카드 번호를 enum으로 저장
    public enum CARD { ATTACK, DEFENSE, ROLL }

    // 카드 번호에 상응되는 정보를 저장
    public static Card[] cardInfo = { new Card("Attack", null, "Inflict 2 Damage", 0),     // 000
                                      new Card("Defense", null, "Defend 2 Damage", 0),     // 001
                                      new Card("Roll", null, "Roll Forward", 0)            // 002
                                    };

    // 전체 카드의 수를 저장
    public static int size = cardInfo.Length;
}
