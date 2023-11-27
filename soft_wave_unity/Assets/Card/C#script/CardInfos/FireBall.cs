using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall :MonoBehaviour, ICard
{
    public void OnAcquire() 
    {
        Debug.Log("Card 001 Acquired");
    }

    public void OnDraw()
    {
        Debug.Log("Card 001 Drew");
    }

    public void OnUse(AimInfo aimInfo)
    {
        GameObject caster = GameObject.Find("FireballCaster");
        caster.GetComponent<FireballCaster>().OnUse(aimInfo);
    }

    public void OnRemove()
    {
        Debug.Log("Card 001 Removed");
    }
}
