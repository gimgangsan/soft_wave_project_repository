using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

// 카드 정보를 담은 구조체
public struct Card
{
    public string name;
    public Sprite image;
    public string desc;
    public float cooltime;

    // 카드 구조체 생성자
    public Card(string name, Sprite image, float cooltime, string desc)
    {
        this.name = name;
        this.image = image;
        this.desc = desc;
        this.cooltime = cooltime;
    }
}

public static class CardInfo
{
    // 카드 번호에 상응되는 정보를 저장
    public static Dictionary<int, Card> cardInfo = new Dictionary<int, Card>();
    // 전체 카드의 수를 저장
    public static int size;
    static CardInfo()
    {
        string path = @"Assets\Card\CardDB.csv";                    // CardDB의 경로;  이스케이프 문자가 아님을 알려주기 위해 @를 붙임
        StreamReader streamReader = new StreamReader(path);         // 주어진 경로의 파일을 열기

        streamReader.ReadLine();                                    // CSV파일의 첫번째 줄(각 열의 이름)을 날려버림
        string[] lines = streamReader.ReadToEnd().Split('\n');      // 파일을 끝까지 읽은 후 엔터를 기준으로 끊어서 문자열로 저장

        foreach (string line in lines)                              // 파일의 각 행을 line에 담아서 순회
        {
            if(line == "") continue;                                    // 종종 CSV파일에 빈 행이 포함되는 경우를 처리하기 위해

            string[] datas = line.Split(',');                           // data에 각 카드의 데이터를 string배열로 저장

            int     id       = int.Parse(datas[0]);                     // id           문자열을 정수형으로 변환
            string  name     = datas[1];                                // name
            Sprite  image    = Resources.Load<Sprite>(datas[2]);        // image        Resoures파일에서 data[2]주소에 해당하는 이미지를 가져옴
            float   cooltime = float.Parse(datas[3]);                   // cooltime     문자열을 실수형으로 변환
            string  desc     = datas[4];                                // desc

            cardInfo[id] = new Card(name, image, cooltime, desc);   // id를 key로 Card구조체를 value값으로 저장
        }
        streamReader.Close();                                       // 메모리 낭비를 막기 위해
        size = cardInfo.Count;
    }
}
