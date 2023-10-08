using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_001 :MonoBehaviour, ICard
{
    public void OnAcquire() 
    {
        Debug.Log("Card 001 Acquired");
    }

    public void OnDraw()
    {
        Debug.Log("Card 001 Drew");
    }

    public void OnUse()
    {
        Debug.Log("Card 001 Used");
    }

    public void OnRemove()
    {
        Debug.Log("Card 001 Removed");
    }
}
