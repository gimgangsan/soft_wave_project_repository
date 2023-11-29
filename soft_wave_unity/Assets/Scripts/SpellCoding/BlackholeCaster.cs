using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeCaster : HeadAndTail
{
    private float MaxRange = 6;
    public GameObject BlackholeObject;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        GameObject newBlackhole = Instantiate(BlackholeObject);
        newBlackhole.transform.position = aimInfo.ShooterTransform.position;
        newBlackhole.GetComponent<Blackhole>().SetDir(aimInfo.CastAngle());
    }
}
