using System;
using UnityEngine;
using UnityEngine.InputSystem;

//플레이어의 이동을 담당
public class Player_Move : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private float moveSpeed = 0f;

    public event Action<float> Moving;

    private void Awake()
    {
        Awake_BindInput();
        Awake_Init();
    }

    private void FixedUpdate()
    {
        FixedUpdate_ApplyMove();
    }

    //=================================
    private void Awake_Init()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake_BindInput()
    {
        //씬 내 플레이어 인풋-액션맵 찾기
        PlayerInput playerInput = GameObject.Find("InputSystem").GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("Player");

        //이동 바인딩
        InputAction moveAction = actionMap.FindAction("Move");
        moveAction.performed += context =>
        {
            moveInput = context.ReadValue<Vector2>();
        };
        moveAction.canceled += context => moveInput = Vector2.zero;
    }

    private void FixedUpdate_ApplyMove()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        Moving?.Invoke(moveInput.x);
    }
    public void GetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
}
