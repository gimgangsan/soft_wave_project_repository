using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorType : MonoBehaviour
{
    public int Index;
    public HeadAndTail ParentScript;

    public void Initiate(int index, Transform ParentObject)
    {
        this.Index = index;
        transform.parent = ParentObject;
        this.ParentScript = ParentObject.GetComponent<HeadAndTail>();
        transform.localPosition = GetInitialLocalPos();
        if(ParentScript == null)
        {
            Debug.LogError("invalid parent object. HeadAndTail script is not found");
        }
    }
    public virtual void WhenParentDragged()
    {

    }

    public virtual Vector2 GetInitialLocalPos()
    {
        return new(0,0);
    }

    public Vector2 GetInitialPos()
    {
        return (Vector2)ParentScript.transform.position + GetInitialLocalPos() * ParentScript.transform.localScale.x;
    }
}
