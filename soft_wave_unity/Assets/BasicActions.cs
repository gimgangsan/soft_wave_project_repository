using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicActions : MonoBehaviour
{

    public Action<int> onDamaged;
    public Action<int> onReducedAmour;
    public Action<int> onReducedHp;
    public Action onDead;

    public int HP = 500;
    public int Amour = 0;

    public int Defense = 0;
    public int DamageRatio = 1;

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

    void GetDamage(int power)
    {
        int Damage = CalculateDamage(power);
        if (Damage <= Amour)
        {
            Amour -= Damage;
            onReducedAmour?.Invoke(Damage);
        }
        else
        {
            int ActualHarm = Damage - Amour;
            onReducedAmour?.Invoke(Amour);
            Amour = 0;
            this.HP -= ActualHarm;
            onReducedHp?.Invoke(ActualHarm);
        }
        onDamaged?.Invoke(Damage);

        Debug.Log(HP);

        if (HP <= 0)
        {
            onDead?.Invoke();
            Debug.Log("player died");
        }
    }

    public int CalculateDamage(int power)
    {
        return (power - Defense) * DamageRatio;
    }
}
