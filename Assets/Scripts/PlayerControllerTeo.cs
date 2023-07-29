//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerTeo : MonoBehaviour
{
    // inputs
    public float horizontalInput;

    // physics
    public float motorPower = 2200;
    private float mass = 3200;
    public float jetForce = 100;
    public float jumpMultiplier = 64;
    public float torqueForce = 1000;
    public float bulletSpeed = 100;

    // object components
    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    public GameObject centerOfMass;
    public GameObject centerOfWeapon;
    private EnergyGeneration energy;

    // jet particles
    public List<ParticleSystem> jumpJets = new List<ParticleSystem>(4);
    public List<ParticleSystem> jetsPush = new List<ParticleSystem>(2);
    public List<ParticleSystem> jetsPull = new List<ParticleSystem>(2);

    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f * 2f, 0);
        mechRb = GetComponent<Rigidbody>();
        mechRb.mass = mass;
        mechRb.centerOfMass = centerOfMass.transform.localPosition;

        energy = GameObject.Find("PlayerVehicle").GetComponent<EnergyGeneration>();

        InvokeRepeating("Test", 1, 1);
    }

    private void FixedUpdate()
    {
        // set inputs
        horizontalInput = Input.GetAxis("Horizontal");

        // mechanics
        if (IsOnGround())
        {
            // add motor torque on wheels
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = horizontalInput * ((motorPower * (wheels.Length * 1.25f)) / wheels.Length);
            }

            // jump
            if (Input.GetKey(KeyCode.W) && energy.Batteries[1].value > 20)
            {
                mechRb.AddForce(Vector3.up * jetForce * jumpMultiplier, ForceMode.Impulse);
                JumpJetsControl(true);
            }
            else
            {
                JumpJetsControl(false);
            }
            MoveJetsControl(false, horizontalInput);
        }
        else
        {
            // disable motor when in air
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = 0;
            }

            // enable jet for fly or slow fall
            if (Input.GetKey(KeyCode.W) && energy.Batteries[1].value > 0)
            {
                mechRb.AddForce(Vector3.up * (jetForce * 3), ForceMode.Impulse);
                energy.DischargeJet(0.02f * 10);
                JumpJetsControl(true);
            }
            else
            {
                JumpJetsControl(false);
            }

            // enable jet for horisontal movement in air
            mechRb.AddForce(Vector3.right * jetForce * horizontalInput, ForceMode.Impulse);
            MoveJetsControl(horizontalInput != 0, horizontalInput);
        }

        // test for mechanics
        if (Input.GetKey(KeyCode.Q))
        {
            //Debug.Log("Q");
            //mechRb.AddTorque(0, 0, torqueForce, ForceMode.Impulse);
        }

        // stabilization in air
        if (!IsOnGround())
        {
            if (transform.rotation.x > 0.01)
            {
                mechRb.AddTorque(0, 0, torqueForce, ForceMode.Impulse);
            }
            if (transform.rotation.x < -0.01)
            {
                mechRb.AddTorque(0, 0, -torqueForce, ForceMode.Impulse);
            }
            if (transform.rotation.x < 0.01 && transform.rotation.x > -0.01)
            {
                mechRb.angularVelocity = Vector3.zero;
            }
        }

    }

    // check for one of wheel in ground
    public bool IsOnGround()
    {
        bool onGround = false;
        foreach (var wheel in wheels)
        {
            if (wheel.isGrounded)
            {
                onGround = true;
            }
        }
        return onGround;
    }

    private void Test()
    {}
    
    private void JumpJetsControl(bool enable)
    {
        foreach (ParticleSystem jet in jumpJets)
        {
            if (enable)
            {
                jet.Play();
            }
            else
            {
                jet.Stop();
            }
        }
    }
    private void MoveJetsControl(bool enable, float input)
    {
        foreach (ParticleSystem jet in jetsPush)
        {
            if (enable && input > 0)
            {
                jet.Play();
            }
            else
            {
                jet.Stop();
            }
        }
        foreach (ParticleSystem jet in jetsPull)
        {
            if (enable && input < 0)
            {
                jet.Play();
            }
            else
            {
                jet.Stop();
            }
        }
        
    }
    public void VehicleReady()
    {
        mechRb.drag = 0.2f;
    }
    public void VehicleStop()
    {
        mechRb.velocity = Vector3.zero;
        mechRb.angularVelocity = Vector3.zero;
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0;
        }
        mechRb.drag = 20;
    }
}
