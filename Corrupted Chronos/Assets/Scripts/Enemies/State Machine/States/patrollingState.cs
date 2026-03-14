using System.Collections;
using UnityEngine;

public class patrollingState : baseState
{
    public patrollingState(EnemiesStateMachine s) : base(s)
    {

        data = s.enemyPair;
        stateMachine = s;
    }
    EnemiesStateMachine stateMachine;
    EnemyPair data;
    float detectionTimer = 0f;

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void EnterState()
    {
        base.EnterState();
        data.shouldLeaderMove = true;
        data.shouldWingmanMove = true;
        data.leaderNextPos = generateNextPosition();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        Transform leader = data.leader.transform;

        // --- CHECK SI LLEGO AL TARGET ---
        float sqrDistance = (leader.position - data.leaderNextPos).sqrMagnitude;

        if ((leader.position - data.leaderNextPos).sqrMagnitude <= data.arriveThreshold * data.arriveThreshold)
        {
            data.leaderNextPos = generateNextPosition();
        }

        // --- WINGMAN OFFSET ---
        Vector3 offset = (-leader.forward * data.wingmanOffset.x) + (leader.right * data.wingmanOffset.y);
        data.wingmanNextPos = leader.position + offset;
        //data.Unit2.transform.forward = leader.forward;

        // --- CHECK SI WINGMAN LLEGO AL TARGET ---
        bool leaderSees = IsTargetInFOV(data.leader.transform, data.player);
        bool wingmanSees = IsTargetInFOV(data.wingman.transform, data.player);

        bool seesTarget = leaderSees || wingmanSees;

        if (seesTarget)
        {
            detectionTimer += Time.deltaTime;
        }
        else
        {
            // Puedes elegir reset inmediato o decremento gradual.
            detectionTimer -= Time.deltaTime;
        }

        detectionTimer = Mathf.Clamp(detectionTimer, 0f, data.detectionTime);

        if (detectionTimer >= data.detectionTime)
        {

            //Aqui puedo hacer que le envie al state machine
            data.targetDetected();
        }
    }

    Vector3 generateNextPosition()  //TODO: Agregar un minimo de distancia para evitar que elija un punto muy cercano al actual, o un punto que esté dentro de un obstáculo
    {
        return GetRandomPointInCircle(data.leader.transform.position, data.patrolRadius);

        Vector3 GetRandomPointInCircle(Vector3 origin, float radius)
        {
            // Genera dirección aleatoria normalizada en plano XZ
            Vector2 random2D = Random.insideUnitCircle * radius;

            // Mantiene la Y del origen
            return new Vector3(
                origin.x + random2D.x,
                origin.y,
                origin.z + random2D.y
            );
        }
    }

    Vector3 generateWingManPosition(Vector3 leaderPos)
    {
        return Vector3.zero;
    }

    bool IsTargetInFOV(Transform unit, Transform target)
    {
        Vector3 origin = unit.position;
        Vector3 forward = Vector3.ProjectOnPlane(unit.forward, Vector3.up).normalized;

        Vector3 toTarget = target.position - origin;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;

        // 1) Chequeo de distancia
        if (distance > data.detectionRadius)
            return false;

        Vector3 dirToTarget = toTarget.normalized;

        // 2) Chequeo de ángulo
        float dot = Vector3.Dot(forward, dirToTarget);

        float halfAngle = data.detectionAngle * 0.5f;
        float threshold = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

        return dot >= threshold;
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
