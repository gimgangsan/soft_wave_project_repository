using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicActions : MonoBehaviour
{
    public UnityEvent<DamageInfo> whenHarmed;
    public UnityEvent<HealInfo> whenHealed;
    public UnityEvent<HPInfo> whenHPChanged;
    public UnityEvent onDead;

    public int MaxHP = 500;
    public int HP = 500;
    public int Amour = 0;

    public int Defense = 0;
    public int DamageRatio = 1;

    private void Start()
    {
        General.Instance.script_player = this;
        whenHPChanged.Invoke(WriteHPInfo());
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            GetHeal(50, 0);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            GetHeal(0, 50);
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
        DamageInfo info = WriteDamageInfo(CalculateDamage(power));
        HP -= info.ReducedHP;
        Amour -= info.ReducedAmour;
        whenHarmed.Invoke(info);
        if (HP <= 0)
        {
            onDead.Invoke();
        }
        whenHPChanged.Invoke(WriteHPInfo());
    }

    public HPInfo WriteHPInfo()
    {
        HPInfo info = new()
        {
            MaxHP = this.MaxHP,
            HP = this.HP,
            Amour = this.Amour
        };
        return info;
    }

    public DamageInfo WriteDamageInfo(int Damage)
    {
        DamageInfo info = new();
        int ActualHarm;
        if (Damage <= Amour)
        {
            info.ReducedAmour = Damage;
        }
        else
        {
            ActualHarm = Damage - Amour;
            info.ReducedAmour = Amour;
            info.ReducedHP = ActualHarm;
        }
        return info;
    }

    public void GetHeal(int heal, int amour)
    {
        HealInfo info = WriteHealInfo(heal, amour);
        this.HP += info.HealedHP;
        this.Amour += info.HealedAmour;
        whenHealed.Invoke(info);
        whenHPChanged.Invoke(WriteHPInfo());
    }

    public HealInfo WriteHealInfo(int heal, int amour)
    {
        HealInfo info = new()
        {
            HealedHP = Mathf.Min(heal, (this.MaxHP - this.HP)),
            HealedAmour = amour
        };
        return info;
    }

    public int CalculateDamage(int power)
    {
        return (power - Defense) * DamageRatio;
    }
}
