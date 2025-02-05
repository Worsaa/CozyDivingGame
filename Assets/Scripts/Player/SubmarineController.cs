using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float acceleration = 2f;
    public float maxSpeed = 10f;
    public float deceleration = 1f;
    public float rotationSpeed = 2f;
    public float buoyancy = 0.1f;
    public Transform exitPoint;
    public Animator animator;

    private bool isPlayerInside = false;
    private GameObject player;
    private ThirdPersonCamera cameraController;
    private Rigidbody rb;
    private float currentSpeed = 0f;
    private Vector3 moveDirection;

    void Start()
    {
        cameraController = FindObjectOfType<ThirdPersonCamera>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        if (isPlayerInside)
        {
            Quaternion cameraRotation = cameraController.GetCameraRotation();
            Vector3 forwardDirection = cameraRotation * Vector3.forward;

            float input = Input.GetAxis("Vertical");

            if (input != 0)
            {
                currentSpeed += input * acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
            }

            moveDirection = forwardDirection.normalized * currentSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardDirection), rotationSpeed * Time.deltaTime);

            bool isMoving = Mathf.Abs(currentSpeed) > 0.1f;
            bool isFast = Mathf.Abs(currentSpeed) > maxSpeed * 0.5f;

            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isRunning", isFast);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ExitSubmarine();
            }
        }
    }

    void FixedUpdate()
    {
        if (isPlayerInside)
        {
            rb.velocity = moveDirection;
            rb.velocity += Vector3.up * buoyancy * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isPlayerInside)
        {
            EnterSubmarine();
        }
    }

    void EnterSubmarine()
    {
        isPlayerInside = true;
        player.SetActive(false);
        cameraController.SetTarget(transform, true);
    }

    void ExitSubmarine()
    {
        isPlayerInside = false;
        player.SetActive(true);
        player.transform.position = exitPoint.position;
        cameraController.SetTarget(player.transform, false);
    }
}
