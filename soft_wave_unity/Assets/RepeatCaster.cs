using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatCaster : HeadAndTail
{
    public int RepeatCount;

    public override void OnUse()
    {
        if (IsExecuted) return;
        IsExecuted = true;
        for(int i = 0; i < RepeatCount; i++)
        {
            ReleaseSpell();
            WhenCasted?.Invoke();
        }
        IsExecuted = false;
    }
}
