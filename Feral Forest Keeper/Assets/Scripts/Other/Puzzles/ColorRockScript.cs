using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRockScript : MonoBehaviour
{
    public List<Color> materialColorRockList;
    public int i;
    public Material materialRock;

    private void Start()
    {
        materialRock = GetComponent<Renderer>().material;
        materialColorRockList.Add(Color.red);
        materialColorRockList.Add(Color.blue);
        materialColorRockList.Add(Color.yellow);
    }

    private void Update()
    {
        materialRock.color = materialColorRockList[i];
    }
    public void ChangeColorRock()
    {
        i++;
        if (i >= materialColorRockList.Count - 1)
        {
            i = 0;
        }
    }
}
