using UnityEngine;

public class PlayerHangableDetector : MonoBehaviour
{
    private bool isHangableNearby = false;
    private GameObject hangableObject = null;

    public bool IsHangableNearby()
    {
        return isHangableNearby;
    }

    public GameObject GetHangableObject()
    {
        return hangableObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hangable"))
        {
            isHangableNearby = true;
            hangableObject = other.gameObject;
            Debug.Log("Обнаружен объект для подвешивания: " + hangableObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hangable"))
        {
            isHangableNearby = false;
            hangableObject = null;
        }
    }
}
