using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;
    SpriteRenderer rend;
    Animator animator;
    BasicActions ba;
    
    public float blinkDuration = 0.5f;
    public float health = 3f;
    public float speed = 3f;
    public float contactDistance = 1f;

    //bool follow = false;

    public float attackCooldown = 0;
    public float attackCoolRate = 1f;
    public float attackRange = 5f;
    public int attackDamage = 1;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ba = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicActions>();
    }

    void Update()
    {
        FollowTarget();
        Attacking();
        if(attackCooldown >= 0)
            attackCooldown -= Time.deltaTime;

    }
    
    void FollowTarget()
    {
        if(Vector2.Distance(transform.position, target.position) > contactDistance && Vector2.Distance(transform.position, target.position) < attackRange)
        {
            if(transform.position.x - target.position.x < 0)
                rend.flipX = false;
            else
                rend.flipX = true;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
        }       
    }

    void Attacking()
    {
        if(Vector2.Distance(transform.position, target.position) <= 2f)
        {
            if(attackCooldown <= 0)
            {
                ba.GetDamage(attackDamage);
                attackCooldown = attackCoolRate;
                animator.SetTrigger("isAttack");
            }
        }
    }

    public void Attacked()
    {
        StartCoroutine(BlinkObject());
    }

    IEnumerator BlinkObject()
    {
        Color originalColor = rend.color; // 현재 색상 저장

        rend.color = Color.red; // 빨간색
        yield return new WaitForSeconds(blinkDuration);

        rend.color = originalColor;
    }
}
