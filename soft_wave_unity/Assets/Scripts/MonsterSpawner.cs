using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] Monsters = new GameObject[5];
    private int[] WaveMonsters = {0, 10, 20, 30}; //1웨이브 10마리, 2웨이브 20마리...
    private int wave = 0;
    private int level = 0;
    private int WaveCool = 60;
    private float time = 0;
    private float cool = 0;
    private float SpawnperWave;
    void Start()
    {
        NextWave();
    }

    void Update()
    {
        cool += Time.deltaTime;
        time += Time.deltaTime;
        if(time >= WaveCool)
        {
            time = 0;
            NextWave();
        }

        if(cool >= SpawnperWave)
        {
            cool = 0;
            MonsterSpawn();
        }
    }

    void NextWave()
    {
        wave++;
        if(wave%3 == 0)
            level++;
        SpawnperWave = WaveCool / WaveMonsters[wave];
    }

    void MonsterSpawn()
    {
        GameObject Monster = Instantiate(Monsters[level]);
        Monster.transform.position = transform.position;
    }
}
