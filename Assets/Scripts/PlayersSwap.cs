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
    private GameObject droidDoll;

    void Start()
    {
        droidDoll = GameObject.Find("PlayerDroidDoll");
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
        droidDoll.SetActive(true);
        Animation dr = droidDoll.GetComponent<Animation>();
        dr.Play("DroidDollGoIn");
        character = vehicle;
        Invoke("ReadyAfterScriptInit", 0.1f);
    }
    void SelectDroid()
    {
        if (isOnGround)
        {
            transportController.VehicleStop();
            DollExit();
            vehicle.GetComponent<PlayerControllerTeo>().enabled = false;
            droid.GetComponent<PlayerControllerArt>().enabled = true;
        }
    }
    void DollExit()
    {
        Debug.Log("DollExit");
        Animation dr = droidDoll.GetComponent<Animation>();
        dr.Play("DroidDollGoOut");
        Invoke("ChangeDroid", 1);

    }
    void Test()
    {
        Animation dr = droidDoll.GetComponent<Animation>();
        dr.Play("DroidDollGoIn");
    }
    void ChangeDroid()
    {
        droid.transform.position = droidDoll.transform.position;
        droid.transform.rotation = droidDoll.transform.rotation;
        droid.SetActive(true);
        character = droid;
        droidDoll.SetActive(false);
    }
}
