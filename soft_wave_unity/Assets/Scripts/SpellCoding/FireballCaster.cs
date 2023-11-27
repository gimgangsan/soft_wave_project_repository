using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCaster : HeadAndTail
{
    public GameObject Fireball;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        Debug.Log("fireball casted");
        Instantiate(Fireball).GetComponent<Fireball>().SetDir(aimInfo.CastAngle());
    }
}
