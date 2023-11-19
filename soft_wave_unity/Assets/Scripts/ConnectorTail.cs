using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTail : MonoBehaviour
{
    public bool IsConnected;

    private void Awake()
    {
        IsConnected = false;
    }

    public void ConnectHead(ConnectorHead HeadScript)
    {
        IsConnected = true;
        HeadScript.IsConnected = true;
        HeadScript.SetHead(transform.position);
    }
}
