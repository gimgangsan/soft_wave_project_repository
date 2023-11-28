using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RedBigMouse : MonsterBehavior
{
    public Monster_RedBigMouse() : base()
    {
        health = 3f;
        speed = 3f;
        ContactDistance = 1f;
        MeleeRate = 1f;
        MeleeDamage = 1;
    }
}
