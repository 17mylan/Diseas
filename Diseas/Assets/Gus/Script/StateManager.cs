using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    public State currentState;

    public State Mooving;
    public State Stunning;
    public State Neutre;
    
    void Start()
    {
        currentState.OnEnterState();
    }

    
    void Update()
    {
        currentState.OnUpdateState();
    }

public void SwitchState(State newState)
{
currentState.OnExitState();

currentState = newState;

currentState.OnEnterState();
}


}
