using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private State firstState;
    private State currentState;
    private Player target;

    public State CurrentState => currentState;

    private void Start()
    {
        target = GetComponent<Enemy>().Target;
        Reset(firstState);
    }

    private void Reset(State startState)
    {
        currentState = startState;
        if (currentState != null)
            currentState.Enter(target);
    }

    private void Update()
    {
        if (!currentState) return; // null check change if not work

        var nextState = currentState.GetNextState();
        if (nextState)
            Transit(nextState);
    }

    private void Transit(State nextState)
    {
        if (currentState)
            currentState.ExitState();
        currentState = nextState;

        if (currentState) // redundant ?
            currentState.Enter(target);
    }
}