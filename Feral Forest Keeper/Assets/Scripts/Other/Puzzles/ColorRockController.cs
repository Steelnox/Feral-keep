using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRockController : MonoBehaviour
{
    public Sanctuary sanctuary;

    public ColorRockPrincipalScript cristal1;
    public ColorRockPrincipalScript cristal2;
    public ColorRockPrincipalScript cristal3;


    void Update()
    {
        if (cristal1.activated && cristal2.activated && cristal3.activated) sanctuary.ActivateSanctuary();
    }
}
