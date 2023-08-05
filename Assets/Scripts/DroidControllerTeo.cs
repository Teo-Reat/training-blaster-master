using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidControllerTeo : MonoBehaviour
{
    Rigidbody droidRb;
    Animator animator;
    public float horizontalInput;
    public float moveMultiplier;
    public float jumpMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        droidRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {
        Move();
        FallFaster();
    }
    void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        droidRb.AddForce(Vector3.right * horizontalInput * moveMultiplier, ForceMode.Impulse);
        if (horizontalInput == 0)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            //Debug.Log("else");
            animator.SetBool("isWalk", true);
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            droidRb.AddForce(Vector3.up * moveMultiplier * jumpMultiplier);
            animator.SetBool("isJump", true);
            Invoke("FallFaster", 0.5f);
        }
        else
        {
            animator.SetBool("isJump", false);
        }
    }
    void FallFaster()
    {
        //Debug.Log("FAAAAAAALL!");
        droidRb.AddForce(1, -4300, 1);
    }
}
