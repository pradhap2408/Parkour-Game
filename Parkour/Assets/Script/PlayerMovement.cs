using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;
    CameraController cameraController;
    Animator animator;
    public float moveMents=2f;
    Quaternion targetRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
   private void Update()
    {
      float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float  moveMents = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            var moveinput=(new Vector3(horizontalInput, 0, verticalInput)).normalized;
        var moveDir=cameraController.PlanarRotation * moveinput;

        if (moveMents > 0)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);
        }
        transform.rotation =Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
        animator.SetFloat("MoveMents", moveMents, 0.1f, Time.deltaTime);
    }
}