using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float topRotationLimit = 45f;
    [SerializeField] private float downRotationLimit = -45f;

    private float defaultPosition;
    private Vector3 velocity = Vector3.zero;

    public void SetTarget(Transform target) {
        targetTransform = target;
    }

    public void Awake()
    {
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void LateUpdate()
    {
        FollowTarget();
        HandleCameraCollision();
    }

    public void FollowTarget() {
        Vector3 targetPosition = Vector3.SmoothDamp(pivotTransform.position, targetTransform.position, ref velocity, smoothTime);
        pivotTransform.position = targetPosition;
    }

    public void SetAngle(Vector2 delta) {
        Vector3 angles = pivotTransform.eulerAngles;
        angles.x -= delta.y * rotationSpeed;
        angles.y += delta.x * rotationSpeed;
        Debug.Log(angles.x);
    
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

    public void HandleCameraCollision() {

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
