using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCaster : HeadAndTail
{
    public float MaxRange = 5;
    public GameObject WallObject;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        Debug.Log("WallCaster casted");
        GameObject newWall = Instantiate(WallObject);
        if(Vector2.Distance(aimInfo.ShooterPos, aimInfo.MousePos) > 5)
        {
            newWall.transform.position = aimInfo.ShooterPos + aimInfo.NomarlizeInto(MaxRange);
        }
        else
        {
            newWall.transform.position = aimInfo.MousePos;
        }
        newWall.transform.eulerAngles = new Vector3(0,0, aimInfo.CastAngle()+90);
    }
}
