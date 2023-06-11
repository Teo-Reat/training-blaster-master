using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerTeo : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    public float motorPower = 1100;
    public float mass = 800;
    public float jetForce = 100;
    public float jumpMultiplier = 84;
    public float xTorq = 100;

    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    public GameObject centerOfMass;

    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f * 2f, 0);
        mechRb = GetComponent<Rigidbody>();
        mechRb.mass = mass;
        mechRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        if (isOnGround())
        {
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = horizontalInput * ((motorPower * 8) / 6);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                mechRb.AddForce(Vector3.up * jetForce * jumpMultiplier, ForceMode.Impulse);
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = 0;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                mechRb.AddForce(Vector3.up * (jetForce * 3), ForceMode.Impulse);
            }
            mechRb.AddForce(Vector3.forward * (jetForce) * horizontalInput, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q");
            mechRb.AddTorque(xTorq, 0, 0, ForceMode.Impulse);
        }

        // Stabilization in air
        if (!isOnGround())
        {
            if (transform.rotation.x > 0.01)
            {
                mechRb.AddTorque(-xTorq, 0, 0, ForceMode.Impulse);
            }
            if (transform.rotation.x < -0.01)
            {
                mechRb.AddTorque(xTorq, 0, 0, ForceMode.Impulse);
            }
            if (transform.rotation.x < 0.01 && transform.rotation.x > -0.01)
            {
                mechRb.angularVelocity = Vector3.zero;
            }
        }

    }
    private void Update()
    {
        
    }
    private bool isOnGround()
    {
        bool onGround = false;
        foreach(var wheel in wheels)
        {
            if (wheel.isGrounded)
            {
                onGround = true;
            }
        }
        return onGround;
    }
}
