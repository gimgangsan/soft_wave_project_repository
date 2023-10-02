using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHPBar : HealthBar
{
    public TMP_Text TextUI;

    public override void WhenHPChanged(HPInfo info)
    {
        base.WhenHPChanged(info);
        TextUI.text = (info.HP + info.Amour).ToString() + " / " + info.MaxHP.ToString();
    }
}
