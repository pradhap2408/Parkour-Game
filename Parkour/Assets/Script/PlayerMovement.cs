using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 10f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Slide")]
    public float normalHeight = 2f;
    public float slideHeight = 1f;
    public float slideTime = 0.8f;

    [Header("Wall Run")]
    public LayerMask wallLayer;
    public float wallRunGravity = -2f;

    CharacterController controller;
    Animator animator;

    Vector3 velocity;

    int currentLane = 1; // 0 Left, 1 Center, 2 Right

    bool isSliding;
    bool isWallRunning;
    float slideTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        controller.height = normalHeight;
        controller.center = new Vector3(0, normalHeight / 2f, 0);
    }

    void Update()
    {
        LaneInput();
        Move();
        Jump();
        Slide();
        WallRun();
    }

    void LaneInput()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
            currentLane--;

        if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
            currentLane++;
#endif
    }

    void Move()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += (isWallRunning ? wallRunGravity : gravity) * Time.deltaTime;

        float targetX = (currentLane - 1) * laneDistance;

        float moveX = Mathf.Lerp(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime);

        Vector3 move = new Vector3(
            moveX - transform.position.x,
            velocity.y * Time.deltaTime,
            forwardSpeed * Time.deltaTime);

        controller.Move(move);

        if (animator)
            animator.SetFloat("Speed", forwardSpeed);
    }

    void Jump()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
#endif
        {
            if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (animator)
                    animator.SetTrigger("Jump");
            }
        }
    }

    void Slide()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
#endif
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
            {
                isSliding = true;
                slideTimer = slideTime;

                controller.height = slideHeight;
                controller.center = new Vector3(0, slideHeight / 2f, 0);

                if (animator)
                    animator.SetBool("Slide", true);
            }
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0)
            {
                isSliding = false;

                controller.height = normalHeight;
                controller.center = new Vector3(0, normalHeight / 2f, 0);

                if (animator)
                    animator.SetBool("Slide", false);
            }
        }
    }

    void WallRun()
    {
        if (controller.isGrounded)
        {
            isWallRunning = false;

            if (animator)
                animator.SetBool("WallRun", false);

            return;
        }

        bool left = Physics.Raycast(transform.position, -transform.right, 1f, wallLayer);
        bool right = Physics.Raycast(transform.position, transform.right, 1f, wallLayer);

        isWallRunning = left || right;

        if (animator)
            animator.SetBool("WallRun", isWallRunning);
    }
}