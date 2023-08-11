using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameBehaviorTeo : MonoBehaviour
{
    public TextMeshProUGUI accJetValue;
    public TextMeshProUGUI accGunValue;
    public TextMeshProUGUI accDroidValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowAccValue(string jet, string gun, string droid)
    {
        accGunValue.text = gun;
        accJetValue.text = jet;
        accDroidValue.text = droid;
    }
}
