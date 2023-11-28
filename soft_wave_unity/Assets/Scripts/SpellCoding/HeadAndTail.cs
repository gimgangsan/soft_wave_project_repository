using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class HeadAndTail : MonoBehaviour, ICard
{
    public int Heads;
    public GameObject HeadObject;
    public GameObject TailObject;
    private ConnectorTail MyTail;
    public Action<AimInfo> WhenCasted;
    public Action WhenDragged;
    public bool IsExecuted { get; set; }

    public string Name { get; set; }
    public Sprite Sprite { get; set; }
    public string Describtion { get; set; }
    public int ManaCost { get; set; }

    private void Awake()
    {
        Name = gameObject.name;
        Sprite = null;
        Describtion = "empty describtion";
        ManaCost = 1;
        GenerateHead();
        GenerateTail();
    }

    public void GenerateHead()
    {
        for (int i = 0; i < Heads; i++)
        {
            GameObject newObject = Instantiate(HeadObject);
            ConnectorType script = newObject.GetComponent<ConnectorType>();
            script.Initiate(i, transform);
            WhenDragged += script.WhenParentDragged;
        }
    }

    public void GenerateTail()
    {
        GameObject newObject = Instantiate(TailObject);
        ConnectorTail script = newObject.GetComponent<ConnectorTail>();
        script.Initiate(0, transform);
        WhenDragged += script.WhenParentDragged;
        MyTail = script;
    }

    public virtual void OnAcquire()
    {

    }

    public virtual void OnDraw()
    {

    }

    public virtual void OnRemove()
    {
        if(MyTail.CurrentHead != null)
        {
            MyTail.CurrentHead.SetHead(MyTail.CurrentHead.GetInitialPos());
            MyTail.CurrentHead.ParentScript.WhenCasted -= this.OnUse;
        }
        Destroy(gameObject);
    }

    public virtual void OnUse(AimInfo aimInfo)
    {
        if (IsExecuted) return;
        IsExecuted = true;
        ReleaseSpell(aimInfo);
        WhenCasted?.Invoke(aimInfo);
        IsExecuted = false;
    }

    public virtual void ReleaseSpell(AimInfo aimInfo)
    {
        Debug.Log("ReleaseSpell func not overrided");
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;
        WhenDragged?.Invoke();
    }
}
