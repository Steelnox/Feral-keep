
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateItem : MonoBehaviour
{
    [SerializeField] float m_XRotationsPerMinute = 1f;
    [SerializeField] float m_YRotationPerMinute = 1f;
    [SerializeField] float m_ZRotationPerMinute = 1f;

    // Update is called once per frame
    void Update()
    {
        // Degrees per frame ^-1 = seconds frame^-1 / seconds minute ^-1 * degrees rotation ^-1 * rotation per minute ^-1
        float l_XDegreesPerFrame = Time.deltaTime / 60 * 360 * m_XRotationsPerMinute;
        transform.RotateAround(transform.position,transform.right,l_XDegreesPerFrame);
        float l_YDegreesPerFrame = Time.deltaTime / 60 * 360 * m_YRotationPerMinute;
        transform.RotateAround(transform.position,transform.up,l_YDegreesPerFrame);
        float l_ZDegreesPerFrame = Time.deltaTime / 60 * 360 * m_ZRotationPerMinute;
        transform.RotateAround(transform.position,transform.forward,l_ZDegreesPerFrame);
    }
}
