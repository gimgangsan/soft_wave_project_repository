using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    Collider2D Collider;
    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }

    public void HitAll()
    {
        Collider2D[] Collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(10, 0.6f), transform.eulerAngles.z);
        for(int i = 0; i < Collisions.Length; i++)
        {
            if (Collisions[i].CompareTag("Enemy"))
            {
                Collisions[i].GetComponent<MonsterBehavior>().Attacked(1);
            }
        }
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
