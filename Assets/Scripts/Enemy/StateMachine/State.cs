using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] private List<Transition> transitions;

    protected Player Target { get; set; }

    public void Enter(Player target)
    {
        if (enabled) return;
        Target = target;
        enabled = true;

        transitions.ForEach(x =>
        {
            x.enabled = true;
            x.Init(Target);
        });
    }

    public State GetNextState()
    {
        return transitions.Where(x => x.NeedTransit)
            .Select(x => x.TargetState)
            .FirstOrDefault();
    }

    public void ExitState()
    {
        if (enabled)
            transitions.ForEach(x => x.enabled = false);
        enabled = false;
    }
}