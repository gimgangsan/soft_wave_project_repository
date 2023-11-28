using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;

//using System.Diagnostics;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    protected Transform target;
    SpriteRenderer rend;
    Animator animator;
    AudioSource Audio;
    Color originalColor;

    
    private float blinkDuration = 0.5f;
    public float health;
    public float speed;
    public int dropExp;

    public float ContactDistance;
    protected float MeleeCooldown = 0;
    public float MeleeRate;
    public int MeleeDamage;


    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = General.Instance.script_player.transform;
        originalColor = rend.color; // 현재 색상 저장
        Audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (General.Instance.isPause) return;   //일시정지

        MeleeCooldown -= Time.deltaTime;
        FollowTarget();
    }
    
    public void FollowTarget()
    {
        if(transform.position.x - target.position.x < 0)
            rend.flipX = false;
        else
            rend.flipX = true;
        if(Vector2.Distance(transform.position, target.position) > ContactDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            if (MeleeCooldown <= 0)
            {
                General.Instance.script_player.GetDamage(MeleeDamage);
                MeleeCooldown = MeleeRate;
                animator.SetTrigger("isAttack");
            }
        }       
    }

    public void Attacked(float damage)
    {
        this.health -= damage;
        if(this.health <= 0)
        {
            General.Instance.script_player.GetExp(dropExp);
            Destroy(gameObject);
        }
        Audio.Play();
        StartCoroutine(BlinkObject());
    }

    IEnumerator BlinkObject()
    {
        rend.color = Color.red; // 빨간색
        yield return new WaitForSeconds(blinkDuration);

        rend.color = originalColor;
    }
}
