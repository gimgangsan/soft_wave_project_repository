using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicType : MonoBehaviour, IRelic
{
    public string Name;

    public void Start()
    {
        gameObject.tag = General.Instance.tag_relic;
    }

    public virtual void GetEquiped()
    {
        Debug.Log("Relic name : " + Name + " is obtained");
    }
}
