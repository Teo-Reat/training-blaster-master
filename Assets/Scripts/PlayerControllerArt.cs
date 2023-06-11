using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerControllerArt : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private int acceletarion = 50;
    [SerializeField] private int deceleration = 50;

    [Header("Ground Parameters")]
    [SerializeField, Range(0, 1)] private float MinGroundNormalY = .65f;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private float stepSmooth = .1f;

    [Header("Jumping")] [SerializeField] private float jumpimgForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private bool sealInput;

    [Header("Debug unview later")]
    [SerializeField] private float input;
    [SerializeField] private bool onGround;
    [SerializeField] private bool onSlope;

    private Vector3 inputDirection;
    private Rigidbody playerRb;
    private CapsuleCollider playerCollider;
    private Vector3 groundNormal;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        inputDirection = new Vector3(input, 0, 0);

        if (!Input.GetKeyDown(KeyCode.Space) || !onGround) return;
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, 0);
        playerRb.AddForce(Vector3.up * jumpimgForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        CheckGround();
        CheckForwardDirection();
        AddGravity();
        MoveCharacter();
    }

    private void AddGravity()
    {
        if (!onGround && playerRb.velocity.y <= 0)
            playerRb.velocity += gravityForce * Time.deltaTime * Physics.gravity;
    }

    private void MoveCharacter()
    {
        if (sealInput && !onGround) return;
        var targetSpeed = input * movementSpeed;
        var deltaSpeed = targetSpeed - playerRb.velocity.x;
        var directionAlongGround = Vector3.ProjectOnPlane(Vector3.right, groundNormal);
        var accelRate = input == 0 ? deceleration : acceletarion;
        playerRb.AddForce(directionAlongGround * (accelRate * deltaSpeed), ForceMode.Force);
        transform.LookAt(transform.position + inputDirection);
    }

    private void CheckGround()
    {
        var isHitting = Physics.Raycast(transform.position, Vector3.down, out var collision, .1f);
        groundNormal = collision.normal;
        onGround = isHitting;
        onSlope = onGround && groundNormal.y != 1;
        playerRb.useGravity = !onSlope;
    }

    private void CheckForwardDirection()
    {
        var isHittingLower = Physics.Raycast(transform.position, transform.forward, out var collisionLower,
            playerCollider.radius);
        if (!isHittingLower) return;
        var isHittingUpper = Physics.Raycast(raycastPoint.position, transform.forward, out var collisionUpper,
            playerCollider.radius);
        if (!isHittingUpper)
        {
            if (!onSlope && input != 0)
                playerRb.position += new Vector3(0, stepSmooth, 0);
            return;
        }

        if (Mathf.Abs(collisionUpper.normal.y) < MinGroundNormalY)
            input = 0;
    }
}