using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class BasicActions : MonoBehaviour
{
    public UnityEvent<DamageInfo> whenHarmed;
    public UnityEvent<HealInfo> whenHealed;
    public UnityEvent<HPInfo> whenHPChanged;
    public UnityEvent onDead;
    Animator animator;
    SpriteRenderer PlayerRend;
    Vector3 mousePos, transPos, targetPos;
    public float MoveSpeed = 5f;

    public int MaxHP = 500;
    public int HP = 500;
    public int Amour = 0;

    public int Defense = 0;
    public int DamageRatio = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayerRend = GetComponent<SpriteRenderer>();
        General.Instance.script_player = this;
        whenHPChanged.Invoke(WriteHPInfo());
        targetPos = new Vector3(0, -16, 0);
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

        if(Input.GetMouseButton(1)) //우클릭 시 플레이어 이동
        {
            if(transform.position.x > targetPos.x)
                PlayerRend.flipX = true;
            else
                PlayerRend.flipX = false;
            CalTargetPos();
        }
        if(targetPos != transform.position)
            MoveToTarget();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(General.Instance.tag_relic))
        {
            collision.gameObject.GetComponent<IRelic>().GetEquiped();
        }
    }

    public void GetDamage(int power)
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
    void CalTargetPos() //마우스 좌표
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
    }

    void MoveToTarget() //마우스 좌표로 플레이어 이동
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * MoveSpeed);
        if(transform.position != targetPos)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }
}
