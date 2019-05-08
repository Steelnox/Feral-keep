using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : MonoBehaviour
{
    public GameObject checkpoint;
    public List<GameObject> enemy_list;
    public List<GameObject> rock_list;
    public PlayerController player;
    public PlayerManager playerManager;

    public Item gauntlet;
    
    public bool rockEvent;
    public Vector3 positionCheckpoint;
    public Material materialActivated;

    private bool activatedSanctuary;
    private bool actionDone;
    private Renderer renderer;

    void Start()
    {
        player = PlayerController.instance;
        playerManager = PlayerManager.instance;
        activatedSanctuary = false;
        actionDone = false;
        positionCheckpoint = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
        renderer = GetComponent<Renderer>();
    }

    
    void Update()
    {
        if(activatedSanctuary && !actionDone)
        {
            renderer.material = materialActivated;
            checkpoint.transform.position = positionCheckpoint;
            if(rockEvent)
            {
                foreach (GameObject rock in rock_list)
                {
                    rock.SetActive(true);
                }
            }

            foreach (GameObject enemy in enemy_list)
            {
                enemy.SetActive(true);
            }

            if(playerManager.powerGauntaletItem == null)
            {
                playerManager.powerGauntaletItem = gauntlet;
            }
            

            actionDone = true;
        }
    }

    public void ActivateSanctuary()
    {
        activatedSanctuary = true;
    }
}
