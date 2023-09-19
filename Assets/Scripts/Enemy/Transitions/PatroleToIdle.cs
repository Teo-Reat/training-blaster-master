using UnityEngine;

public class PatroleToIdle : Transition
{
    [SerializeField] private float accuracy;
    [SerializeField] private Patrole patroleState;

    private void Update()
    {
        var isNearWaypoint = Vector3.Distance(transform.position, patroleState.CurrentDestination.transform.position) <
                             accuracy;
        if (isNearWaypoint)
            NeedTransit = true;
    }
}