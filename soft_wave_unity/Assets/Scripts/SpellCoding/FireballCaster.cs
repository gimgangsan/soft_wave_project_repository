using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCaster : HeadAndTail
{
    public GameObject Fireball;
    public override void ReleaseSpell(AimInfo aimInfo)
    {
        Debug.Log("fireball casted");
        GameObject newFireball = Instantiate(Fireball);
        newFireball.transform.position = General.Instance.script_player.transform.position;
        newFireball.GetComponent<Fireball>().SetDir(aimInfo.CastAngle());
    }
}
