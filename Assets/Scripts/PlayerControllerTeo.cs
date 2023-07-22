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
    private List<Module> batteries = new List<Module>(5)
    {
        new Module
        { name = "gun", value = 100, max = 100 },
        new Module
        { name = "jet", value = 100, max = 100 },
        new Module
        { name = "droid", value = 100, max = 100 },
    };
    public List<Module> Batteries { get { return batteries; } }
    private float generateFrequency = 0.2f;

    // object components
    private Rigidbody mechRb;
    public WheelCollider[] wheels;
    public GameObject centerOfMass;
    public GameObject centerOfWeapon;
    public GameObject bullet;
    private GameBehaviorTeo gameBehavior;

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

        gameBehavior = GameObject.Find("GameBehavior").GetComponent<GameBehaviorTeo>();

        InvokeRepeating("GenerateEnergy", 0, generateFrequency);
        InvokeRepeating("Test", 1, 1);
    }

    private void FixedUpdate()
    {
        // show acc energy
        ShowAccValue();

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
            if (Input.GetKey(KeyCode.W) && batteries[1].value > 20)
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
            if (Input.GetKey(KeyCode.W) && batteries[1].value > 0)
            {
                mechRb.AddForce(Vector3.up * (jetForce * 3), ForceMode.Impulse);
                batteries[1].value -= (0.02f * 10);
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
        for (int i = 0; i < batteries.Count; i++)
        {
            if (batteries[i].value < batteries[i].max)
            {
                batteries[i].value += chargeUnit / countNotFullBatteries();
            }
        }
    }

    // accumulators value interface
    private void ShowAccValue()
    {
        gameBehavior.ShowAccValue(
            $"{batteries[0].name}: {Math.Floor(batteries[0].value)}",
            $"{batteries[1].name}: {Math.Floor(batteries[1].value)}",
            $"{batteries[2].name}: {Math.Floor(batteries[2].value)}"
        );
    }
    private void Test()
    {}
    public void DischargeGun(float power)
    {
        //accGunCurrent -= power;
        batteries[0].value -= power;
    }
    public void DischargeJet(float power)
    {
        //accGunCurrent -= power;
        batteries[1].value -= power;
    }
    public void DischargeDroid(float power)
    {
        //accGunCurrent -= power;
        batteries[2].value -= power;
    }
    private int countNotFullBatteries()
    {
        int num = 0;
        for (int i = 0; i < batteries.Count; i++)
        {
            if (batteries[i].value < 100)
            {
                num++;
            }
        }
        return num;
    }
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
    public class Module
    {
        public string name;
        public float value;
        public float max;
    }
}
