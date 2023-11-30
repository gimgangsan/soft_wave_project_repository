using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction mousePositionAction;
    private Camera cam;
    private Vector2 mousePos;
    private Vector2 targetPos;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = new InputAction(binding: "<Mouse>/rightButton");
        moveAction.performed += ctx => clickToMove(ctx);
        mousePositionAction = new InputAction(binding: "Mouse/position");
        mousePositionAction.performed += ctx => getMousePosition(ctx);
        cam = Camera.main;
    }
    public void OnEnable()
    {
        moveAction.Enable();
        mousePositionAction.Enable();
    }
    public void getMousePosition(InputAction.CallbackContext con)
    {
        mousePos = con.ReadValue<Vector2>();
    }
    public void clickToMove(InputAction.CallbackContext con)
    {
        Vector2 direction;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        targetPos.x = point.x;
        targetPos.y = point.y;
        direction = (targetPos - rb.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }
    public void checkMove()
    {
        if (Mathf.Abs((rb.position.x + rb.position.y) - (targetPos.x + targetPos.y)) < 0.3f) rb.velocity = Vector2.zero;
    }
    void fixedUpdate()
    {
        checkMove();
    }
}
