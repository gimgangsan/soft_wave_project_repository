using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuduIdol : RelicType
{
    Rigidbody2D rigid;
    private void Awake()
    {
        this.Name = "budu idol";
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rigid.velocity = new Vector2(1, 0);
    }

    public override void GetEquiped()
    {
        General.Instance.script_player.onDead.AddListener(this.OnDead);
        base.GetEquiped();
    }

    public void OnDead()
    {
        General.Instance.script_player.HP = 150;
        General.Instance.script_player.onDead.RemoveListener(this.OnDead);
    }
}
