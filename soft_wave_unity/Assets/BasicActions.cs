using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicActions : MonoBehaviour
{
    public Action<int> onDamaged;
    public Action onDead;

    public int HP = 500;

    private void Awake()
    {
        General.Instance.script_player = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetDamage(100);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetDamage(50);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(General.Instance.tag_relic))
        {
            collision.gameObject.GetComponent<IRelic>().GetEquiped();
        }
        
        
    }

    void GetDamage(int amount)
    {
        HP -= amount;
        if (onDamaged != null) onDamaged(amount);
        Debug.Log(HP);

        if (HP <= 0)
        {
            if(onDead != null) onDead();
            Debug.Log("player died");
        }
    }
}
