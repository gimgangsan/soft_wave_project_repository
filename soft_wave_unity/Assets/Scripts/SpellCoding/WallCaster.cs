using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCaster : HeadAndTail
{
    public float MaxRange = 4;
    public GameObject WallObject;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        base.ReleaseSpell(aimInfo);
        GameObject newWall = Instantiate(WallObject);
        if(Vector2.Distance(aimInfo.ShooterTransform.position, aimInfo.MousePos) > MaxRange)
        {
            newWall.transform.position = (Vector2)aimInfo.ShooterTransform.position + aimInfo.NomarlizeInto(MaxRange);
        }
        else
        {
            newWall.transform.position = aimInfo.MousePos;
        }
        newWall.transform.eulerAngles = new Vector3(0,0, aimInfo.CastAngle()+90);
    }
}
