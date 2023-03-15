using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravityScale = 1f;
    public float mouseSensitivity = 10000000000000f;

    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float mouseX, mouseY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Handle player jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            Debug.Log("j");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // Handle player gravity
        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Handle camera rotation
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
}
