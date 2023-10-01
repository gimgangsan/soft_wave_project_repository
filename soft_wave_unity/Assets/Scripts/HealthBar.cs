using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public RectTransform LeadFill;
    public RectTransform FollowerFill;
    float Ratio = 1;
    float FollowerRatio = 1;
    float Width;

    private void Awake()
    {
        Width = GetComponent<RectTransform>().rect.width;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Ratio -= 0.1f;
        }

        if(Ratio < FollowerRatio)
        {
            FollowerRatio -= 0.0005f;
            float newWidth2 = Mathf.RoundToInt(Width * FollowerRatio);
            FollowerFill.offsetMax = new Vector2(-(Width - newWidth2), 0);
        }
    }

    public void WhenHPChanged(float HP_Ratio)
    {
        this.Ratio = HP_Ratio;
        float newWidth = Mathf.RoundToInt(Width * Ratio);
        LeadFill.offsetMax = new Vector2(-(Width - newWidth), 0);
    }

    //public void WhenHarmed(DamageInfo info)
    //{
    //    float newWidth = Mathf.RoundToInt(Width * Ratio);
    //    LeadFill.offsetMax = new Vector2(-(Width - newWidth), 0);
    //}

    //public void WhenHealed(HealInfo info)
    //{
    //    float newWidth = Mathf.RoundToInt(Width * Ratio);
    //    LeadFill.offsetMax = new Vector2(-(Width - newWidth), 0);
    //    FollowerFill.offsetMax = new Vector2(-(Width - newWidth), 0);
    //}
}
