using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonRock : MonoBehaviour
{
    private SimonController simonMaster;
    public Color colorRock;
    public Material materialRock;

    void Start()
    {
        simonMaster = SimonController.instance;
        materialRock = GetComponent<Renderer>().material;

    }

    public void InteractWithSimonRock()
    {
        if (simonMaster.sequenceDone && !simonMaster.fail)
        {
            simonMaster.CheckSimon(this.gameObject);
        }
    }
}
