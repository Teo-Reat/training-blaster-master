using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTeo : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float motorPower = 700;
    //public float speed = 50;
    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    public Rigidbody[] jets;
    public GameObject centerOfMass;
    // Start is called before the first frame update
    void Start()
    {
        //Physics.gravity = new Vector3(0, -9.81f * 2f, 0);
        mechRb = GetComponent<Rigidbody>();
        mechRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = horizontalInput * motorPower;
        }
        
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            foreach (var jet in jets)
            {
                if (true)
                {

                }
                Rigidbody jetDrive = GetComponent<Rigidbody>();
                jetDrive.AddForce(Vector3.up * 1000, ForceMode.Impulse);
            }
            //mechRb.AddForce(Vector3.up * 5000, ForceMode.Impulse);
        }
        //horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        //mechRb.AddRelativeForce(Vector3.forward * speed);
        //mechRb.AddForce(Vector3.forward * speed * horizontalInput, ForceMode.Acceleration);
    }
}
