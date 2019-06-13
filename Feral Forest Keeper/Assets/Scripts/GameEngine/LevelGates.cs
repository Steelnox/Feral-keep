/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class LevelGates : MonoBehaviour
{
    [SerializeField]
    private bool usingGate;

	/*void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}*/
    public void UseGate()
    {
        usingGate = true;
    }
    public void ResetGate()
    {
        usingGate = false;
    }
    public bool IsBeingUsed()
    {
        return usingGate;
    }
}
