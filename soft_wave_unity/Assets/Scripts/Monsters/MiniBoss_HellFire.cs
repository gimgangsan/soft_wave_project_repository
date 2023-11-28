using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss_HellFire : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public float Speed;
    public int Damage;
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<BasicActions>().GetDamage(50);
            Destroy(gameObject);
        }
    }

    public void SetDir(float degree)
    {
        transform.eulerAngles = new Vector3(0, 0, degree);
        float Radian = transform.eulerAngles.z * Mathf.Deg2Rad;
        Rigidbody.velocity = new Vector2(Mathf.Cos(Radian) * Speed, Mathf.Sin(Radian) * Speed);
    }
}
