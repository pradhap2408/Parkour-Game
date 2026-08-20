using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 4f, -6f);
    public float smoothSpeed = 8f;
    public float mouseSensitivity = 100f;
    public float rotationSpeed = 10f;



    private void Start()
    {
        target = transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null) return;
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        transform.RotateAround(target.position, transform.right, -mouseY * rotationSpeed * Time.deltaTime);

    }
}