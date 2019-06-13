using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    private State previousState;

    public void ChangeState(State nextState)
    {
        if (currentState != null) currentState.Exit();

        previousState = currentState;
        currentState = nextState;

        currentState.Enter();
    }

    public void ExecuteState()
    {
        if (currentState != null) currentState.Execute();
    }

    public void BackToPrevious()
    {
        currentState.Exit();
        currentState = previousState;
        currentState.Enter();
    }
}

