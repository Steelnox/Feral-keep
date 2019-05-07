using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLookAt : MonoBehaviour
{
    public Transform camera;
    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(camera);
    }
}
