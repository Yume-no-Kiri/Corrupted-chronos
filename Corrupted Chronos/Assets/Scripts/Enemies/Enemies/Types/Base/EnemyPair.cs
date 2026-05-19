using UnityEngine;

public class EnemyPair : MonoBehaviour
{
    #region Variables
    public Transform player;
    bool bothUnitsAlive = true;
    bool unitsAliveFlag = false;

    [Header("Leader Params")]
    public EnemyUnit leader; //Leader
    public bool leaderHasTarget;
    public bool shouldLeaderMove;
    public Vector3 leaderNextPos;
    public Transform leaderTarget;

    [Header("Wingman Params")]
    public EnemyUnit wingman; //Wingman
    public bool wingmanHasTarget;
    public bool shouldWingmanMove;
    public Vector3 wingmanNextPos;
    public Transform wingmanTarget;

    [Header("Patrol Params")] //Todas las variables son publicas por ahora, pero se pueden cambiar a un SO y darle una referencia al baseState
    public Vector2 wingmanOffset = new Vector2(4f, 2f); // (x:Back, y:Side)
    public float patrolRadius = 5f;
    public float arriveThreshold = 0.1f;
    public float detectionRadius = 12f;
    public float detectionAngle = 60f; // grados totales del cono
    public float detectionTime = 2f; // Tiempo que deben detectar al jugador para cambiar a estado de aggro
    public bool playerDetected;

    [Header("Attack1 Params")]
    public float attackDistance = 8f;
    public float attackDistanceTolerance = 1f;

    [Header("Pressure Formation Params")]
    public float pressureDistance = 8f;
    public float pressureDistanceTolerance = 1f;
    public float minWingmanAngle = 30;

    //States
    public EnemiesStateMachine stateMachine;

    public baseState patrolling;
    public baseState attack1;
    public baseState pressure;
    public baseState singleState;



    #endregion

    private void Awake()
    {
        stateMachine = new EnemiesStateMachine(this);
        patrolling = new patrollingState(stateMachine);
        attack1 = new attackingState(stateMachine);
        pressure = new pressureState(stateMachine);
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine.initialize(patrolling);
    }

    // Update is called once per frame
    void Update()
    {
        handleLeader();
        handleWingman();

        stateMachine.currentState.FrameUpdate();
        if ((leader == null || wingman == null) && !unitsAliveFlag)
        {
            bothUnitsAlive = false;
            unitsAliveFlag = true;
            if(stateMachine.currentState == pressure)
            {
                stateMachine.changeState(attack1);
            }
        }

        if (leader == null && wingman == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    void handleLeader()
    {
        if (leader != null)
        {
            leader._lookTarget = leaderTarget;
            leader.hasMoveTarget = shouldLeaderMove;
            leader.moveTarget = leaderNextPos;
        }       
    }

    void handleWingman()
    {
        if (wingman != null)
        {
            wingman._lookTarget = wingmanTarget;
            wingman.hasMoveTarget = shouldWingmanMove;
            wingman.moveTarget = wingmanNextPos;
        }      
    }

    public void onPairBroken(EnemyUnit unit)
    {
        singleState = new singleState(stateMachine, unit);
    }

    public void targetDetected()
    {
        //TODO: hacer que se elija un random attack pattern
        if (bothUnitsAlive)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    stateMachine.changeState(attack1);
                    break;
                case 1:
                    stateMachine.changeState(pressure);
                    break;
            }
        }
        else
        {
            stateMachine.changeState(attack1);
            //Pasar a un attacking state individual por defecto
        }
    }
}
