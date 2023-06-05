using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisualTeo : MonoBehaviour
{
    public WheelCollider wheel;
    private Vector3 wheelPos = new Vector3();
    private Quaternion wheelRotation = new Quaternion();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wheel.GetWorldPose(out wheelPos, out wheelRotation);
        transform.position = wheelPos;
        transform.rotation = wheelRotation;
    }
}
