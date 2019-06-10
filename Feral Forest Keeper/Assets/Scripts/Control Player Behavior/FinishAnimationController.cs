using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAnimationController : MonoBehaviour
{
    private bool dashArriveIsDone;

    void Start()
    {
        dashArriveIsDone = true;
    }

    public bool GetDashArriveIsDone()
    {
        return dashArriveIsDone;
    }
    public void DashArriveFinish()
    {
        dashArriveIsDone = true;
    }
    public void StartDashing()
    {
        dashArriveIsDone = false;
    }
}
