using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetColorer : MonoBehaviour
{
    public Color playerColor = Color.cyan;
    public Color hangableColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        // Найти все объекты с тегом "Player" и изменить их цвет
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Renderer renderer = player.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = playerColor;
            }
        }

        // Найти все объекты с тегом "Hangable" и изменить их цвет
        GameObject[] hangables = GameObject.FindGameObjectsWithTag("Hangable");
        foreach (GameObject hangable in hangables)
        {
            Renderer renderer = hangable.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = hangableColor;
            }
        }
    }
}
