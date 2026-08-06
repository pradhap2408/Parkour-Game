using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float sideSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Slide")]
    public float slideTime = 1f;

    [Header("Wall Run")]
    public float wallRunSpeed = 10f;
    public float wallCheckDistance = 1f;
    public float wallJumpForce = 8f;
    public LayerMask wallLayer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Animator animator;

    private bool isGrounded;
    private bool isSliding;
    private bool isWallRunning;

    private CapsuleCollider capsule;
    private float originalHeight;
    private Vector3 originalCenter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;

        if (capsule != null)
        {
            originalHeight = capsule.height;
            originalCenter = capsule.center;
        }
    }

    void Update()
    {
        GroundCheck();

        Move();

        Jump();

        Slide();

        WallRun();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 vel = rb.linearVelocity;
        vel.z = forwardSpeed;
        vel.x = h * sideSpeed;

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        animator.SetFloat("Speed", forwardSpeed);
        animator.SetBool("IsGrounded", isGrounded);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isWallRunning)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            animator.SetTrigger("Jump");
        }
    }

    void Slide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isSliding && isGrounded)
            {
                StartCoroutine(SlideRoutine());
            }
        }
    }

    System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;

        animator.SetTrigger("Slide");

        if (capsule != null)
        {
            capsule.height *= 0.5f;
            capsule.center = new Vector3(
                capsule.center.x,
                capsule.center.y - 0.5f,
                capsule.center.z);
        }

        yield return new WaitForSeconds(slideTime);

        if (capsule != null)
        {
            capsule.height = originalHeight;
            capsule.center = originalCenter;
        }

        isSliding = false;
    }

    void WallRun()
    {
        bool leftWall = Physics.Raycast(transform.position, -transform.right, wallCheckDistance, wallLayer);
        bool rightWall = Physics.Raycast(transform.position, transform.right, wallCheckDistance, wallLayer);

        if ((leftWall || rightWall) && !isGrounded)
        {
            if (!isWallRunning)
            {
                StartWallRun();
            }

            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                0,
                wallRunSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 jumpDir = Vector3.up;

                if (leftWall)
                    jumpDir += transform.right;
                else
                    jumpDir -= transform.right;

                StopWallRun();

                rb.linearVelocity = Vector3.zero;
                rb.AddForce(jumpDir.normalized * wallJumpForce, ForceMode.Impulse);

                animator.SetTrigger("Jump");
            }
        }
        else
        {
            if (isWallRunning)
                StopWallRun();
        }
    }

    void StartWallRun()
    {
        isWallRunning = true;

        rb.useGravity = false;

        animator.SetBool("IsWallRunning", true);
    }

    void StopWallRun()
    {
        isWallRunning = false;

        rb.useGravity = true;

        animator.SetBool("IsWallRunning", false);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundDistance);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,
            transform.right * wallCheckDistance);

        Gizmos.DrawRay(transform.position,
            -transform.right * wallCheckDistance);
    }
}