using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRockPrincipalScript : MonoBehaviour
{
    public Material materialPrincipalRock;
    public GameObject rock1ToCheck;
    public GameObject rock2ToCheck;

    private Material rockMaterial1;
    private Material rockMaterial2;

    public int orderRock;

    public Color colorRock;

    public bool activated;
    void Start()
    {
        materialPrincipalRock = GetComponent<Renderer>().material;
        

        activated = false;

    }

    void Update()
    {
        rockMaterial1 = rock1ToCheck.GetComponent<Renderer>().material;
        rockMaterial2 = rock2ToCheck.GetComponent<Renderer>().material;
        switch (orderRock)
        {
            case 1:
                if((rockMaterial1.color == Color.yellow && rockMaterial2.color == Color.red) || (rockMaterial1.color == Color.red && rockMaterial2.color == Color.yellow))
                {
                    activated = true;
                }
                else
                {
                    activated = false;
                }
                break;
            case 2:
                if ((rockMaterial1.color == Color.red && rockMaterial2.color == Color.blue) || (rockMaterial1.color == Color.blue && rockMaterial2.color == Color.red))
                {
                    activated = true;
                }
                else
                {
                    activated = false;
                }
                break;
            case 3:
                if ((rockMaterial1.color == Color.blue && rockMaterial2.color == Color.yellow) || (rockMaterial1.color == Color.yellow && rockMaterial2.color == Color.blue))
                {
                    activated = true;
                }
                else
                {
                    activated = false;
                }
                break;
        }

        if (activated)
        {

            materialPrincipalRock.SetColor("_EmissionColor", colorRock);
        }
        else
        {
            materialPrincipalRock.SetColor("_EmissionColor", Color.black);

        }
    }
}
