using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelFist : RelicType
{
    Rigidbody2D rigid;
    private void Awake()
    {
        this.Name = "steel fist";
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rigid.velocity = new Vector2(1, 0);
    }
    public override void GetEquiped()
    {
        General.Instance.script_player.onDamaged += this.onDamaged;
        base.GetEquiped();
    }

    public void onDamaged(int Amount)
    {
        if(Amount < 80)
        {
            General.Instance.script_player.HP += Amount;
        }
    }
}
