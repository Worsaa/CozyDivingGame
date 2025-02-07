using UnityEngine;

public class DiverController : MonoBehaviour
{
    public float speed = 5f;
    public float acceleration = 2f;
    public float alignSpeed = 2f;
    public float deceleration = 1f;
    public float rotationSpeed = 5f;
    public Transform cameraTransform;
    public Animator animator;
    public float waterLevel = 0f;
    public float increasedGravity = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        bool freeLook = Input.GetKey(KeyCode.C);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = freeLook ? (transform.forward * v + transform.right * h) : (cameraTransform.forward * v + cameraTransform.right * h);
        if (inputDir.magnitude > 0.1f)
        {
            if (!freeLook)
            {
                moveDirection = inputDir;
            }
            float multiplier = Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f;
            currentSpeed += acceleration * multiplier * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
            if (!freeLook)
                moveDirection = Vector3.Lerp(moveDirection, 
                    cameraTransform.forward, alignSpeed * Time.deltaTime).normalized;
        }
        bool isMoving = currentSpeed > 0.1f;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        if (!freeLook)
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(cameraTransform.forward), rotationSpeed * Time.deltaTime));
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * currentSpeed;
        if (transform.position.y > waterLevel)
        {
            rb.velocity += Vector3.down * increasedGravity * Time.fixedDeltaTime;
        }
    }
}
