using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject LeadFill;
    public GameObject FollowerFill;
    int Ratio = 1;
    int FollowerRatio = 1;
    float Width;

    private void Awake()
    {
        Width = GetComponent<RectTransform>().rect.width;
    }
}
