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
        float distToPlayer = Vector3.Distance(data.leader.transform.position, data.player.position);

        // --- CONTROL DE DISTANCIA DEL LIDER ---
        if (Mathf.Abs(distToPlayer - data.attackDistance) > data.attackDistanceTolerance)
        {
            data.leaderNextPos = GenerateAttackPosition();
        }

        // --- WINGMAN SIGUE AL LIDER ---
        Vector3 offset =
            (-data.leader.transform.forward * data.wingmanOffset.x) +
            (data.leader.transform.right * data.wingmanOffset.y);

        data.wingmanNextPos = data.leader.transform.position + offset;

        // --- ATAQUE DEL LIDER ---
        bool leaderInRange =
            distToPlayer <= data.attackDistance + data.attackDistanceTolerance &&
            distToPlayer >= data.attackDistance - data.attackDistanceTolerance;

        if (leaderInRange)
        {
            data.leader.primaryAttack();

            data.leaderTarget = data.player.transform;
            data.wingmanTarget = data.player.transform;
        }

        // --- ATAQUE DEL WINGMAN (SI EL JUGADOR SE ACERCA DEMASIADO) ---
        float wingmanDist = Vector3.Distance(data.wingman.transform.position, data.player.position);

        if (wingmanDist < data.attackDistance * 0.6f) // puedes ajustar este factor
        {
            data.wingman.primaryAttack();
        }
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