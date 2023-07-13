using System;
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

    // energy management
    private float reactorPower = 5;
    private float gunShootCost = 0.2f;
    private float generateFrequency = 0.2f;
    private float accGunMax = 100;
    private float accGunCurrent = 100;
    private float accJetMax = 100;
    private float accJetCurrent = 100;
    private float accDroidMax = 100;
    private float accDroidCurrent = 100;

    // object components
    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    public GameObject centerOfMass;
    public GameObject centerOfWeapon;
    public GameObject bullet;
    private GameBehaviorTeo gameBehavior;

    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f * 2f, 0);
        mechRb = GetComponent<Rigidbody>();
        mechRb.mass = mass;
        mechRb.centerOfMass = centerOfMass.transform.localPosition;

        gameBehavior = GameObject.Find("GameBehavior").GetComponent<GameBehaviorTeo>();

        InvokeRepeating("GenerateEnergy", 0, generateFrequency);
        InvokeRepeating("Test", 1, 1);
    }

    private void FixedUpdate()
    {
        // show acc energy
        ShowAccValue();

        // shooting
        //if (Input.GetKey(KeyCode.E) && accGunCurrent > gunShootCost)
        //{
        //    GameObject newBullet = Instantiate(bullet, centerOfWeapon.transform.position + new Vector3(0, 0, 0), centerOfWeapon.transform.rotation) as GameObject;
        //    Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        //    bulletRb.velocity = transform.forward * bulletSpeed;
        //    accGunCurrent -= gunShootCost;

        //}

        // set inputs
        horizontalInput = Input.GetAxis("Horizontal");

        // mechanics
        if (isOnGround())
        {
            // add motor torque on wheels
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = horizontalInput * ((motorPower * (wheels.Length * 1.25f)) / wheels.Length);
            }

            // jump
            if (Input.GetKey(KeyCode.W) && accJetCurrent > 20)
            {
                mechRb.AddForce(Vector3.up * jetForce * jumpMultiplier, ForceMode.Impulse);
            }
        }
        else
        {
            // disable motor when in air
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = 0;
            }

            // enable jet for fly or slow fall
            if (Input.GetKey(KeyCode.W) && accJetCurrent > 0)
            {
                mechRb.AddForce(Vector3.up * (jetForce * 3), ForceMode.Impulse);
                accJetCurrent -= (0.02f * 10);
            }

            // enable jet for horisontal movement in air
            mechRb.AddForce(Vector3.right * jetForce * horizontalInput, ForceMode.Impulse);
        }

        // test for mechanics
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q");
            mechRb.AddTorque(0, 0, torqueForce, ForceMode.Impulse);
        }

        // stabilization in air
        if (!isOnGround())
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
    private bool isOnGround()
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

    // energy generation
    private void GenerateEnergy()
    {
        float chargeUnit = reactorPower / (1 / generateFrequency);
        if (accGunCurrent < accGunMax)
        {
            accGunCurrent += chargeUnit;
        }
        else if (accJetCurrent < accJetMax)
        {
            accJetCurrent += chargeUnit;
        }
        else if (accDroidCurrent < accDroidMax)
        {
            accDroidCurrent += chargeUnit;
        }
    }

    // accumulators value interface
    private void ShowAccValue()
    {
        gameBehavior.ShowAccValue(
            $"Gun: {Math.Round(accGunCurrent)}",
            $"Jet: {Math.Round(accJetCurrent)}",
            $"Droid: {Math.Round(accDroidCurrent)}"
        );
    }
    private void Test()
    {}
    public void DischargeGun(float power)
    {
        accGunCurrent -= power;
    }
    public void DischargeJet(float power)
    {
        accGunCurrent -= power;
    }
    public void DischargeDroid(float power)
    {
        accGunCurrent -= power;
    }
}
