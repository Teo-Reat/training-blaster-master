using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerControllerArt : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpimgForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private float MinGroundNormalY = .65f;
    [SerializeField] private bool isDashing;


    private float minMoveDistance = .001f;
    private bool onGround;
    private bool onSlope;
    private Rigidbody playerRb;
    private CapsuleCollider playerCollider;
    private Vector3 targetVelocity; //���� ����� ���������
    private Vector3 Velocity; // ���� �������� ������
    private Vector3 groundNormal;
    private float shellRadius = .02f;
    private float jumpForceMax;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerRb.useGravity = false;
    }

    void Update()
    {
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
            Velocity.y = jumpimgForce;
    }

    void FixedUpdate()
    {
        Velocity += Physics.gravity * (gravityForce * Time.deltaTime);

        Velocity.x = targetVelocity.x * movementSpeed;
        onGround = false;
        onSlope = false;
        jumpForceMax = (jumpimgForce - 1) * Time.fixedDeltaTime;
        transform.LookAt(transform.position + new Vector3(targetVelocity.x, 0, 0));

        var deltaPosition = Velocity * Time.fixedDeltaTime;
        var moveAlongGround = new Vector3(groundNormal.y, -groundNormal.x);
        var move = moveAlongGround * deltaPosition.x;

        Movement(move, false);
        move = Vector3.up * deltaPosition.y;
        Movement(move, true);
    }

    void Movement(Vector3 direction, bool isYDirection)
    {
        var tryingToJump = direction.y > jumpForceMax;
        var isHitAngleOk = true;
        var distance = direction.magnitude; // ����������
        if (distance > minMoveDistance)
        {
            var directionOnYMove = !tryingToJump && isYDirection ? Vector3.down : direction;
            var isHitted = Physics.Raycast(transform.position, directionOnYMove, out var hit,
                (isYDirection ? .1f : playerCollider.radius) + shellRadius);
            if (isHitted)
            {
                var currentNormal = hit.normal;
                isHitAngleOk = currentNormal.y > MinGroundNormalY;
                if (currentNormal.y > MinGroundNormalY)
                {
                    onGround = true;
                    if (isYDirection)
                    {
                        onSlope = currentNormal.y != 1;
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }

                    var projection = Vector3.Dot(Velocity, currentNormal);
                    if (projection < 0)
                        Velocity -= projection * currentNormal;
                    var mod = hit.distance - shellRadius;
                    distance = mod < distance ? mod : distance;
                }
            }
        }

        if ((onSlope && isYDirection && !tryingToJump) || (!isHitAngleOk && !isYDirection)) return;
        playerRb.position += direction.normalized * distance;
    }
}