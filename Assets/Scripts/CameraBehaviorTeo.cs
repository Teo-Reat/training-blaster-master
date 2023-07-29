using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviorTeo : MonoBehaviour
{
    public List<GameObject> players;
    public GameObject activePlayer;
    private Vector3 offsetTransport = new Vector3(0, 4, -16);
    private Vector3 offsetDroid = new Vector3(0, 4, -12);

    // Start is called before the first frame update
    void Start()
    {
        //SwapCamera(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (activePlayer == players[0])
        {
            transform.position = activePlayer.transform.position + offsetTransport;
        }
        else
        {
            transform.position = activePlayer.transform.position + offsetDroid;
        }
    }
    public void SwapCamera(int playerIndex)
    {
        activePlayer = players[playerIndex];
    }
}
