using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCaster : HeadAndTail
{
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        BasicActions ShooterHpScript = aimInfo.ShooterTransform.GetComponent<BasicActions>();
        (ShooterHpScript)?.GetHeal(10, 0);
    }
}
