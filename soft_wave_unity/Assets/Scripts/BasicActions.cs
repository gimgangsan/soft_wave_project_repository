using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasicActions : MonoBehaviour, IDamagable
{
    public UnityEvent<DamageInfo> whenHarmed;
    public UnityEvent<HealInfo> whenHealed;
    public UnityEvent<HPInfo> whenHPChanged;
    public UnityEvent onDead;
    Animator animator;
    SpriteRenderer PlayerRend;
    Vector3 targetPos;
    public Slider ExpBar;
    public GameObject PinPoint;
    public float MoveSpeed = 5f;

    public int MaxHP = 500;
    public int HP = 500;
    public int Amour = 0;
    public int Exp = 0;     // 경험치
    public int MaxExp = 50; // 레벨업에 필요한 경험치
    private const int ExpStep = 30; // 레벨업 시, MaxExp가 얼마나 증가할 것인가

    public int Defense = 0;
    public int DamageRatio = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayerRend = GetComponent<SpriteRenderer>();
        whenHPChanged.Invoke(WriteHPInfo());
        targetPos = new Vector3(0, -16, 0);
        ExpBar.value = Exp;
        ExpBar.maxValue = MaxExp;
    }

    // Update is called once per frame
    void Update()
    {
        if (General.Instance.isPause) return;

        if(Input.GetMouseButton(1)) //우클릭 시 플레이어 이동
        {
            if(transform.position.x > targetPos.x)
                PlayerRend.flipX = true;
            else
                PlayerRend.flipX = false;
            targetPos = General.Instance.MousePos();
        }
        if (targetPos != transform.position)
        {
            MoveToTarget();
            PinPoint.SetActive(true);
            PinPoint.transform.position = targetPos;
        }
        else    PinPoint.SetActive(false);
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

        if (HP <= 0)
        {
            GameObject.Find("GameOver UI").GetComponent<GameOverUI>().DoGameOver();
        }
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

    void MoveToTarget() //마우스 좌표로 플레이어 이동
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * MoveSpeed);
        if(transform.position != targetPos)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    public void GetExp(int exp)
    {
        Exp += exp;

        //레벨업
        if (Exp >= MaxExp)
        {
            Exp %= MaxExp;
            MaxExp += ExpStep;

            //선택지에 올라올 카드 선별
            int[] cardIndexes = new int[3];
            for(int i = 0; i < 3; i++)
                cardIndexes[i] = UnityEngine.Random.Range(1, General.Instance.cardsList.Length);
            General.Instance.getCardManager.GetCard(cardIndexes);   //카드 선택
        }

        //UI 조정
        ExpBar.maxValue = MaxExp;
        ExpBar.value = Exp;
    }
}
