/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    public GameObject levelScenario;
    public LevelGates levelGateIN;
    public LevelGates levelGateOUT;
    public GameObject levelCheckPoint;

    [SerializeField]
    private bool active = false;

	void Start ()
    {
		
	}
	
	/*void Update ()
    {
		
	}*/
    public void SetActive(bool b)
    {
        active = b;
    }
    public bool IsActive()
    {
        return active;
    }
}
