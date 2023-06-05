using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeControll : MonoBehaviour
{
    private bool isGround = false;
    public ParticleSystem smoke;
    
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground") isGround = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground") isGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGround) smoke.Play(); else smoke.Pause();
    }
}
