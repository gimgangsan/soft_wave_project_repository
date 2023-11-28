using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCaster : HeadAndTail
{
    public GameObject Fireball;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        GameObject newFireball = Instantiate(Fireball);
        newFireball.transform.position = aimInfo.ShooterTransform.position;
        newFireball.GetComponent<Fireball>().SetDir(aimInfo.CastAngle());
    }
}
