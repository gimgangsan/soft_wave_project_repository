using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class DoubleCaster : HeadAndTail
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AimInfo aimInfo;
            aimInfo.ShooterPos = new Vector2(0, 0);
            aimInfo.MousePos = new Vector2(1, 1);
            OnUse(aimInfo);
        }
    }
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        Debug.Log("doubleCaster casted");
    }
}
