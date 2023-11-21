using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCaster : HeadAndTail
{
    public override void ReleaseSpell()
    {
        Debug.Log("fireball casted");
    }
}
