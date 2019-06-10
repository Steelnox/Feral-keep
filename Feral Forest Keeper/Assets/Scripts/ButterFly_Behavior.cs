using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterFly_Behavior : MonoBehaviour
{
    public enum ButterFlyState { GO_TO_POINT, STAY_ON_LOCATION};
    public ButterFlyState actualState;

    public MeshRenderer meshRenderer;

    void Start()
    {
        
    }

    void Update()
    {
        switch (actualState)
        {
            case ButterFlyState.GO_TO_POINT:
                break;
            case ButterFlyState.STAY_ON_LOCATION:
                break;
        }
    }
    private void ChangeState(ButterFlyState newState)
    {
        //EXIT STATE
        switch (actualState)
        {
            case ButterFlyState.GO_TO_POINT:
                break;
            case ButterFlyState.STAY_ON_LOCATION:
                break;
        }
        //ENTER STATE
        switch (newState)
        {
            case ButterFlyState.GO_TO_POINT:
                break;
            case ButterFlyState.STAY_ON_LOCATION:
                break;
        }
        actualState = newState;
    }
    private void MoveWings(float range)
    {

    }
}
