using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCaster : HeadAndTail
{
    private void Awake()
    {
        ManaCost = 1;
        GenerateHead();
    }
}
