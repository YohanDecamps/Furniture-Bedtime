using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerInputActions playerInputActions;
    CameraManager cameraManager;
    Rigidbody rb;

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump");
        }
    }

    public void Rotate(Vector2 direction) {
        Vector3 rotateDirection = cameraTransform.forward * direction.y + cameraTransform.right * direction.x;
        rotateDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(rotateDirection);
    }

    public void Move(Vector2 direction) {
        Vector3 moveDirection = cameraTransform.forward * direction.y + cameraTransform.right * direction.x;
        moveDirection.y = 0;
        rb.MovePosition(transform.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        rb = GetComponent<Rigidbody>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the player
        if (playerInputActions.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            Move(playerInputActions.Player.Movement.ReadValue<Vector2>());
            Rotate(playerInputActions.Player.Movement.ReadValue<Vector2>());
        }
    }

    void Update()
    {
        // Rotate the camera
        if (playerInputActions.Player.Look.ReadValue<Vector2>() != Vector2.zero)
        {
            cameraManager.SetAngle(playerInputActions.Player.Look.ReadValue<Vector2>());
        }
    }
}
