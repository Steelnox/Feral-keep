using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather_System : MonoBehaviour
{
    public GameObject dirLightsPivot;
    public float cloudsSpeedMovement;

    private float actualSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        actualSpeed = Mathf.Lerp(actualSpeed, Random.Range(cloudsSpeedMovement / 2, cloudsSpeedMovement * 2), Time.deltaTime);
        dirLightsPivot.transform.position += Vector3.right * cloudsSpeedMovement * Time.deltaTime;
    }
}
