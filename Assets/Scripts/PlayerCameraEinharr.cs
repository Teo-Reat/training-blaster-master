using UnityEngine;

public class PlayerCameraEinharr : MonoBehaviour
{
    public Transform target; // Ссылка на объект игрока
    public float smoothSpeed = 2.7f; // Скорость следования камеры

    private Vector3 offset; // Смещение камеры относительно игрока

    private void Start()
    {
        // Рассчитываем и сохраняем смещение камеры относительно игрока
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // Рассчитываем позицию, к которой должна двигаться камера
        Vector3 desiredPosition = target.position + offset;

        // Плавно перемещаем камеру к желаемой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Устанавливаем позицию камеры
        transform.position = smoothedPosition;
    }
}
