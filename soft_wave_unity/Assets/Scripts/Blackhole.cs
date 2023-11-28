using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    private float Duration = 3;
    private float Speed = 5;
    Rigidbody2D Rigidbody;
    private void Awake()
    {
        Destroy(gameObject, Duration);
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.transform.position = transform.position;
        }
    }

    public void SetDir(float degree)
    {
        float Radian = transform.eulerAngles.z * Mathf.Deg2Rad;
        Rigidbody.velocity = new Vector2(Mathf.Cos(Radian) * Speed, Mathf.Sin(Radian) * Speed);
    }
}
