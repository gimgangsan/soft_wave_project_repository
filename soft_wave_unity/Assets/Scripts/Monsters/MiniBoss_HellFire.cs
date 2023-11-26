using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss_HellFire : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BasicActions>().GetDamage(50);
            Destroy(gameObject);
        }
    }
}
