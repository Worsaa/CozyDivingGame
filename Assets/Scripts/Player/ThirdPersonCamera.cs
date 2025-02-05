using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 normalOffset = new Vector3(0, 2, -4);
    public Vector3 submarineOffset = new Vector3(0, 4, -8);
    public float rotationSpeed = 2f;

    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 currentOffset;

    void Start()
    {
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }
        currentOffset = normalOffset;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * currentOffset;

        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }

    public void SetTarget(Transform newTarget, bool isSubmarine)
    {
        target = newTarget;
        currentOffset = isSubmarine ? submarineOffset : normalOffset;
    }

    public Quaternion GetCameraRotation()
    {
        return Quaternion.Euler(pitch, yaw, 0);
    }
}
