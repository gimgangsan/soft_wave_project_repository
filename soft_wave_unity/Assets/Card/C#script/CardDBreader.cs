using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

// CardDB.csv파일을 읽기 위한 클래스
// CardDB에는 각 카드의 id(DB의 PrimaryKey), name, image경로, description이 저장됨
// 코드를 이해하는 데 도움을 줄 수 있는 유튜브 영상 https://youtu.be/SvUYPh6lnqo?t=540
public static class CardDBreader
{
    static string path = @"Assets\Card\CardDB.csv";          // CardDB의 경로; 이스케이프 문자가 아님을 알려주기 위해 @를 붙임

    public static string[] ReadDB(int ID)                           // ID를 기준으로 CSV파일의 각 행을 탐색하여 문자열 배열을 반환하는 함수
    {
        Stream readStream = new FileStream(path, FileMode.Open);        // 주어진 경로의 파일을 읽기모드로 열기
        StreamReader streamReader = new StreamReader(readStream);

        string line = null;                                             // line에는 CSV파일의 행이 저장됨
        for (int i = 0; i <= ID; i++)                                   // id == 0 : 각 필드의 제목; id >= 1 : id에 해당하는 카드의 정보가 담긴 행
            line = streamReader.ReadLine();                                 // 원하는 id에 도달할 때까지 계속 다음행으로 넘김
            
        readStream.Close();                                             // 혹시 모를 메모리 낭비를 막기 위해

        return line.Split(',');                                         // 읽은 행을 ','를 기준으로 나눠서 반환
    }
}