using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserbeamCaster : HeadAndTail
{
    public GameObject LaserObject;

    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        GameObject newLaser = Instantiate(LaserObject);
        newLaser.transform.position = aimInfo.ShooterTransform.position;
        newLaser.transform.eulerAngles = new Vector3 (0, 0, aimInfo.CastAngle());
    }
}
