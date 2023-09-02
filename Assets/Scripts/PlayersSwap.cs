using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersSwap : MonoBehaviour
{
    public GameObject character;
    public GameObject vehicle;
    public GameObject droid;
    private PlayerControllerTeo transportController;
    private bool isOnGround;
    private GameObject droidSpawnPoint;

    void Start()
    {
        droidSpawnPoint = GameObject.Find("DroidSpawnPoint");
        transportController = GameObject.Find("PlayerVehicle").GetComponent<PlayerControllerTeo>();
        SelectTransport();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Test();
        }
        isOnGround = transportController.IsOnGround();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Swap();
        }
    }
    // script not so good
    void Swap()
    {
        if (character == vehicle)
        {
            SelectDroid();
        }
        else
        {
            SelectTransport();
        }
    }
    void ReadyAfterScriptInit()
    {
        transportController.VehicleReady();
    }
    void SelectTransport()
    {
        vehicle.GetComponent<PlayerControllerTeo>().enabled = true;
        droid.GetComponent<PlayerControllerArt>().enabled = false;
        droid.SetActive(false);
        character = vehicle;
        Invoke("ReadyAfterScriptInit", 0.1f);
    }
    void SelectDroid()
    {
        if (isOnGround)
        {
            transportController.VehicleStop();
            ChangeDroid();
            vehicle.GetComponent<PlayerControllerTeo>().enabled = false;
            droid.GetComponent<PlayerControllerArt>().enabled = true;
        }
    }
    void Test()
    {
        
    }
    void ChangeDroid()
    {
        droid.transform.position = droidSpawnPoint.transform.position;
        droid.transform.rotation = droidSpawnPoint.transform.rotation;
        droid.SetActive(true);
        character = droid;
    }
}
