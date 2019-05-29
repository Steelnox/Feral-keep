using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadersControl : MonoBehaviour
{
    private float heightMin = 0.0f;
    private float heightMax = 7.05f;

    public MeshRenderer heightDegradateLightingShader_Material;

    private float worldHeightRelativeToPlayer;
    float min;
    float max;
    float randomAmplitude;
    float randomSpeed;

    void Start()
    {
        heightDegradateLightingShader_Material = gameObject.GetComponent<MeshRenderer>();
        randomAmplitude = Random.Range(0.05f, 0.01f);
        randomSpeed = Random.Range(0.3f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        //heightDegradateLightingShader_Material = gameObject.GetComponent<MeshRenderer>();
        worldHeightRelativeToPlayer = PlayerAnimationController.instance.transform.position.y;
        min = Mathf.Lerp(min, worldHeightRelativeToPlayer + heightMin, Time.deltaTime * 10);
        max = Mathf.Lerp(max, worldHeightRelativeToPlayer + heightMax, Time.deltaTime * 10);
        heightDegradateLightingShader_Material.material.SetFloat("_HeightMin", min);
        heightDegradateLightingShader_Material.material.SetFloat("_HeightMax", max);
        heightDegradateLightingShader_Material.material.SetFloat("_Amplitude", randomAmplitude);
    }
}
