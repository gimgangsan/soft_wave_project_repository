using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCaster : HeadAndTail
{
    public override void Cast()
    {
        Debug.Log("fireball casted");
        base.Cast();
    }
}
