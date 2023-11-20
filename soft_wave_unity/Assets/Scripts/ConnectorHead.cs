using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorHead : MonoBehaviour
{
    private LineRenderer Line;
    private SpriteRenderer Sprite;
    public GameObject Parent;
    public ConnectorTail CurrentTail;
    public Vector2 InitialPos { get; set; }

    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (CurrentTail == null)
        {
            InitialPos = transform.position;
        }
        else
        {
            CurrentTail.Disconnect();
            CurrentTail = null;
        }
        Sprite.color = new Color(1, 0, 0, 0.6f);
        Line.SetPosition(0, InitialPos);
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
            SetHead(InitialPos);
        }
    }

    public void SetHead(Vector2 pos)
    {
        transform.position = pos;
        Line.SetPosition(1, pos);
    }
}
