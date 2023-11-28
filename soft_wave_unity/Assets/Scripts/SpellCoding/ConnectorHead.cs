using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorHead : ConnectorType
{
    private LineRenderer Line;
    private SpriteRenderer Sprite;
    public ConnectorTail CurrentTail;

    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    public override void WhenParentDragged()
    {
        Vector2 InitialPos = GetInitialPos();
        Line.SetPosition(0, InitialPos);
        if (CurrentTail == null)
        {
            Line.SetPosition(1, InitialPos);
            transform.position = InitialPos;
        }
        else
        {
            transform.position = CurrentTail.transform.position;
        }
    }

    public override Vector2 GetInitialLocalPos()
    {
        return new((float)(Index + 1) / (float)(ParentScript.Heads + 1) - 0.5f, -0.55f);
    }

    private void OnMouseDown()
    {
        if(CurrentTail != null)
        {
            CurrentTail.Disconnect();
            CurrentTail = null;
        }
        Sprite.color = new Color(1, 0, 0, 0.6f);
        Line.SetPosition(0, GetInitialPos());
    }

    private void OnMouseDrag()
    {
        SetHead(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        Sprite.color = new Color(0, 1, 0, 1);
        Vector2 BoxSize = new(transform.localScale.x, transform.localScale.y);
        Collider2D hit = Physics2D.OverlapBox(transform.position, BoxSize,0,LayerMask.GetMask("ConnectorTail"));
        if (hit != null)
        {
            hit.GetComponent<ConnectorTail>().ConnectHead(this);
        }
        if (CurrentTail == null)
        {
            SetHead(GetInitialPos());
        }
    }

    public void SetHead(Vector2 pos)
    {
        transform.position = pos;
        Line.SetPosition(1, pos);
    }
}
