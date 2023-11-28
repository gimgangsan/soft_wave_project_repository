using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatCaster : HeadAndTail
{
    public int RepeatCount;

    public override void OnUse(AimInfo aimInfo)
    {
        if (IsExecuted) return;
        IsExecuted = true;
        ReleaseSpell(aimInfo);
        StartCoroutine(RepeatCast(aimInfo));
        IsExecuted = false;
    }

    private IEnumerator RepeatCast(AimInfo aimInfo)
    {
        WhenCasted?.Invoke(aimInfo);
        yield return new WaitForSeconds(0.2f);
        WhenCasted?.Invoke(aimInfo);
        yield break;
    }
}
