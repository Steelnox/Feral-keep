using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public GameObject checkpoint;
    public PlayerController player;
    void Start()
    {
        player = PlayerController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player")
        {
            checkpoint.transform.position = player.transform.position;
        }
    }
}
