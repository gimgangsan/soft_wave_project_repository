using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public RectTransform LeadFill;
    public RectTransform FollowerFill;
    public TMP_Text TextUI;
    float Ratio = 1;
    float FollowerRatio = 1;
    float Width;

    private void Awake()
    {
        Width = GetComponent<RectTransform>().rect.width;
    }

    private void Update()
    {
        if(Ratio < FollowerRatio)
        {
            FollowerRatio -= 0.0005f;
            float newWidth2 = Mathf.RoundToInt(Width * FollowerRatio);
            FollowerFill.offsetMax = new Vector2(-(Width - newWidth2), 0);
        }
    }

    public void WhenHPChanged(HPInfo info)
    {
        this.Ratio = info.GetHPRatio();
        float newWidth = Mathf.RoundToInt(Width * Ratio);
        LeadFill.offsetMax = new Vector2(-(Width - newWidth), 0);
        TextUI.text = info.HP.ToString() + " / " + info.MaxHP.ToString();
    }
}
