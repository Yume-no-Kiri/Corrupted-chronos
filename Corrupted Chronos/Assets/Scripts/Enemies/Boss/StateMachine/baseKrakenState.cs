using UnityEngine;

public class baseKrakenState : baseState
{
    public baseKrakenState(krakenStateMachine s) : base(s)
    {

    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
