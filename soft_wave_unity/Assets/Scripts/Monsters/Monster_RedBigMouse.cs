using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RedBigMouse : MonsterBehavior
{
    public Monster_RedBigMouse() : base()
    {
        health = 3f;
        speed = 3f;
        contactDistance = 1f;
        attackCoolRate = 1f;
        attackRange = 2f;
        attackDamage = 1;
        isMelee = true;
    }
}
