using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintsAssistant : MonoBehaviour
{
    public GameObject rightFoot;
    public GameObject leftFoot;

    public void R_Foot()
    {
        PlayerParticlesSystemController.instance.SetWalkDustTrail_R_ParticlesOnScene(rightFoot.transform.position);
    }
    public void L_Foot()
    {
        PlayerParticlesSystemController.instance.SetWalkDustTrail_L_ParticlesOnScene(leftFoot.transform.position);
    }
}
