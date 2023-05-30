using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTeo : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float motorPower = 500;
    //public float speed = 50;
    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    // Start is called before the first frame update
    void Start()
    {
        //    Physics.gravity = new Vector3(0, -9.81f * 2f, 0);
        mechRb = GetComponent<Rigidbody>();
    //    mechRb.centerOfMass = new Vector3(0, -0.7f, 0);
    //    //mechRb.AddForce(Vector3.up * speed, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = horizontalInput * motorPower;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    horizontalInput = Input.GetAxis("Horizontal");
    //    //verticalInput = Input.GetAxis("Vertical");
    //    //mechRb.AddRelativeForce(Vector3.forward * speed);
    //    mechRb.AddForce(Vector3.forward * speed * horizontalInput, ForceMode.Acceleration);
    //}
}
