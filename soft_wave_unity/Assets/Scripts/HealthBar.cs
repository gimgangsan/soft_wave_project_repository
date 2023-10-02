using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public RectTransform LeadFill;
    public RectTransform FollowerFill;
    public RectTransform AmourFill;
    protected int HP = 0;
    protected int MaxHP = 9999999;
    protected float Trace = 0;
    protected float Width;

    public void Awake()
    {
        Width = GetComponent<RectTransform>().rect.width;
    }

    public void Update()
    {
        // 초록 체력바 + 회색 체력바가 빨강 체력바보다 짧을 때
        if(HP < Trace)
        {
            Trace -= 0.3f;
        }
        else
        {
            Trace = HP;
        }
        float FollowerWidth = Width * (Trace / MaxHP);
        FollowerFill.offsetMax = new Vector2(-(Width - FollowerWidth), 0);
    }

    public virtual void WhenHPChanged(HPInfo info)
    {
        this.HP = info.HP;
        this.MaxHP = info.MaxHP;
        
        float LeadWidth = Width * info.GetHPRatio();
        LeadFill.offsetMax = new Vector2(-(Width - LeadWidth), 0);

        float AmourWidth = Width * info.GetAmourRatio();
        AmourFill.offsetMax = new Vector2(-(Width - LeadWidth - AmourWidth), 0);
        AmourFill.offsetMin = new Vector2(LeadWidth, 0);
    }
}
