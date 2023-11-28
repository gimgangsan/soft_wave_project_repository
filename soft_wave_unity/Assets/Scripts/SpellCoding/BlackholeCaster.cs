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
        Vector2 newBlackholePos = (Vector2.Distance(aimInfo.ShooterTransform.position, aimInfo.MousePos) > MaxRange) ?
                                (Vector2)aimInfo.ShooterTransform.position + aimInfo.NomarlizeInto(MaxRange) : aimInfo.MousePos;
        GameObject newBlackhole = Instantiate(BlackholeObject);
        newBlackhole.transform.position = newBlackholePos;
        newBlackhole.GetComponent<Blackhole>().SetDir(aimInfo.CastAngle());
    }
}
