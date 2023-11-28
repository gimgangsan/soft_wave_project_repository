using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCaster : HeadAndTail
{
    public float MaxRange = 5;
    public GameObject FlashEffect;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        Vector2 newShooterPos = (Vector2.Distance(aimInfo.ShooterTransform.position, aimInfo.MousePos) > MaxRange) ?
                                (Vector2)aimInfo.ShooterTransform.position + aimInfo.NomarlizeInto(MaxRange) : aimInfo.MousePos;
        aimInfo.ShooterTransform.position = newShooterPos;
        Instantiate(FlashEffect).transform.position = newShooterPos;
    }
}
