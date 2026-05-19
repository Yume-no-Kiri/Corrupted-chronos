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

        HandleUnitDeaths();

        if (data.leader == null)
            return;

        currentAttackPosition = GenerateAttackPosition();
        data.leaderNextPos = currentAttackPosition;
    }

    public override void FrameUpdate()
    {
        HandleUnitDeaths();

        // Si ambos murieron
        if (data.leader == null)
            return;

        EnemyUnit activeLeader = data.leader;

        float distToPlayer =
            Vector3.Distance(activeLeader.transform.position, data.player.position);

        // --- CONTROL DE DISTANCIA ---
        if (Mathf.Abs(distToPlayer - data.attackDistance)
            > data.attackDistanceTolerance)
        {
            data.leaderNextPos = GenerateAttackPosition();
        }

        // --- WINGMAN FOLLOW ---
        if (data.wingman != null)
        {
            Vector3 offset =
                (-activeLeader.transform.forward * data.wingmanOffset.x) +
                (activeLeader.transform.right * data.wingmanOffset.y);

            data.wingmanNextPos = activeLeader.transform.position + offset;
        }

        // --- ATAQUE DEL LIDER ---
        bool leaderInRange =
            distToPlayer <= data.attackDistance + data.attackDistanceTolerance &&
            distToPlayer >= data.attackDistance - data.attackDistanceTolerance;

        if (leaderInRange)
        {
            activeLeader.primaryAttack();

            data.leaderTarget = data.player.transform;

            if (data.wingman != null)
            {
                data.wingmanTarget = data.player.transform;
            }
        }

        // --- ATAQUE DEL WINGMAN ---
        if (data.wingman != null)
        {
            float wingmanDist =
                Vector3.Distance(
                    data.wingman.transform.position,
                    data.player.position
                );

            if (wingmanDist < data.attackDistance * 0.6f)
            {
                data.wingman.primaryAttack();
            }
        }
    }

    void HandleUnitDeaths()
    {
        // Si murió el líder pero sigue vivo el wingman
        if (data.leader == null && data.wingman != null)
        {
            data.leader = data.wingman;
            data.wingman = null;

            Debug.Log("Wingman promoted to Leader");
        }
    }

    Vector3 GenerateAttackPosition()
    {
        Vector3 playerPos = data.player.position;
        Vector3 leaderPos = data.leader.transform.position;

        Vector3 dir = leaderPos - playerPos;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            dir = data.leader.transform.forward;

        dir.Normalize();

        return playerPos + dir * data.attackDistance;
    }
}