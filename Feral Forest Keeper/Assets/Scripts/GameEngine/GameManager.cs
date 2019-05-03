using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool pause;
    public RectTransform provisionalGUIMenu;

    private Vector2 provisionalGUIMenuOnScreenPos;
    private Vector2 hidePos;

    void Start()
    {
        provisionalGUIMenuOnScreenPos = provisionalGUIMenu.anchoredPosition;
        hidePos = Vector2.right * 1000;
    }

    void Update()
    {
        if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                pause = false;
            }
            else
            {
                pause = true;
            }
        }
        if (pause)
        {
            PlayerController.instance.SetCanMove(false);
            if (provisionalGUIMenu.anchoredPosition != provisionalGUIMenuOnScreenPos) provisionalGUIMenu.anchoredPosition = provisionalGUIMenuOnScreenPos;
            if (Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        else
        {
            PlayerController.instance.SetCanMove(true);
            if (provisionalGUIMenu.anchoredPosition != hidePos) provisionalGUIMenu.anchoredPosition = hidePos;
        }
    }
}
