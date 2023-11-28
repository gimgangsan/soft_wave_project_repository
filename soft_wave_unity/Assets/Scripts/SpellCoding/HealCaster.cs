using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCaster : HeadAndTail
{
    public GameObject HealEffect;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        BasicActions ShooterHpScript = aimInfo.ShooterTransform.GetComponent<BasicActions>();
        (ShooterHpScript)?.GetHeal(10, 0);
        GameObject newEffect = Instantiate(HealEffect);
        newEffect.transform.SetParent(aimInfo.ShooterTransform);
        newEffect.transform.localPosition = Vector3.zero;
    }
}
