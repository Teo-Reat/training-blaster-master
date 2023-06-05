using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangable : MonoBehaviour
{
    // Ссылка на персонажа
    public GameObject hangingPlayer = null;

    // Метод для выполнения цепляния персонажа за объект
    public void Hang(GameObject player)
    {
        // Сохраняем ссылку на персонажа
        hangingPlayer = player;

        // Получаем нижнюю точку коллайдера персонажа
        Vector3 hangPosition = GetComponent<Collider>().bounds.max;
        // Получаем компонент коллайдера игрока
        Collider playerCollider = player.GetComponent<Collider>();

        // Вычитаем высоту коллайдера игрока из hangPosition
        hangPosition -= new Vector3(0f, playerCollider.bounds.size.y, playerCollider.bounds.center.z);


        // Прикрепляем персонажа к объекту
        Rigidbody playerRigidbody = hangingPlayer.GetComponent<Rigidbody>();
        playerRigidbody.useGravity = false; // Отключаем гравитацию

        // Запускаем корутину для плавного перемещения персонажа
        StartCoroutine(MovePlayerToHangPosition(hangingPlayer.transform, hangPosition));
        hangingPlayer.transform.parent = transform;
    }

    private IEnumerator MovePlayerToHangPosition(Transform playerTransform, Vector3 targetPosition)
    {
        float duration = 0.1f; // Длительность перемещения
        float elapsedTime = 0f;

        Vector3 startPosition = playerTransform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Вычисляем текущую позицию персонажа с учетом плавного перемещения
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // Применяем новую позицию к персонажу
            playerTransform.position = newPosition;

            yield return null;
        }

        // Завершение перемещения, сбрасываем скорость персонажа
        Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        playerRigidbody.velocity = Vector3.zero;
    }





    // Метод для отсоединения персонажа от объекта
    public void Release()
    {
      if (hangingPlayer != null)
      {
          // Включаем гравитацию для персонажа
          Rigidbody playerRigidbody = hangingPlayer.GetComponent<Rigidbody>();
          playerRigidbody.useGravity = true;

          // Отсоединяем персонажа от объекта и сбрасываем ссылку
          hangingPlayer.transform.parent = null;
          hangingPlayer = null;
      }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Обнаружен персонаж, цепляем его к объекту
      //      Hang(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject == hangingPlayer)
        {
            // Персонаж вышел из зоны объекта, отсоединяем его
          //  Release();
        }
    }
}
