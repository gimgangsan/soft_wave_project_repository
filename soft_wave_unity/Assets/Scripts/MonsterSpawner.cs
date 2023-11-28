using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] Monsters = new GameObject[5];
    public GameObject[] MiniBosses = new GameObject[5];
    private int[,] WaveMonsters = new int[,] {{0, 10, 20, 30, 30, 40}, {0, 0, 0, 10, 20 ,30}}; //1웨이브 10마리, 2웨이브 20마리...
    private int wave = 0;
    //private int level = 0;
    private int bosslevel = 0;
    private int WaveCool = 60;
    private float time = 0;
    private float[] cool = new float[5];
    private float[] SpawnperWave = new float[5];
    void Start()
    {
        for(int i=0;i<5;i++)
        {
            cool[i] = 0;
            SpawnperWave[i] = 0;
        }
        NextWave();
    }

    void Update()
    {
        cool[0] += Time.deltaTime;
        cool[1] += Time.deltaTime;
        time += Time.deltaTime;
        if(time >= WaveCool && wave < 5) //일단 최대 웨이브 5로 설정
        {
            time = 0;
            NextWave();
        }

        if(cool[0] >= SpawnperWave[0])
        {
            cool[0] = 0;
            MonsterSpawn(0);
        }

        if(cool[1] >= SpawnperWave[1])
        {
            cool[1] = 0;
            MonsterSpawn(1);
        }
    }

    void NextWave()
    {
        wave++;
        /*if(wave%3 == 0)
            level++;*/
        for(int i=0;i<2;i++)
        {
            if(WaveMonsters[i, wave] == 0)
            {
                SpawnperWave[i] = 1000;
                break;
            }
            SpawnperWave[i] = WaveCool / WaveMonsters[i, wave];
        }
        if(wave%5 == 0)
        {
            GameObject MiniBoss = Instantiate(MiniBosses[bosslevel]);
            MiniBoss.transform.position = transform.position;
            bosslevel++;
        }
    }

    void MonsterSpawn(int n)
    {
        GameObject Monster = Instantiate(Monsters[n]);
        Monster.transform.position = transform.position;
    }
}
