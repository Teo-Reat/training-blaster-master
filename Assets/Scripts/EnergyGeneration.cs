using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGeneration : MonoBehaviour
{
    private GameBehaviorTeo gameBehavior;

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

    // start
    private void Start()
    {
        gameBehavior = GameObject.Find("GameBehavior").GetComponent<GameBehaviorTeo>();
        InvokeRepeating("GenerateEnergy", 0, generateFrequency);
    }

    // fixed update
    private void FixedUpdate()
    {
        ShowAccValue();
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
    public void DischargeGun(float power)
    {
        batteries[0].value -= power;
    }
    public void DischargeJet(float power)
    {
        batteries[1].value -= power;
    }
    public void DischargeDroid(float power)
    {
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
    public class Module
    {
        public string name;
        public float value;
        public float max;
    }
}
