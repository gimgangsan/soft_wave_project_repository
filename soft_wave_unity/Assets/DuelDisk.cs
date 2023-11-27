using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelDisk : MonoBehaviour
{
    AimInfo CurrentAim;
    void Update()
    {
        CurrentAim = new AimInfo(General.Instance.script_player.transform.position, General.Instance.MousePos());
        transform.eulerAngles = new Vector3(0, 0, CurrentAim.CastAngle());
    }
}
