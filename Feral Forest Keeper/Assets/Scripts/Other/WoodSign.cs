using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSign : MonoBehaviour
{
    public TypeLetterByLetter letterScript;
    public GameObject canvas;

    public void ActivateLetterScript()
    {
        canvas.SetActive(true);
        StartCoroutine(letterScript.ShowText());
    }
}
