using System.Collections.Generic;
using UnityEngine;

public class Card_003 : MonoBehaviour, ICard
{
    public void OnAcquire()
    {
        Debug.Log("Card 003 Acquired");
    }

    public void OnDraw()
    {
        Debug.Log("Card 003 Drew");
    }

    public void OnUse(AimInfo aimInfo)
    {
        Debug.Log("Card 003 Used");
    }

    public void OnRemove()
    {
        Debug.Log("Card 003 Removed");
    }
}
