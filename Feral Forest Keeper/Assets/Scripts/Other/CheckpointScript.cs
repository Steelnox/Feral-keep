using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    bool checkpointActivated;
    public GameObject checkpoint;
    public PlayerController player;
    void Start()
    {
        checkpointActivated = false;
        player = PlayerController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!checkpointActivated && other.tag == "Player")
        {
            checkpoint.transform.position = player.transform.position;
            checkpointActivated = true;
        }
    }
}
