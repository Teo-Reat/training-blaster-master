using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersSwap : MonoBehaviour
{
    public Transform character;
    public List<Transform> possibleCharaters;
    public int whichCharacter;
    private CameraBehaviorTeo playerCamera;
    private PlayerControllerTeo transportController;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("MainCamera").GetComponent<CameraBehaviorTeo>();
        transportController = GameObject.Find("PlayerVehicle").GetComponent<PlayerControllerTeo>();
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
    // script not so good
    void Swap()
    {
        character = possibleCharaters[whichCharacter];
        if (whichCharacter == 0)
        {
            
            playerCamera.SwapCamera(whichCharacter);
            possibleCharaters[0].GetComponent<PlayerControllerTeo>().enabled = true;
            possibleCharaters[1].GetComponent<PlayerControllerArt>().enabled = false;
            Invoke("ReadyAfterScriptInit", 0.1f);
        }
        else
        {
            transportController.VehicleStop();
            playerCamera.SwapCamera(whichCharacter);
            possibleCharaters[0].GetComponent<PlayerControllerTeo>().enabled = false;
            possibleCharaters[1].GetComponent<PlayerControllerArt>().enabled = true;
        }
    }
    void ReadyAfterScriptInit()
    {
        transportController.VehicleReady();
    }
}
