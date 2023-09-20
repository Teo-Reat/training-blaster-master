using System.Collections.Generic;
using UnityEngine;

public class Patrole : State
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed;

    private int currentPoint;
    public Transform CurrentDestination => waypoints[currentPoint];

    private void OnEnable() =>
        transform.LookAt(transform.position + new Vector3(waypoints[currentPoint].position.x, 0, 0));

    private void LateUpdate()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, waypoints[currentPoint].position, Time.deltaTime * speed);
    }

    private void OnDisable() => currentPoint = ++currentPoint % waypoints.Count;
}