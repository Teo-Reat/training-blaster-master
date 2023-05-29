using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTeo : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 50;
    private Rigidbody mechRb;
    // Start is called before the first frame update
    void Start()
    {
        mechRb = GetComponent<Rigidbody>();
        //mechRb.AddForce(Vector3.up * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        //mechRb.AddRelativeForce(Vector3.forward * speed);
        mechRb.AddForce(Vector3.forward * speed * horizontalInput);
    }
}
