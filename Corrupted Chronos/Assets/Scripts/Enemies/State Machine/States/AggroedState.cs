using UnityEngine;

public class AggroedState : baseState
{
    public AggroedState(EnemiesStateMachine s) : base(s)
    {

        data = s.enemyPair;
        stateMachine = s;
    }
    EnemiesStateMachine stateMachine;
    EnemyPair data;

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Aggroed State Entered");
        data.leaderNextPos = data.leader.transform.position; // El líder se queda en su posición actual
        data.wingmanNextPos = data.wingman.transform.position; // El wingman se mueve hacia el líder
    }

    public override void ExitState()
    {
        base.ExitState();
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
