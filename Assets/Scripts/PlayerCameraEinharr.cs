using UnityEngine;

public class PlayerCameraEinharr : MonoBehaviour
{
    public Transform target; // ������ �� ������ ������
    public float smoothSpeed = 2.7f; // �������� ���������� ������

    private Vector3 offset; // �������� ������ ������������ ������

    private void Start()
    {
        // ������������ � ��������� �������� ������ ������������ ������
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // ������������ �������, � ������� ������ ��������� ������
        Vector3 desiredPosition = target.position + offset;

        // ������ ���������� ������ � �������� �������
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // ������������� ������� ������
        transform.position = smoothedPosition;
    }
}
