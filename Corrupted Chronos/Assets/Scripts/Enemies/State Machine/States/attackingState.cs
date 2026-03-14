using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class attackingState : baseState
{
    public attackingState(EnemiesStateMachine s) : base(s)
    {
        data = s.enemyPair;
        stateMachine = s;
    }

    EnemiesStateMachine stateMachine;
    EnemyPair data;

    Vector3 currentAttackPosition;

    public override void EnterState()
    {
        base.EnterState();

        currentAttackPosition = GenerateAttackPosition();
        data.leaderNextPos = currentAttackPosition;
    }

    public override void FrameUpdate()
    {
        data.leader.lookTarget = data.player.transform;
        data.wingman.lookTarget = data.player.transform;

        float distToPlayer = Vector3.Distance(data.leader.transform.position, data.player.position);

        if (Mathf.Abs(distToPlayer - data.attackDistance) > data.attackDistanceTolerance)
        {
            data.leaderNextPos = GenerateAttackPosition();
        }

        // Wingman sigue al líder
        Vector3 offset =
            (-data.leader.transform.forward * data.wingmanOffset.x) +
            (data.leader.transform.right * data.wingmanOffset.y);

        data.wingmanNextPos = data.leader.transform.position + offset;
    }

    Vector3 GenerateAttackPosition()
    {
        Debug.Log("aaaa");
        Vector3 playerPos = data.player.position;
        Vector3 leaderPos = data.leader.transform.position;

        Vector3 dir = leaderPos - playerPos;
        dir.y = 0f;

        // Si por alguna razón está exactamente encima del jugador
        if (dir.sqrMagnitude < 0.001f)
            dir = data.leader.transform.forward;

        dir.Normalize();

        return playerPos + dir * data.attackDistance;
    }
}