using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event Action<Vector2> OnMove;
    public event Action<Vector2> OnLook;
    public event Action OnJump;

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            OnJump?.Invoke();
        }
    }

    void Awake()
    {
        // Initialize the player input actions
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        // Add the jump action
        playerInputActions.Player.Jump.performed += Jump;
    }

    void FixedUpdate()
    {
        if (playerInputActions.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            OnMove?.Invoke(playerInputActions.Player.Movement.ReadValue<Vector2>());
        } else {
            OnMove?.Invoke(Vector2.zero);
        }
    }
    void Update()
    {
        if (playerInputActions.Player.Look.ReadValue<Vector2>() != Vector2.zero)
        {
            OnLook?.Invoke(playerInputActions.Player.Look.ReadValue<Vector2>());
        }
    }
}
