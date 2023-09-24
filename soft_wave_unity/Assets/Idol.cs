using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idol : RelicType
{
    Rigidbody2D rigid;
    private void Start()
    {
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

    void onDamaged(int Amount) 
    {
        if(General.Instance.script_player.HP <= 0)
        {
            General.Instance.script_player.HP = 150;
            General.Instance.script_player.onDamaged -= this.onDamaged;
        }
    }
}
