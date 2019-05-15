using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadersControl : MonoBehaviour
{
    private float heightMin = 0.0f;
    private float heightMax = 5.10f;

    public MeshRenderer heightDegradateLightingShader_Material;

    private float worldHeightRelativeToPlayer;

    void Start()
    {
        heightDegradateLightingShader_Material = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //heightDegradateLightingShader_Material = gameObject.GetComponent<MeshRenderer>();
        worldHeightRelativeToPlayer = PlayerAnimationController.instance.transform.position.y;

        heightDegradateLightingShader_Material.material.SetFloat("_HeightMin", worldHeightRelativeToPlayer + heightMin);
        heightDegradateLightingShader_Material.material.SetFloat("_HeightMax", worldHeightRelativeToPlayer + heightMax);
    }
}
