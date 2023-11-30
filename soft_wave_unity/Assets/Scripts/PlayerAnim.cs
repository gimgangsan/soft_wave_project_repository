using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private enum State { idle, move, attack, damaged }
    State state = State.idle;
    [SerializeField] private Animator playerAnim;
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }
    public void switchAnim()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            state = State.idle;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            state = State.move;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            state = State.attack;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            state = State.damaged;
        }
        playerAnim.SetInteger("state", (int)state);
    }
    // Update is called once per frame
    void Update()
    {
        switchAnim();
    }
}
