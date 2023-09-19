using UnityEngine;

public class IdleToPatrole : Transition
{
    [SerializeField] private float idleTime;
    private float enterStateTime;

    private void OnEnable()
    {
        NeedTransit = false;
        enterStateTime = Time.time;
    }

    private void Update() => NeedTransit = Time.time - enterStateTime > idleTime;
}