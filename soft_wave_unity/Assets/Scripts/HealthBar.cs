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
            float FollowerWidth = Mathf.RoundToInt(Width * FollowerRatio);
            FollowerFill.offsetMax = new Vector2(-(Width - FollowerWidth), 0);
        }
        else
        {
            FollowerRatio = Ratio;
            float FollowerWidth = Mathf.RoundToInt(Width * FollowerRatio);
            FollowerFill.offsetMax = new Vector2(-(Width - FollowerWidth), 0);
        }
    }

    public void WhenHPChanged(HPInfo info)
    {
        this.Ratio = info.GetHPRatio();
        float LeadWidth = Mathf.RoundToInt(Width * Ratio);
        LeadFill.offsetMax = new Vector2(-(Width - LeadWidth), 0);

        float AmourWidth = Mathf.RoundToInt(Width * info.GetAmourRatio());
        AmourFill.offsetMax = new Vector2(-(Width - LeadWidth - AmourWidth), 0);
        AmourFill.offsetMin = new Vector2(LeadWidth, 0);

        TextUI.text = (info.HP + info.Amour).ToString() + " / " + info.MaxHP.ToString();
    }
}
