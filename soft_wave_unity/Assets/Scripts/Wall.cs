using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour, IDamagable
{
    public float Duration;
    public int hp = 200;
    private void Awake()
    {
        Destroy(gameObject, Duration);
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
