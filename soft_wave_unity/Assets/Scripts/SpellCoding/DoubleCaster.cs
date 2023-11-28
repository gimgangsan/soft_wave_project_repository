using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class DoubleCaster : HeadAndTail
{
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        Debug.Log("doubleCaster casted");
    }
}
