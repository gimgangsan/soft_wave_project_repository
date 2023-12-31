using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTail : ConnectorType
{
    public ConnectorHead CurrentHead;

    public override void WhenParentDragged()
    {
        transform.localPosition = GetInitialLocalPos();
        if (CurrentHead != null)
        {
            CurrentHead.SetHead(transform.position);
        }
    }

    public override Vector2 GetInitialLocalPos()
    {
        return new((float)(Index + 1) / (float)(2) - 0.5f, 0.55f);
    }

    public void ConnectHead(ConnectorHead HeadScript)
    {
        if (this.ParentScript == HeadScript.ParentScript) return;
        if (CurrentHead != null) return;
        HeadScript.SetHead(transform.position);
        HeadScript.CurrentTail = this;
        HeadScript.ParentScript.WhenCasted += this.ParentScript.OnUse;
        this.CurrentHead = HeadScript;
    }

    public void Disconnect()
    {
        CurrentHead.CurrentTail = null;
        CurrentHead.ParentScript.WhenCasted -= this.ParentScript.OnUse;
        this.CurrentHead = null;
    }
}
