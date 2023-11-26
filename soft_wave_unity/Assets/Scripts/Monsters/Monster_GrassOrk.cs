using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_GrassOrk : MonsterBehavior
{
    public Monster_GrassOrk() : base()
    {
        health = 3f;
        speed = 2f;
        contactDistance = 1f;
        attackCoolRate = 1f;
        attackRange = 2f;
        attackDamage = 10;
        isMelee = true;
    }
}
