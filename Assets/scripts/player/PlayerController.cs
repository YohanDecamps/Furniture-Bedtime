using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Transform cameraTransform;
    Rigidbody rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.OnMove += Move;
        playerManager.OnJump += Jump;
        
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    public void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Jump");
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
        Rotate(direction);
    }
}
