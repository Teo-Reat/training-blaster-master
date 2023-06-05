using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetColorer : MonoBehaviour
{
    public Color playerColor = Color.blue;
    public Color hangableColor = Color.red;
    public Color climbableColor = Color.cyan;

    void Start()
    {
        // Изменяем цвет всех объектов с тегом "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Renderer renderer = player.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = playerColor;
            }
        }

        // Изменяем цвет всех объектов с тегом "Hangable" и добавляем скрипт Hangable, если его нет
        GameObject[] hangables = GameObject.FindGameObjectsWithTag("Hangable");
        foreach (GameObject hangable in hangables)
        {
            Renderer renderer = hangable.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = hangableColor;
            }

            // Проверяем, есть ли скрипт Hangable на объекте, и если нет, добавляем его
            if (hangable.GetComponent<Hangable>() == null)
            {
                hangable.AddComponent<Hangable>();
            }
        }

        // Изменяем цвет всех объектов с тегом "Climbable"
        GameObject[] climbables = GameObject.FindGameObjectsWithTag("Climbable");
        foreach (GameObject climbable in climbables)
        {
            Renderer renderer = climbable.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = climbableColor;
            }
        }
    }
}
