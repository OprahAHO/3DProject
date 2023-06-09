using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xAxisClamp = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        // ????????
        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = .0f;
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
        }

        transform.Rotate(-mouseY, 0, 0);
        playerBody.Rotate(0, mouseX, 0);
    }
}

