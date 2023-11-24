using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAndTail : MonoBehaviour, ICard
{
    public int Heads;
    public GameObject HeadObject;
    public int Tails;
    public GameObject TailObject;
    public ICard[] NextNodes;
    public Action whenDragged;
    public bool IsExecuted { get; private set; }

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
        NextNodes = new ICard[Heads];
    }

    public void GenerateHead()
    {
        for (int i = 0; i < Heads; i++)
        {
            GameObject newObject = Instantiate(HeadObject);
            ConnectorType script = newObject.GetComponent<ConnectorType>();
            script.Initiate(i, transform);
            whenDragged += script.WhenParentDragged;
        }
    }

    public void GenerateTail()
    {
        for (int i = 0; i < Tails; i++)
        {
            GameObject newObject = Instantiate(TailObject);
            ConnectorType script = newObject.GetComponent<ConnectorType>();
            script.Initiate(i, transform);
            whenDragged += script.WhenParentDragged;
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

    public virtual void OnUse()
    {
        if (IsExecuted) return;
        IsExecuted = true;
        ReleaseSpell();
        for(int i = 0; i < Heads; i++)
        {
            (NextNodes[i])?.OnUse();
        }
        IsExecuted = false;
    }

    public virtual void ReleaseSpell()
    {
        Debug.Log("ReleaseSpell func not overrided");
    }

    
}
