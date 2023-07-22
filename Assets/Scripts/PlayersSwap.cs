using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersSwap : MonoBehaviour
{
    public Transform character;
    public List<Transform> possibleCharaters;
    public int whichCharacter;

    // Start is called before the first frame update
    void Start()
    {
        if (character == null && possibleCharaters.Count > 0)
        {
            character = possibleCharaters[0];
        }
        Swap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (whichCharacter == 0)
            {
                whichCharacter = 1; 
            }
            else
            {
                whichCharacter = 0;
            }
            Swap();
        }
    }
    void Swap()
    {
        character = possibleCharaters[whichCharacter];
        if (whichCharacter == 0)
        {
            possibleCharaters[0].GetComponent<PlayerControllerTeo>().enabled = true;
            possibleCharaters[1].GetComponent<PlayerControllerArt>().enabled = false;
        }
        else
        {
            possibleCharaters[0].GetComponent<PlayerControllerTeo>().enabled = false;
            possibleCharaters[1].GetComponent<PlayerControllerArt>().enabled = true;
        }
    }
}
