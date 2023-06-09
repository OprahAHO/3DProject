using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_MovementBase : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    private Vector3 inputDirection;
    private float currentSpeed;
    public float accelerationTime = 0.5f;
    private float velocityY = 0f;
    private float smoothTime = 0.1f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No Camera tagged 'MainCamera' is found in the scene!");
        }
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // ???????????????????
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        inputDirection = moveX * right + moveZ * forward;
    }

    private void FixedUpdate()
    {
        // ????????
        float targetSpeed = inputDirection.magnitude * moveSpeed;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocityY, accelerationTime);

        // ???????????
        rb.MovePosition(transform.position + inputDirection * currentSpeed * Time.fixedDeltaTime);
    }
}
