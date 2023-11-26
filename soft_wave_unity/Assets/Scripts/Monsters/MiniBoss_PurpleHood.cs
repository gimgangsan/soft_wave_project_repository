using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss_PurpleHood : MonsterBehavior
{
    public MiniBoss_PurpleHood() : base()
    {
        health = 10f;
        speed = 3f;
        contactDistance = 10f;
        attackCoolRate = 3f;
        attackRange = 15f;
        projectileSpeed = 15f;
        isMelee = false;
    }
}
