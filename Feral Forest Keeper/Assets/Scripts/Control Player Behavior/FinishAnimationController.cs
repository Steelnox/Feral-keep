using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAnimationController : MonoBehaviour
{
    public string animationName;
    [SerializeField]
    private bool isDone;
    [SerializeField]
    private bool isPlaying;

    void Start()
    {
        isDone = true;
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FinishAnimation()
    {
        isDone = true;
        isPlaying = false;
    }
    public void InitiateAnimation()
    {
        isDone = false;
        isPlaying = true;
    }
    public bool GetIsDone()
    {
        return isDone;
    }
    public bool GetIsPlaying()
    {
        return isPlaying;
    }
    public string GetAnimationName()
    {
        return animationName;
    }
}
