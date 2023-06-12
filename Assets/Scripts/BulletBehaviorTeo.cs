using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviorTeo : MonoBehaviour
{
    private float onScreenDelay = 4;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, onScreenDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Tank"))
        {
            Destroy(gameObject, onScreenDelay);
        }
        
    }
}
