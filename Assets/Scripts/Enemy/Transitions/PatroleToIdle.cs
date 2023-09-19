using UnityEngine;

[RequireComponent(typeof(Patrole))]
public class PatroleToIdle : Transition
{
    [SerializeField] private float accuracy;
    private Patrole patroleState;
    private void Start()
    {
        patroleState = GetComponent<Patrole>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, patroleState.CurrentDestination.transform.position) < accuracy)
            NeedTransit = true;
    }
}
