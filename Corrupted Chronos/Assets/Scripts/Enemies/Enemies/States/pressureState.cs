using UnityEngine;

public class pressureState : baseState
{
    public pressureState(EnemiesStateMachine s) : base(s)
    {
        data = s.enemyPair;
        stateMachine = s;
    }

    EnemiesStateMachine stateMachine;
    EnemyPair data;

    Vector3 leaderAttackPos;
    Vector3 wingmanAttackPos;

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Pressure State Entered");
        data.leaderTarget = data.player.transform;
        data.wingmanTarget = data.player.transform;

        leaderAttackPos = GenerateAttackPosition(data.leader.transform);
        wingmanAttackPos = GenerateWingmanAttackPosition();

        data.leaderNextPos = leaderAttackPos;
        data.wingmanNextPos = wingmanAttackPos;
    }

    public override void FrameUpdate()
    {
        float leaderDist = Vector3.Distance(data.leader.transform.position, data.player.position);
        float wingmanDist = Vector3.Distance(data.wingman.transform.position, data.player.position);

        // --- CONTROL DISTANCIA LIDER ---
        if (Mathf.Abs(leaderDist - data.attackDistance) > data.pressureDistanceTolerance)
        {
            leaderAttackPos = GenerateAttackPosition(data.leader.transform);
            data.leaderNextPos = leaderAttackPos;
        }

        // --- CONTROL DISTANCIA WINGMAN ---
        if (Mathf.Abs(wingmanDist - data.attackDistance) > data.pressureDistanceTolerance)
        {
            wingmanAttackPos = GenerateWingmanAttackPosition();
            data.wingmanNextPos = wingmanAttackPos;
        }

        // --- ATAQUE DEL LIDER ---
        bool leaderInRange =
            leaderDist <= data.attackDistance + data.pressureDistanceTolerance &&
            leaderDist >= data.attackDistance - data.pressureDistanceTolerance;

        if (leaderInRange)
        {
            data.leader.primaryAttack();
        }

        // --- ATAQUE DEL WINGMAN ---
        bool wingmanInRange =
            wingmanDist <= data.attackDistance + data.pressureDistanceTolerance &&
            wingmanDist >= data.attackDistance - data.pressureDistanceTolerance;

        if (wingmanInRange)
        {
            data.wingman.primaryAttack();
        }
    }

    Vector3 GenerateAttackPosition(Transform unit)
    {
        Vector3 playerPos = data.player.position;
        Vector3 unitPos = unit.position;

        Vector3 dir = unitPos - playerPos;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            dir = unit.forward;

        dir.Normalize();

        return playerPos + dir * data.attackDistance;
    }

    Vector3 GenerateWingmanAttackPosition()
    {
        Vector3 playerPos = data.player.position;
        Vector3 leaderPos = data.leader.transform.position;

        Vector3 leaderDir = leaderPos - playerPos;
        leaderDir.y = 0f;

        if (leaderDir.sqrMagnitude < 0.001f)
            leaderDir = data.leader.transform.forward;

        leaderDir.Normalize();

        float minAngle = data.minWingmanAngle;   // por ejemplo 30
        float maxAngle = 90f;

        // elegimos izquierda o derecha
        float randomAngle;

        if (Random.value < 0.5f)
            randomAngle = Random.Range(-maxAngle, -minAngle);
        else
            randomAngle = Random.Range(minAngle, maxAngle);

        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);

        Vector3 wingmanDir = rotation * leaderDir;

        return playerPos + wingmanDir * data.pressureDistance;
    }
}