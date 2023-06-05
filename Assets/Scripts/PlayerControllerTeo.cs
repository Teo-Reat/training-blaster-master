using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTeo : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float motorPower = 1100;
    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    public GameObject centerOfMass;
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f * 2f, 0);
        mechRb = GetComponent<Rigidbody>();
        mechRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = horizontalInput * ((motorPower * 8) / 6);
            Vector3 wheelPos = new Vector3();
            Quaternion wheelRot = new Quaternion();
            Debug.Log(wheel.rpm);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space");
            mechRb.AddForce(Vector3.up * 750, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q");
            mechRb.AddTorque(1000, 1000, 1000, ForceMode.Impulse);
        }

    }
}
