using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorHead : MonoBehaviour
{
    private LineRenderer Line;
    private SpriteRenderer Sprite;
    public Vector2 InitialPos { get; set; }
    public bool IsConnected { get; set; }

    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
        Sprite = GetComponent<SpriteRenderer>();
        InitialPos = transform.position;
        IsConnected = false;
    }

    private void OnMouseDown()
    {
        IsConnected = false;
        Sprite.color = new Color(1, 0, 0, 0.6f);
        Line.SetPosition(0, InitialPos);
    }

    private void OnMouseDrag()
    {
        SetHead(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        Sprite.color = new Color(1, 1, 1, 1);
        Vector2 BoxSize = new(transform.localScale.x, transform.localScale.y);
        Collider2D hit = Physics2D.OverlapBox(transform.position, BoxSize,0,LayerMask.GetMask("ConnectorTail"));
        if (hit != null )
        {
            hit.GetComponent<ConnectorTail>().ConnectHead(this);
        }
        if (IsConnected == false)
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
