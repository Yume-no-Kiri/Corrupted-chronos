using UnityEngine;

public class baseStateMachine
{
    public baseStateMachine()
    {
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
