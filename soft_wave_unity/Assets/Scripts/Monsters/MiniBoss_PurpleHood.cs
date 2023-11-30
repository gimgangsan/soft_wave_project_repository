using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MiniBoss_PurpleHood : MonsterBehavior
{
    public GameObject Projectile;
    protected float FireCooldown = 0;
    public float FireRate = 5;
    public float FireDistance = 10;

    public MiniBoss_PurpleHood() : base()
    {
        health = 10f;
        speed = 3f;
        ContactDistance = 2f;
        MeleeRate = 3f;
        MeleeDamage = 10;
    }

    private void Update()
    {
        if (General.Instance.isPause) return;
        MeleeCooldown -= Time.deltaTime;
        FireCooldown -= Time.deltaTime;
        FollowTarget();
        ShootFire();
    }

    public void ShootFire()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if(FireCooldown <= 0 && distance <= FireDistance)
        {
            GameObject proj = Instantiate(Projectile);
            Vector2 playerDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            proj.transform.position = transform.position;
            proj.GetComponent<MiniBoss_HellFire>().SetDir(angle);
            FireCooldown = FireRate;
        }
    }
}
