using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State targetState;
    protected Player Target { get; private set; }

    public State TargetState => targetState; // null/destroy check

    public bool NeedTransit { get; protected set; }

    public void Init(Player target) => Target = target;

    private void OnEnable() => NeedTransit = false;
}