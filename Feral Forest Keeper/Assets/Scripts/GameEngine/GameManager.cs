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

    private Vector3 startCheckPointPosition;
    private Vector3 branchItem_InitLocation;
    private Vector3 swordItem_InitLocation;

    void Start()
    {
        actualRespawnCoolDown = respawnCoolDown;
        provisionalGUIMenuOnScreenPos = provisionalGUIMenu.anchoredPosition;
        hidePos = Vector2.down * 1000;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.transform.position = levelCheckPoint.transform.position;
        startCheckPointPosition = levelCheckPoint.transform.position;
        branchItem_InitLocation = branchItem.transform.position;
        swordItem_InitLocation = swordItem.transform.position;
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
            //Debug.Log("Entering Pause");
            PlayerController.instance.noInput = true;
            if (provisionalGUIMenu.anchoredPosition != provisionalGUIMenuOnScreenPos) provisionalGUIMenu.anchoredPosition = provisionalGUIMenuOnScreenPos;
            if (Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        else
        {
            //Debug.Log("Entering Quiting Pause");
            PlayerController.instance.noInput = false;
            if (provisionalGUIMenu.anchoredPosition != hidePos) provisionalGUIMenu.anchoredPosition = hidePos;
        }
        /////// PLAYER DEATH CONTROLER ////////
        ///By FALLING
        if (PlayerController.instance.actualPlayerLive > 0 && PlayerController.instance.deathByFall)
        {
            
            if (actualRespawnCoolDown == respawnCoolDown)
            {
                PlayerHitFeedbackController.instance.FallHit();
                PlayerController.instance.SetCanMove(false);
                CameraController.instance.SetActualBehavior(CameraController.Behavior.PLAYER_DEATH);
            }

            actualRespawnCoolDown -= Time.deltaTime;

            if (actualRespawnCoolDown <= 0)
            {
                PlayerController.instance.transform.position = levelCheckPoint.transform.position;
                if (PlayerController.instance.transform.position == levelCheckPoint.transform.position)
                {
                    //Debug.Log("After death, player on CheckPointPosition");
                    actualRespawnCoolDown = respawnCoolDown;
                    PlayerController.instance.actualPlayerLive--;
                    PlayerController.instance.deathByFall = false;
                    PlayerController.instance.SetCanMove(true);
                }
            }

            if (actualRespawnCoolDown < 0.1f)
            {
                CameraController.instance.SetActualBehavior(CameraController.Behavior.FOLLOW_PLAYER);
            }
            
        }
        else
        ///By DIYNG
        if (PlayerController.instance.actualPlayerLive <= 0)
        {
            //Debug.Log("Entering Death by Dying");
            PlayerController.instance.noInput = true;
            if (provisionalGUIMenu.anchoredPosition != provisionalGUIMenuOnScreenPos) provisionalGUIMenu.anchoredPosition = provisionalGUIMenuOnScreenPos;
            if (Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.E))
            {
                ResetGame();
            }
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
    private void ResetGame()
    {
        levelCheckPoint.transform.position = startCheckPointPosition;
        PlayerController.instance.transform.position = levelCheckPoint.transform.position;
        PlayerController.instance.SetCanMove(true);
        PlayerController.instance.deathByFall = false;
        PlayerController.instance.actualPlayerLive = PlayerController.instance.playerLive;

        if (!PlayerController.instance.startWithAllSkills)
        {
            PlayerAnimationController.instance.SetWeaponAnim(true);

            PlayerManager.instance.branchWeaponForAnimations.SetActive(false);
            PlayerManager.instance.leafWeaponForAnimations.SetActive(false);

            Player_GUI_System.instance.SetOnScreenBranchWeaponIcon(false);
            Player_GUI_System.instance.SetOnScreenLeafWeaponIcon(false);
            Player_GUI_System.instance.SetOnScreenStrenfthForestIcon(false);

            PlayerManager.instance.branchWeaponSlot = null;
            PlayerManager.instance.leafSwordSlot = null;
            PlayerManager.instance.powerGauntaletSlot = null;

            PlayerAnimationController.instance.SetWeaponAnim(false);
        }
        CameraController.instance.p_Camera.transform.position = PlayerController.instance.transform.position + CameraController.instance.cameraOffSet;

        if (branchItem.collected) branchItem.SetItem(branchItem_InitLocation, Vector3.up * 45);
        if (swordItem.collected) swordItem.SetItem(swordItem_InitLocation, Vector3.up * 45);
    }
}
