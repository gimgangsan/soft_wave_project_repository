using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

// ī�� ������ ���� ����ü
public struct Card
{
    public string name;
    public Sprite image;
    public string desc;
    public ICard effects;

    // ī�� ����ü ������
    public Card(string name, Sprite image, string desc, ICard effects)
    {
        this.name = name;
        this.image = image;
        this.desc = desc;
        this.effects = effects;
    }
}

public static class CardInfo
{
    // ī�� ��ȣ�� �����Ǵ� ������ ����
    public static Dictionary<int, Card> cardInfo = new Dictionary<int, Card>();
    // ��ü ī���� ���� ����
    public static int size;
    static CardInfo()
    {
        string path = @"Assets\Card\CardDB.csv";                    // CardDB�� ���;  �̽������� ���ڰ� �ƴ��� �˷��ֱ� ���� @�� ����
        StreamReader streamReader = new StreamReader(path);         // �־��� ����� ������ ����

        streamReader.ReadLine().Split(',');                         // CSV������ ù��° ��(�� ���� �̸�)�� ��������
        string[] lines = streamReader.ReadToEnd().Split('\n');      // ������ ������ ���� �� ���͸� �������� ��� ���ڿ��� ����

        foreach (string line in lines)                              // ������ �� ���� line�� ��Ƽ� ��ȸ
        {
            if(line == "") continue;                                    // ���� CSV���Ͽ� �� ���� ���ԵǴ� ��츦 ó���ϱ� ����

            string[] datas = line.Split(',');                           // data�� �� ī���� �����͸� string�迭�� ����

            int     id       = int.Parse(datas[0]);                     // id           ���ڿ��� ���������� ��ȯ
            string  name     = datas[1];                                // name
            Sprite  image    = Resources.Load<Sprite>(datas[2]);        // image        Resoures�������� data[2]�ּҿ� �ش��ϴ� �̹����� ������
            string  desc     = datas[4];                                // desc

            Type type = Type.GetType(datas[3]);        // �ش� �̸��� ���� Ŭ������ �����ϴ��� Ȯ��
            ICard effects;
            if (type != null) effects = (ICard)(Activator.CreateInstance(type));    // �����ϴ� ��� �ش� Ŭ���� ����
            else effects = null;                                                    // �������� �ʴ� ��� null�� ó��

            cardInfo[id] = new Card(name, image, desc, effects);   // id�� key�� Card����ü�� value������ ����
            Debug.Log(id + " saved");
        }
        streamReader.Close();                                       // �޸� ���� ���� ����
        size = cardInfo.Count;
    }
}
