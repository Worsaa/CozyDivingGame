using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 2, -4); 
    public float rotationSpeed = 2f;
    public float playerRotationSpeed = 5f; 

    private float yaw = 0f; 
    private float pitch = 0f; 

    void Start()
    {
   

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

     
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        //pitch = Mathf.Clamp(pitch, -30f, 60f); // Slope

        // Rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Uppda camera
        transform.position = desiredPosition;
        transform.LookAt(target.position);

        // Rotate player
        Quaternion playerRotation = Quaternion.Euler(0, yaw, 0);
        target.rotation = Quaternion.Slerp(target.rotation, playerRotation, playerRotationSpeed * Time.deltaTime);
    }
}
