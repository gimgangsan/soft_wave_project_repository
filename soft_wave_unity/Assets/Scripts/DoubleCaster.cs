using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCaster : HeadAndTail
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cast();
        }
    }
    public override void ReleaseSpell()
    {
        Debug.Log("doubleCaster casted");
    }
}
