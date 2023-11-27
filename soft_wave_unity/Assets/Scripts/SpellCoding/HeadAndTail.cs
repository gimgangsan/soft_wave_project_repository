using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class HeadAndTail : MonoBehaviour, ICard
{
    public int Heads;
    public GameObject HeadObject;
    public int Tails;
    public GameObject TailObject;
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
        for (int i = 0; i < Tails; i++)
        {
            GameObject newObject = Instantiate(TailObject);
            ConnectorType script = newObject.GetComponent<ConnectorType>();
            script.Initiate(i, transform);
            WhenDragged += script.WhenParentDragged;
        }
    }

    public virtual void OnAcquire()
    {

    }

    public virtual void OnDraw()
    {

    }

    public virtual void OnRemove()
    {

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
