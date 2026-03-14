using UnityEngine;

public class EnemiesStateMachine
{
    public EnemyPair enemyPair;
    public EnemiesStateMachine(EnemyPair p)
    {
        enemyPair = p;
    }

    public baseState currentState { get; private set; }

    public void initialize(baseState startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void changeState(baseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
