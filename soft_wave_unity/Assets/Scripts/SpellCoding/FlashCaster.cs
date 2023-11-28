using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCaster : HeadAndTail
{
    public float MaxRange = 5;
    public GameObject FlashEffect;
    float maxY = 4;
    float minY = -37.5f;
    float maxX = 46.5f;
    float minX = -46.5f;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        Instantiate(FlashEffect).transform.position = aimInfo.ShooterTransform.position;
        Vector2 newShooterPos = (Vector2.Distance(aimInfo.ShooterTransform.position, aimInfo.MousePos) > MaxRange) ?
                                (Vector2)aimInfo.ShooterTransform.position + aimInfo.NomarlizeInto(MaxRange) : aimInfo.MousePos;
        newShooterPos.x = Mathf.Min(Mathf.Max(minX, newShooterPos.x), maxX);
        newShooterPos.y = Mathf.Min(Mathf.Max(minY, newShooterPos.y), maxY);
        aimInfo.ShooterTransform.position = newShooterPos;
        Instantiate(FlashEffect).transform.position = newShooterPos;
    }
}
