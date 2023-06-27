using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunningAdv : MonoBehaviour
{
    [Header("Wallrunning")]
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;
    private Vector3 wallNormal;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;

    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityGounterForce;

    [Header("Reference")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovementAdv pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdv>();
        
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        
    }
    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();

    }

    private void CheckForWall()
    {
        wallRight = (Physics.Raycast(transform.position, orientation.right, out RaycastHit rightWallHit, wallCheckDistance) && rightWallHit.collider.GetComponent<WallComponent>() != null);
        wallLeft = (Physics.Raycast(transform.position, -orientation.right, out RaycastHit leftWallHit, wallCheckDistance) && leftWallHit.collider.GetComponent<WallComponent>() != null);

    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight) || !pm.grounded;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.GetComponent<WallComponent>() != null)
        {
            wallNormal = collision.contacts[0].normal;
            //Debug.Log(wallNormal);
        }

    }
    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((wallLeft || wallRight)  && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(jumpKey)) WallJump();
        }

        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTime > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;

        }

        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (cam == null) return;
        cam.DoFov(90f);
        if (wallLeft) cam.DoTilt(-15f);
        if (wallRight) cam.DoTilt(15f);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        Debug.Log(wallForward);
        if ((orientation.forward - wallForward).magnitude > (orientation.forward + wallForward).magnitude)
            wallForward = -wallForward;
        Debug.Log(wallForward);

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallForward * 100, ForceMode.Force);
        }

        if (useGravity)
            rb.AddForce(transform.up * gravityGounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        cam.DoFov(80f);
        cam.DoTilt(0f);
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        //Debug.Log(forceToApply);
    }


}
