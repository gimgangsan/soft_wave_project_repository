using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicType : MonoBehaviour, IRelic
{
    public string Name = "no name";
    private void Awake()
    {
        gameObject.tag = General.Instance.tag_relic;
    }

    public virtual void GetEquiped()
    {
        Debug.Log("Relic name : " + Name + " is obtained");
    }
}
