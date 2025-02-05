using UnityEngine;

public class DiverController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public Transform cameraTransform;
    public Animator animator;

    public float waterLevel = 0f;
    public float increasedGravity = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Quaternion targetRotation = cameraTransform.rotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));

        moveDirection = rb.rotation * new Vector3(h, 0, v);
        moveDirection = moveDirection.normalized;

        bool isMoving = moveDirection.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
    }

    void FixedUpdate()
    {
        Vector3 movement = moveDirection * (Input.GetKey(KeyCode.LeftShift) ? speed * 1.5f : speed);
        rb.velocity = movement;

        if (transform.position.y > waterLevel)
        {
            rb.velocity = movement * 0.5f;
            rb.velocity += Vector3.down * increasedGravity * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity *= 0.98f;
        }
    }
}
