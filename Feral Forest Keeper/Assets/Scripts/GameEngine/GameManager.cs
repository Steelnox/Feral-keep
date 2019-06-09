using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public Item branchItem;
    public Item swordItem;
    public Item gantletItem;
    public bool pause;
    public RectTransform provisionalGUIMenu;
    public Item[] liveUpPool;
    private Vector2 provisionalGUIMenuOnScreenPos;
    [HideInInspector]
    public Vector2 hidePos;

    public Enemy[] enemiesPool;
    public Enemy[] enemiesEventPool;
    public List<Projectile> projectilePool;
    public float respawnCoolDown;
    [SerializeField]
    private float actualRespawnCoolDown;
    public GameObject levelCheckPoint;

    void Start()
    {
        actualRespawnCoolDown = respawnCoolDown;
        provisionalGUIMenuOnScreenPos = provisionalGUIMenu.anchoredPosition;
        hidePos = Vector2.down * 1000;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.transform.position = levelCheckPoint.transform.position;
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
        /////// PLAYER DEATH CONTROLER ////////
        ///By FALLING
        if (PlayerController.instance.deathByFall)
        {
            if (actualRespawnCoolDown == respawnCoolDown)
            {
                PlayerHitFeedbackController.instance.FallHit();
                //PlayerController.instance.p_controller.enabled = false;
                PlayerController.instance.SetCanMove(false);
                //PlayerController.instance.transform.position = hidePos;
                CameraController.instance.SetActualBehavior(CameraController.Behavior.PLAYER_DEATH);
            }
            actualRespawnCoolDown -= Time.deltaTime;
            if (actualRespawnCoolDown <= 0)
            {
                //PlayerController.instance.p_controller.enabled = false;
                //PlayerController.instance.SetCanMove(false);
                PlayerController.instance.transform.position = levelCheckPoint.transform.position;
                //CameraController.instance.SetActualBehavior(CameraController.Behavior.PLAYER_DEATH);
                if (PlayerController.instance.transform.position == levelCheckPoint.transform.position)
                {
                    Debug.Log("After death, player on CheckPointPosition");
                    //PlayerController.instance.actualPlayerLive = PlayerController.instance.playerLive;
                    //PlayerController.instance.playerAlive = true;
                    //PlayerAnimationController.instance.SetDeathByFall(false);
                    actualRespawnCoolDown = respawnCoolDown;
                    PlayerController.instance.actualPlayerLive--;
                    PlayerController.instance.SetCanMove(true);
                    //PlayerController.instance.p_controller.enabled = true;
                    PlayerController.instance.deathByFall = false;
                    //CameraController.instance.SetActualBehavior(CameraController.Behavior.FOLLOW_PLAYER);
                }
            }  
            if (actualRespawnCoolDown < 0.1f)
            {
                CameraController.instance.SetActualBehavior(CameraController.Behavior.FOLLOW_PLAYER);
            }
        }
        ///By DIYNG
        if (PlayerController.instance.actualPlayerLive <= 0)
        {
            
        }
    }
    public Item GetRandomLiveUpItem()
    {
        int random = Random.Range(0, liveUpPool.Length);
        if (liveUpPool[random].collected)
        {
            //liveUpPool[random].collected = false;
            return liveUpPool[random];
        }
        else
        {
            return null;
        }
    }

    public Projectile GetProjectileNotActive()
    {
        for (int i = 0; i < projectilePool.Count; i++)
        {

            if(projectilePool[i].activated == false)
            {
                projectilePool[i].activated = true;
                return projectilePool[i];
            }    
        }

        return null;
    }
    public float GetActualRespawnCooldown()
    {
        return actualRespawnCoolDown;
    }
}
