using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushGrass_Behavior : MonoBehaviour
{
    public BushParts_Behavior[] parts;
    //public float drag;
    public bool playerOver;
    public float grassContactDistance;
    public float bushContactDistance;

    void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].interactionDistance = grassContactDistance;
        }
    }

    void Update()
    {
        if (playerOver)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].playerInRange = playerOver;
            }
        }
        //if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, g.transform.position) < g.bushContactDistance - 0.1f
        //        && PlayerController.instance.playerRoot.transform.position.y >= g.transform.position.y - 0.2f
        //        && PlayerController.instance.playerRoot.transform.position.y < g.transform.position.y + 0.2f)
        if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, transform.position) < bushContactDistance - 0.1f
                && PlayerController.instance.playerRoot.transform.position.y >= transform.position.y - 0.2f
                && PlayerController.instance.playerRoot.transform.position.y < transform.position.y + 0.2f)
        {
            playerOver = true;
            PlayerController.instance.MovingInSlowZone(true);
        }
        else
        {
            playerOver = false;
        }
    }
}
