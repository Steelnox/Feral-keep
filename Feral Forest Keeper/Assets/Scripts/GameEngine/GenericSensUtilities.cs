using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSensUtilities : MonoBehaviour
{
    #region Singleton

    public static GenericSensUtilities instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public float DistanceBetween2Vectors(Vector3 a, Vector3 b)
    {
        return (a - b).magnitude;
    }
    public Vector3 GetDirectionFromTo_N(Vector3 from, Vector3 to)
    {
        Vector3 result;

        return result = (to - from).normalized;
    }
    /// <summary>
    /// Transform 3D Vector to 2D Vector(3Dvec.x, 3Dvec.z).
    /// </summary>
    public Vector2 Transform3DTo2DMovement(Vector3 movVect)
    {
        Vector2 mov2D;
        mov2D.x = movVect.x;
        mov2D.y = movVect.z;

        return mov2D;
    }
    /// <summary>
    /// Transform 2D Vector to 3D Vector(2Dvec.x, 0, 2Dvec.y).
    /// </summary>
    public Vector3 Transform2DTo3DMovement(Vector2 movVect)
    {
        Vector3 mov3D;
        mov3D.x = movVect.x;
        mov3D.y = 0;
        mov3D.z = movVect.y;

        return mov3D;
    }
}
