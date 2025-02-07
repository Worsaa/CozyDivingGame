using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float acceleration = 2f;
    public float maxSpeed = 10f;
    public float deceleration = 1f;
    public float rotationSpeed = 2f;
    public float buoyancy = 0.1f;
    public float waterLevel = 0f;
    public float increasedGravity = 5f;
    public Transform exitPoint;
    public Animator animator;
    public ParticleSystem[] engineEffects;
    public ParticleSystem[] boostEffects;
    public ParticleSystem sandEffect;
    public ParticleSystem softCrashEffect;
    public ParticleSystem hardCrashEffect;
    public float sandEffectDistance = 3f;
    public LayerMask groundLayer;
    public LayerMask sandLayer;

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
        moveDirection = transform.forward;
    }

    void Update()
    {
        if (isPlayerInside)
        {
            bool freeLook = Input.GetKey(KeyCode.C);
            Quaternion camRot = cameraController.GetCameraRotation();
            Vector3 forwardDirection = freeLook ? transform.forward : (camRot * Vector3.forward);
            float input = Input.GetAxis("Vertical");
            bool isBoosting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (input != 0)
            {
                float speedModifier = isBoosting ? 2f : 1f;
                currentSpeed += input * acceleration * speedModifier * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
            }

            if (!freeLook)
                moveDirection = forwardDirection.normalized * currentSpeed;
            else
                moveDirection = transform.forward * currentSpeed;

            if (!freeLook)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardDirection), rotationSpeed * Time.deltaTime);

            bool isMoving = Mathf.Abs(currentSpeed) > 0.1f;
            bool isFast = Mathf.Abs(currentSpeed) > maxSpeed * 0.5f;
            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isRunning", isFast);

            if (input > 0)
            {
                foreach (ParticleSystem ps in engineEffects)
                    if (!ps.isPlaying) ps.Play();
                if (isBoosting)
                {
                    foreach (ParticleSystem ps in boostEffects)
                        if (!ps.isPlaying) ps.Play();
                }
                else
                {
                    foreach (ParticleSystem ps in boostEffects)
                        if (ps.isPlaying) ps.Stop();
                }
            }
            else
            {
                foreach (ParticleSystem ps in engineEffects)
                    if (ps.isPlaying) ps.Stop();
                foreach (ParticleSystem ps in boostEffects)
                    if (ps.isPlaying) ps.Stop();
            }

            CheckSandEffect(isMoving);

            if (Input.GetKeyDown(KeyCode.Q))
                ExitSubmarine();
        }
    }

    void FixedUpdate()
    {
        if (isPlayerInside)
        {
            if (transform.position.y > waterLevel)
            {
                rb.linearVelocity = moveDirection * 0.5f;
                rb.linearVelocity += Vector3.down * increasedGravity * Time.fixedDeltaTime;
            }
            else
            {
                rb.linearVelocity = moveDirection;
                rb.linearVelocity += Vector3.up * buoyancy * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (transform.position.y > waterLevel)
            {
                rb.linearVelocity += Vector3.down * increasedGravity * Time.fixedDeltaTime;
            }
        }
    }

    void CheckSandEffect(bool isMoving)
    {
        if (isMoving)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, sandEffectDistance, groundLayer))
            {
                if (!sandEffect.isPlaying)
                    sandEffect.Play();
                return;
            }
        }
        if (sandEffect.isPlaying)
            sandEffect.Stop();
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Stone"))
        {
            if (hardCrashEffect != null)
            {
                ParticleSystem effect = Instantiate(hardCrashEffect, contact.point, Quaternion.LookRotation(contact.normal));
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constant);
            }
        }
        else if (((1 << collision.gameObject.layer) & sandLayer.value) != 0)
        {
            if (softCrashEffect != null)
            {
                ParticleSystem effect = Instantiate(softCrashEffect, contact.point, Quaternion.LookRotation(contact.normal));
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constant);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            player = other.gameObject;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isPlayerInside)
            EnterSubmarine();
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
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        foreach (ParticleSystem ps in engineEffects)
            if (ps.isPlaying) ps.Stop();
        foreach (ParticleSystem ps in boostEffects)
            if (ps.isPlaying) ps.Stop();
    }
}
