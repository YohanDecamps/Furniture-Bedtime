using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Transform cameraTransform;
    Rigidbody rb;

    [Header("Moving")]
    [SerializeField] private float speed = 5f;

    [Header("Falling and Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float leapingForce = 5f;
    [SerializeField] private float fallingForce = 5f;
    [SerializeField] private LayerMask groundLayer;

    private float airTime = 0f;

    private PlayerManager playerManager;

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.OnMove += Move;
        playerManager.OnJump += Jump;
        
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Jump() {
        if (!IsGrounded()) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Rotate(Vector2 direction) {
        Vector3 rotateDirection = cameraTransform.forward * direction.y + cameraTransform.right * direction.x;
        rotateDirection.y = 0;
        if (direction != Vector2.zero) {
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }
    }

    private void Move(Vector2 direction) {
        if (!IsGrounded()) {
            rb.velocity = Vector3.zero;
            return;
        };
        Vector3 moveDirection = cameraTransform.forward * direction.y + cameraTransform.right * direction.x;
        moveDirection.y = 0;
        rb.velocity = moveDirection.normalized * speed;
        Rotate(direction);
    }

    void FixedUpdate()
    {
        if (!IsGrounded()) {
            Fall();
        } else {
            airTime = 0f;
        }
    }

    private bool IsGrounded() {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.01f, -Vector3.up, out hit, 1, groundLayer)) {
            return true;
        }
        return false;
    }

    private void Fall() {
        airTime += Time.deltaTime;
        // Add Leaping velocity to the player
        rb.AddForce(transform.forward * leapingForce, ForceMode.Force);
        // Add gravity to the player
        rb.AddForce(-transform.up * airTime * fallingForce, ForceMode.Force);
    }
}
