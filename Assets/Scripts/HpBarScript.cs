using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    public Image bar;
    private float start_hp;

    
    void Start()
    {
        start_hp = EnemyScript.hp; 
    }

    void Update()
    {
        bar.fillAmount = EnemyScript.hp / start_hp;
    }
}
