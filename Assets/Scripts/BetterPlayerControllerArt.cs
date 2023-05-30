using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class BetterPlayerControllerArt : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    //[SerializeField] private float gravityModifier;
    [SerializeField] private float MinGroundNormalY = .65f;
    [SerializeField] private bool onGround;
    [SerializeField] private bool onSlope;
    [SerializeField] private Vector3 velocity;

    private float aceleration = 100;
    //private bool isJumping;
    private Vector3 groundNormal;
    private Vector3 input;
    private Rigidbody playerRb;
    private CapsuleCollider playerCollider;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        input = Vector3.right * Input.GetAxis("Horizontal");
        transform.LookAt(transform.position + input);

        if (!onGround || !Input.GetKeyDown(KeyCode.Space)) return;
        //isJumping = true;
        var localVelocity = new Vector3(playerRb.velocity.x, 0, 0);
        playerRb.velocity = localVelocity;
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        onGround = CheckDirection(Vector3.down, .1f, out groundNormal);
        onSlope = groundNormal.y != 1 && onGround;

        playerRb.useGravity = !onSlope;
        var directionOnSurface =
            Vector3.ProjectOnPlane(input, groundNormal) * aceleration;

        //WIP
        var isPathBlocked = CheckDirection(transform.forward, playerCollider.radius, out var wallNormal);
        if (isPathBlocked && MathF.Abs(wallNormal.y) < MinGroundNormalY) return;

        ConstrainSpeed();
        MakeCharacterStopIfNoInput(directionOnSurface);

        //debug purposes
        velocity = playerRb.velocity;
    }

    private void MakeCharacterStopIfNoInput(Vector3 directionOnSurface)
    {
        //типа работает но надо еще leeway починить?
        if (onSlope && !Input.anyKey)
            playerRb.velocity = Vector3.zero;
        else if (onGround && !Input.anyKey)
            playerRb.velocity = (Vector3.down + input) * movementSpeed;
        else
            playerRb.AddForce(directionOnSurface);
    }

    private void ConstrainSpeed()
    {
        // ограничение x и y на земле
        if (onGround && playerRb.velocity.magnitude > movementSpeed)
            playerRb.velocity = playerRb.velocity.normalized * movementSpeed;

        //ограничение x скорости и разграничение y в прыжке
        else if (!onGround && MathF.Abs(playerRb.velocity.x) > movementSpeed)
        {
            var localX = playerRb.velocity.x >= 0 ? movementSpeed : -movementSpeed;
            playerRb.velocity = new Vector3(localX, playerRb.velocity.y, 0);
        }
    }

    private bool CheckDirection(Vector3 direction, float distance, out Vector3 hitSurface)
    {
        var isHitted = Physics.Raycast(transform.position, direction, out var surface, distance);
        hitSurface = surface.normal;
        return isHitted && surface.collider.CompareTag($"Ground");
    }
}