using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public float Speed;
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<MonsterBehavior>().Attacked();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float Radian = transform.eulerAngles.z * Mathf.Deg2Rad;
        Rigidbody.velocity = new Vector2(Mathf.Cos(Radian) * Speed, Mathf.Sin(Radian) * Speed) ;
    }

    public void SetDir(float degree)
    {
        transform.eulerAngles = new Vector3(0, 0, degree);
    }
}
