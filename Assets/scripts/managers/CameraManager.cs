using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // the player most of the time
    private Transform pivotTransform;
    private Transform cameraTransform;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float topRotationLimit = 45f;
    [SerializeField] private float downRotationLimit = -45f;
    private PlayerManager playerManager;

    private float defaultPosition;
    private Vector3 velocity = Vector3.zero;

    public void SetTarget(Transform target) {
        targetTransform = target;
    }

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.OnLook += SetAngle;
        
        cameraTransform = Camera.main.transform;
        pivotTransform = cameraTransform.parent;
        defaultPosition = cameraTransform.localPosition.z;
    }

    void LateUpdate()
    {
        FollowTarget();
        HandleCameraCollision();
    }

    private void FollowTarget() {
        Vector3 targetPosition = Vector3.SmoothDamp(pivotTransform.position, targetTransform.position, ref velocity, smoothTime);
        pivotTransform.position = targetPosition;
    }

    private void SetAngle(Vector2 delta) {
        Vector3 angles = pivotTransform.eulerAngles;
        angles.x -= delta.y * rotationSpeed;
        angles.y += delta.x * rotationSpeed;
    
        // Clamping the camera rotation
        if (angles.x < (360 + downRotationLimit) && angles.x > topRotationLimit) {
            if (angles.x > 180) {
                angles.x = 360 + downRotationLimit;
            } else {
                angles.x = topRotationLimit;
            }
        } if (angles.x > topRotationLimit && angles.x < (360 + downRotationLimit)) {
            if (angles.x > 180) {
                angles.x = topRotationLimit;
            } else {
                angles.x = 360 + downRotationLimit;
            }
        }
        pivotTransform.localEulerAngles = angles;
        
    }

    private void HandleCameraCollision() {

        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - pivotTransform.position;
        direction.Normalize();
        if (Physics.SphereCast(pivotTransform.position, 0.4f, direction, out hit, Mathf.Abs(targetPosition))) {
            float distance = Vector3.Distance(pivotTransform.position, hit.point);
            targetPosition = -distance;
        } else {
            targetPosition = defaultPosition;
        }
        Vector3 newPosition = cameraTransform.localPosition;
        newPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime * 100);
        cameraTransform.localPosition = newPosition;
    }
}
