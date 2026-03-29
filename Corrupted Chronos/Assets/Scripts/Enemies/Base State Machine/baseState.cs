using System;
using UnityEngine;

public class baseState
{
    public baseState(baseStateMachine s)
    {
        this.stateMachine = s;
    }

    baseStateMachine stateMachine;

    //Called By State Machine upon entering state
    public virtual void EnterState()
    {
        
    }

    //Called By State Machine upon exiting state
    public virtual void ExitState()
    {
        
    }

    //Called by State Machine in Update
    public virtual void FrameUpdate()
    {
    }

    //Called by State Machine in FixedUpdate
    public virtual void PhysicsUpdate()
    {
    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void OnTriggerStay(Collider other)
    {
    }

    public virtual void OnTriggerExit(Collider other)
    {
    }

    public virtual void AnimationTriggerEvent()
    {

    }
}
