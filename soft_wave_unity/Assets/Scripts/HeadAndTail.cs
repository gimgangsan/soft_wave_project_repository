using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAndTail : MonoBehaviour, ISpellCaster
{
    public int Heads;
    public GameObject HeadObject;
    public int Tails;
    public GameObject TailObject;
    public ISpellCaster[] NextNodes;
    public bool IsExecuted { get; private set; }

    private void Awake()
    {
        GenerateHead();
        GenerateTail();
        NextNodes = new ISpellCaster[Heads];
    }

    public void GenerateHead()
    {
        for (int i = 0; i < Heads; i++)
        {
            GameObject newObject = Instantiate(HeadObject);
            newObject.transform.SetParent(transform, true);
            newObject.transform.localPosition = HeadLocalPosOf(i);
            ConnectorHead script = newObject.GetComponent<ConnectorHead>();
            script.Parent = this;
            script.Index = i;
        }
    }

    public void GenerateTail()
    {
        for (int i = 0; i < Tails; i++)
        {
            GameObject newObject = Instantiate(TailObject);
            newObject.transform.SetParent(transform, true);
            newObject.transform.localPosition = TailLocalPosOf(i);
            newObject.GetComponent<ConnectorTail>().Parent = gameObject;
        }
    }

    private Vector2 HeadLocalPosOf(int index)
    {
        Vector2 pos = new((float)(index + 1) / (float)(this.Heads + 1) - 0.5f, -0.5f);
        return pos;
    }

    private Vector2 TailLocalPosOf(int index)
    {
        Vector2 pos = new((float)(index + 1) / (float)(this.Tails + 1) - 0.5f, 0.5f);
        return pos;
    }

    public void Cast()
    {
        if (IsExecuted) return;
        IsExecuted = true;
        ReleaseSpell();
        for(int i = 0; i < Heads; i++)
        {
            (NextNodes[i])?.Cast();
        }
        IsExecuted = false;
    }

    public virtual void ReleaseSpell()
    {
        Debug.Log("ReleaseSpell func not overrided");
    }
}
