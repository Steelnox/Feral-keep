using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushGrass_Behavior : MonoBehaviour
{
    public BushParts_Behavior[] parts;
    public int numOfParts;
    //public float drag;
    public bool playerOver;
    public float grassContactDistance;
    public float bushContactDistance;

    private int numInactiveParts;

    void Start()
    {
        if (numOfParts > parts.Length) numOfParts = parts.Length;
        if (numOfParts <= 0) numOfParts = 0;

        numInactiveParts = parts.Length - numOfParts;
        for (int i = numOfParts; i < parts.Length; i++)
        {
            parts[i].bodyPivot.GetComponentInChildren<MeshRenderer>().enabled = false;
            parts[i].enabled = false;
        }
        for (int i = 0; i < numOfParts; i++)
        {
            parts[i].interactionDistance = grassContactDistance;
        }
    }
    void Update()
    {
        if (playerOver)
        {
            for (int i = 0; i < numOfParts; i++)
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
