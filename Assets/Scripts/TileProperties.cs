using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    private BoxCollider cubeCollider;
    private Vector3[] topEdges;
    public bool rightEdgeHangable = false;
    public bool leftEdgeHangable = false;
    public Transform EdgeMarker;

    public Vector3 rightEdgePoint;
    public Vector3 leftEdgePoint;

    public bool destructible = false;

    private void Start()
    {
        // Получаем коллайдер
        cubeCollider = GetComponent<BoxCollider>();

        // Находим верхние рёбра
        topEdges = GetTopEdges(cubeCollider);

        if (rightEdgeHangable)
        {
            CreateHangableMarker(EdgeMarker, (topEdges[0] + topEdges[1]) * 0.5f);
        }

        if (leftEdgeHangable)
        {
            CreateHangableMarker(EdgeMarker, (topEdges[2] + topEdges[3]) * 0.5f);
        }
    }

    // Метод для нахождения верхних рёбер коллайдера
    private Vector3[] GetTopEdges(BoxCollider collider)
    {
        Vector3 center = collider.bounds.center;
        Vector3 extents = collider.bounds.extents;

        Vector3[] edges = new Vector3[4];
        edges[0] = center + new Vector3(extents.x, extents.y, extents.z); // Верхняя правая задняя вершина
        edges[1] = center + new Vector3(extents.x, extents.y, -extents.z); // Верхняя правая передняя вершина
        edges[2] = center + new Vector3(-extents.x, extents.y, extents.z); // Верхняя левая задняя вершина
        edges[3] = center + new Vector3(-extents.x, extents.y, -extents.z); // Верхняя левая передняя вершина

        return edges;
    }

    // Создание маркера "hangable" на указанной позиции
    private void CreateHangableMarker(Transform markerPrefab, Vector3 position)
    {
        if (markerPrefab != null)
        {
            Transform marker = Instantiate(markerPrefab, position, Quaternion.identity);
            marker.tag = "Hangable";
            // Устанавливаем родительский объект
            marker.parent = transform;
        }
    }

}
