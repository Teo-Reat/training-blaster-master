using System;
using System.Collections;
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

    [Header("Jumping")] 
    [SerializeField] private float jumpimgForce;
    [SerializeField] private float additionalGravityForce;
    [SerializeField] private bool sealInput;

    [Header("Debug unview later")]
    [SerializeField] private float input;
    [SerializeField] private bool onGround;
    [SerializeField] private bool onSlope;

    [SerializeField] private bool isJumping = true;
    private Vector3 inputDirection;
    private Rigidbody playerRb;
    private CapsuleCollider playerCollider;
    private Vector3 groundNormal;
    Animator animator;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        inputDirection = new Vector3(input, 0, 0);

        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            JumpUp();
        }
    }

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(.1f);
        isJumping = true;
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
            playerRb.velocity += additionalGravityForce * Time.deltaTime * Physics.gravity;
    }

    private void MoveCharacter()
    {
        if (sealInput && isJumping) return;
        if (input == 0 || !onGround)
        {
            animator.SetBool("isWalk", false);
        }
        if (input != 0 && onGround)
        {
            animator.SetBool("isWalk", true);
        }
        var targetSpeed = input * movementSpeed;
        var deltaSpeed = targetSpeed - playerRb.velocity.x;
        var directionAlongGround = Vector3.ProjectOnPlane(Vector3.right, groundNormal);
        var accelRate = input == 0 ? deceleration : acceletarion;
        playerRb.AddForce(directionAlongGround * (accelRate * deltaSpeed), ForceMode.Force);
        transform.LookAt(transform.position + inputDirection);
    }
    void JumpUp()
    {
        animator.SetBool("isJump", true);
        Invoke("SetFallAnimationOn", 0.1f);
    }

    private void CheckGround()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, out var collision, .1f);
        groundNormal = collision.normal;
        if (!onGround)
        {
            StartCoroutine(CoyoteTime());
            animator.SetBool("isFall", true);
        }

        if (onGround)
        {
            isJumping = false;
            animator.SetBool("isFall", isJumping);
            animator.SetBool("isLand", true);
            Invoke("SetLandAnimationOff", 0.1f);
        }  
            
        onSlope = onGround && groundNormal.y != 1;
        playerRb.useGravity = !onSlope;
    }

    private void CheckForwardDirection()
    {
        var isHittingLower = Physics.Raycast(transform.position, transform.forward, out var collisionLower,
            playerCollider.radius);
        //no obstacle on lower raycast
        if (!isHittingLower) return;

        var isHittingUpper = Physics.Raycast(raycastPoint.position, transform.forward, out var collisionUpper,
            playerCollider.radius);
        //no obstacle on upper raysact
        if (!isHittingUpper)
        {
            //not on slope and lower obstacle = 90 degrees => lift character a bit. Im assume where is no ladders on slopes
            if (!onSlope && input != 0 && collisionLower.normal.y == 0)
                playerRb.position += new Vector3(0, stepSmooth, 0);
            return;
        }
        // upper obstacle is not walkable slope or wall
        if (Mathf.Abs(collisionUpper.normal.y) < MinGroundNormalY)
            input = 0;
    }
    void SetFallAnimationOn()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, 0);
        playerRb.AddForce(Vector3.up * jumpimgForce, ForceMode.Impulse);
        animator.SetBool("isJump", false);
        animator.SetBool("isFall", true);
    }
    void SetLandAnimationOff()
    {
        animator.SetBool("isLand", false);
    }
}