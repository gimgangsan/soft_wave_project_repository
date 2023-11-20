using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTail : MonoBehaviour
{
    public bool IsConnected;
    public GameObject Parent;

    private void Awake()
    {
        IsConnected = false;
    }

    public void ConnectHead(ConnectorHead HeadScript)
    {
        if (this.Parent == HeadScript.Parent) return;
        if (IsConnected) return;
        this.IsConnected = true;
        HeadScript.SetHead(transform.position);
        HeadScript.CurrentTail = this;
    }

    public void Disconnect()
    {
        this.IsConnected = false;
    }
}
