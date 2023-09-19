using UnityEngine;

public class ToPatrole : Transition
{
    [SerializeField] private float idleTime;
    [SerializeField] bool test;
    private float enterStateTime;

    void OnEnable()
    {
        NeedTransit = false;
        enterStateTime = Time.time;
    }

    void Update()
    {
        NeedTransit = Time.time - enterStateTime > idleTime;
    }
}
