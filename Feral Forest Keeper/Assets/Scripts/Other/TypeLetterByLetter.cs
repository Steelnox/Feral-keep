using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeLetterByLetter : MonoBehaviour
{
    public float delay = 0.1f;
    public string fullText;
    private string currentText = "";
    private int i = 0;
    private float timer;
    public GameObject canvas;

    private void Update()
    {
        if (i >= fullText.Length)
        {
            timer += Time.deltaTime;
            if(timer >= 2.0f)
            {
                timer = 0;
                currentText = "";
                i = 0;
                canvas.SetActive(false);
            }
        }
    }
    public IEnumerator ShowText()
    {
       for(i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            this.GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
      
       

    }
}
