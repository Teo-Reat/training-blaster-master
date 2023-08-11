using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviorTeo : MonoBehaviour
{
    private PlayersSwap playersSwap;
    private Vector3 offsetTransport = new Vector3(0, 4, -16);
    private Vector3 offsetDroid = new Vector3(0, 4, -12);

    // Start is called before the first frame update
    void Start()
    {
        playersSwap = GameObject.Find("GameBehavior").GetComponent<PlayersSwap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playersSwap.character == playersSwap.vehicle)
        {
            transform.position = playersSwap.vehicle.transform.position + offsetTransport;
        }
        else
        {
            transform.position = playersSwap.droid.transform.position + offsetDroid;
        }
    }
}
