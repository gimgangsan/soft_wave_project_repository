using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTail : ConnectorType
{
    public bool IsConnected;

    private void Awake()
    {
        IsConnected = false;
    }

    public override void WhenParentDragged()
    {
        transform.localPosition = GetInitialLocalPos();
    }

    public override Vector2 GetInitialLocalPos()
    {
        return new((float)(Index + 1) / (float)(ParentScript.Heads + 1) - 0.5f, 0.5f);
    }

    public void ConnectHead(ConnectorHead HeadScript)
    {
        if (this.ParentScript == HeadScript.ParentScript) return;
        if (IsConnected) return;
        this.IsConnected = true;
        HeadScript.SetHead(transform.position);
        HeadScript.CurrentTail = this;
        HeadScript.ParentScript.NextNodes[HeadScript.Index] = this.ParentScript.GetComponent<ICard>();
    }

    public void Disconnect()
    {
        this.IsConnected = false;
    }
}
