using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangable : MonoBehaviour
{
    // Ссылка на персонажа
    public GameObject hangingPlayer = null;
    public Vector3 center;

    // Центральная точка коллайдера объекта
    public Vector3 ColliderCenter
    {
        get
        {
            Collider collider = GetComponent<Collider>();
            return collider.bounds.center;
        }
    }

    // Пример использования
    private void Start()
    {
        center = ColliderCenter;
    }
    private void Update(){
      // Рисование отметки в центральной точке коллайдера
      Debug.DrawRay(center, Vector3.left, Color.blue, 0.3f);
      Debug.DrawRay(center, Vector3.right, Color.blue, 0.3f);
    }
}
