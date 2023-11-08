using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Testing Fireball
public class TestFireBall : MonoBehaviour
{
    public GameObject fireballPrefab;
    Transform playerTransform;
    Vector3 mousePos, transPos, targetPos;
    Vector3 playerPos;
    public float ballSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            playerPos = playerTransform.position;
            CalTargetPos();
            
            GameObject fireball = Instantiate(fireballPrefab, playerPos, Quaternion.identity);
            Vector2 direction = (Vector2)targetPos - (Vector2)fireball.transform.position;
            direction.Normalize();
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            rb.velocity = direction * ballSpeed;
            Destroy(fireball, 3);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void CalTargetPos() //마우스 좌표
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
    }
}
