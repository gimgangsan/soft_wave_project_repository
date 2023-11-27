using System.Collections.Generic;
using UnityEngine;

public class Card_002 : MonoBehaviour, ICard
{
    public void OnAcquire()
    {
        Debug.Log("Card 002 Acquired");
    }

    public void OnDraw()
    {
        Debug.Log("Card 002 Drew");
    }

    public void OnUse(AimInfo aimInfo)
    {
        Debug.Log("Card 002 Used");
    }

    public void OnRemove()
    {
        Debug.Log("Card 002 Removed");
    }
}
