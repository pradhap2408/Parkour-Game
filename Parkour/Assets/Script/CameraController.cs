using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public Transform playerBody;
    public float distance;
    public float hight;
    private float rotationY;
    private float rotationX;
    private float minvertAngle = -20f;
    private float maxvertAngle = 45f;
    public float rotationSpeed = 2f;
    public bool invertX;
    private bool invertY;
    //float inverttX;
    float inverttY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()  {
        //inverttX=(invertX)? -1 : 1;
        inverttY= (invertY) ? -1 : 1;
        rotationX -= Input.GetAxis("Mouse Y")* rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minvertAngle, maxvertAngle);
        rotationY -= Input.GetAxis("Mouse X") * inverttY * rotationSpeed;

        var targetRoation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.position = playerBody.position + targetRoation * new Vector3(0, hight, distance);
        transform.rotation = targetRoation;

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
