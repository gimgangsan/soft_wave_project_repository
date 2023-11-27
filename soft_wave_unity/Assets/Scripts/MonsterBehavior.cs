using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;

//using System.Diagnostics;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;
    SpriteRenderer rend;
    Animator animator;
    Color originalColor;

    public GameObject Projectile;
    private float blinkDuration = 0.5f;
    public float health;
    public float speed;
    public float contactDistance;
    public float projectileSpeed;

    //bool follow = false;

    private float attackCooldown = 0;
    public float attackCoolRate;
    public float attackRange;
    public int attackDamage;
    public bool isMelee;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = General.Instance.script_player.transform;
        originalColor = rend.color; // 현재 색상 저장
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
        if(transform.position.x - target.position.x < 0)
            rend.flipX = false;
        else
            rend.flipX = true;
        if(Vector2.Distance(transform.position, target.position) > contactDistance)
        {
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
        if(Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            if(attackCooldown <= 0)
            {
                if(isMelee)
                {
                    General.Instance.script_player.GetDamage(attackDamage);
                    attackCooldown = attackCoolRate;
                    animator.SetTrigger("isAttack");
                }
                else
                {
                    GameObject proj = Instantiate(Projectile);
                    Vector2 playerDirection = (target.position - transform.position).normalized;
                    Rigidbody2D projectileRb = proj.GetComponent<Rigidbody2D>();

                    proj.transform.position = transform.position;
                    projectileRb.velocity = playerDirection * projectileSpeed;
                    attackCooldown = attackCoolRate;
                    float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
                    proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Destroy(proj, 10);
                }
            }
        }
    }

    public void Attacked()
    {
        StartCoroutine(BlinkObject());
    }

    IEnumerator BlinkObject()
    {
        rend.color = Color.red; // 빨간색
        yield return new WaitForSeconds(blinkDuration);

        rend.color = originalColor;
    }
}
