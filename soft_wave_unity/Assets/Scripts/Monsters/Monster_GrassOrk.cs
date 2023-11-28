using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_GrassOrk : MonsterBehavior
{
    public Monster_GrassOrk() : base()
    {
        health = 3f;
        speed = 2f;
        ContactDistance = 1f;
        MeleeRate = 1f;
        MeleeDamage = 10;
    }
}
