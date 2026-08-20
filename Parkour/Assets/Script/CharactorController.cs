using UnityEngine;

public class CharactorController : MonoBehaviour
{
  public CharacterController controller;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();


      
    }

    // Update is called once per frame
    void Update()
    {
    

        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * speed *Time.deltaTime;
    }
}
