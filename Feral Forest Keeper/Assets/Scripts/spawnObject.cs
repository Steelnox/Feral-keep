using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObject : MonoBehaviour
{

    public KeyCode actionKeyCode = KeyCode.F;
    private bool canSpawnTree;

    public GameObject tree;
    public Transform positionSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawnTree)
        {
            if (Input.GetButtonDown("B"))
            {
                Instantiate(tree, positionSpawn);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canSpawnTree = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canSpawnTree = false;
        }

    }
}
