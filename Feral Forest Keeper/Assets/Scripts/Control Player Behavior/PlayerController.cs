﻿/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton

    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public GameObject objectToMove;
    public GameObject gatePotition;
    public GameObject playerRoot;
    public GameObject characterModel;
    public GameObject attackPivot;
    public GameObject shootPoint;
    public TrailRenderer attackTrail;
    public CharacterController p_controller;
    public MeleeAtack_PlayerState meleeState;
    public Movement_PlayerState movementState;
    public PushRock_State pushRockState;
    public PushLog_State pushLogState;
    public Dash_PlayerState dashState;
    public ShowWeapon_PlayerState showingWeaponState;
    public GameObject meleeWeaponRoot;
    public BoxCollider weaponCollider;

    public int playerLive;
    public int dashCharges;
    public float dashCooldownTime;
    public float dashCooldownSmoothRecover;
    public float movementSpeed;
    public float movementLockTargetSpeed;
    public float bushMovementSpeed;
    public float gravityForce;
    public float gravity = 0;
    public float smooth;
    public float actualPlayerLive;
    public float smoothAttackTrail;
    public float attackCoolDown;
    public int damagePlayer;
    public float deathHeight;
    public float hitCooldownTime;

    public float X_Input;
    public float Z_Input;
    public Vector3 movement;
    public Vector2 direction;
    public Vector3 modelForwardDirection;
    public bool canMove;
    public bool gettingHit;
    public bool attacking;
    public bool imGrounded;
    public bool dashing;
    public bool inSlowMovement;
    public State currentState;
    public float actualSpeedMultipler;
    public bool useWeaponColllider;
    public bool noInput;
    public bool no_X_Input;
    public bool no_Y_Input;
    public bool startWithAllSkills;
    public bool playerAlive;
    public bool pushing;
    public bool falling;
    public bool fallingToDeath;
    public bool showingWeapon;

    [SerializeField]
    public float dashCooldown;
    [SerializeField]
    private float dashChargesRemind;
    private Vector3 initFallingPosition;
    public Vector3 pushDirection;
    private float actualHitCooldown;
    public bool deathByFall;
    [SerializeField]
    private Vector3 showingWeaponInitForward;
    [SerializeField]
    private Vector3 showingDirection;
    public bool flyingDashFinished;
    private float checkDistanceOffset;

    protected StateMachine p_StateMachine = new StateMachine();

    void Start()
    {
        playerAlive = true;
        attackPivot.transform.localRotation = Quaternion.Euler(0, -60, 0);
        SetCanMove(true);
        ChangeState(movementState);
        actualSpeedMultipler = movementSpeed;
        dashing = false;
        dashCooldown = dashCooldownTime;
        actualPlayerLive = playerLive;
        initFallingPosition = this.transform.position;
        pushing = false;
        actualHitCooldown = hitCooldownTime;
        attackTrail.enabled = false;
        deathByFall = false;
        showingWeapon = false;
        flyingDashFinished = false;
        checkDistanceOffset = GenericSensUtilities.instance.DistanceBetween2Vectors(playerRoot.transform.position, characterModel.transform.position);
    }

    void Update()
    {
        CheckInputsConditions();
        ApplyGravity();
        SetDashCooldown();
        ItemsDetection();
        DoorsDetection();
        SanctuaryDetection();
        ColorRockDetection();
        SimonRockDetection();
        WoodSignDetection();

        if (gettingHit)
        {
            if (!PlayerAnimationController.instance.GetGettingHitAnimState())PlayerAnimationController.instance.SetGettingHitAnim(true);
            PlayerHitFeedbackController.instance.Hit();
            actualHitCooldown -= Time.deltaTime;
            if (actualHitCooldown <= 0)
            {
                PlayerAnimationController.instance.SetGettingHitAnim(false);
                actualHitCooldown = hitCooldownTime;
                gettingHit = false;
            }
        }
        /////INPUTS CHECK////
        //XboxGamePadKeyTest();

        imGrounded = p_controller.isGrounded; //Now each State setup imGrounded.
        if (imGrounded) initFallingPosition = this.transform.position;
        /////////END OF MOVEMENT LOGIC////////

        ///Check if Over Grass///
        if (!PlayerSensSystem.instance.CheckIfOverGrass() && currentState != pushLogState && currentState != pushRockState)
        {
            MovingInSlowZone(false);
        }

        if (actualPlayerLive == 0)
        {
            if (playerAlive != false) playerAlive = false;
        }
        if (actualPlayerLive > 0)
        {
            if (playerAlive != true) playerAlive = true;
        }
        //if (showingWeapon)
        //{
        //    showingWeaponCount += Time.deltaTime;
        //    if(GetCanMove())SetCanMove(false);
        //    float time = showingWeaponCount / 0.5f;
        //    playerRoot.transform.forward = Vector3.Lerp(showingWeaponInitForward, showingDirection, time);
        //    if (showingWeaponCount > 2.0f)
        //    {
        //        SetCanMove(true);
        //        showingWeapon = false;
        //        showingWeaponCount = 0;
        //    }
        //}
        p_StateMachine.ExecuteState();
        
    }
    private void DoorsDetection()
    {
        if (PlayerSensSystem.instance.nearestDoor != null && PlayerSensSystem.instance.nearestDoor.doorKeys.Length > 0)
        {
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestDoor.transform.position, characterModel.transform.position) < PlayerSensSystem.instance.nearestDoor.interactionDistance && PlayerManager.instance.FindKeysInInventory(PlayerSensSystem.instance.nearestDoor.doorKeys) && PlayerSensSystem.instance.nearestDoor.GetLockedActualState() == true)
            {
                //Debug.Log("HAVE THE AMOUNT OF KEYS?: " + PlayerManager.instance.FindKeysInInventory(PlayerSensSystem.instance.nearestDoor.doorKeys));
                Player_GUI_System.instance.SetOnScreenUnlockDoorIcon(true);
                if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                {
                    PlayerSensSystem.instance.nearestDoor.OpenDoor();
                }
            }
            else
            {
                Player_GUI_System.instance.SetOnScreenUnlockDoorIcon(false);
            }
        }
        else
        {
            Player_GUI_System.instance.SetOnScreenUnlockDoorIcon(false);
        }
    }
    private void ItemsDetection()
    {
        if (PlayerSensSystem.instance.nearestItem != null)
        {
            //Debug.Log("ItemNear");
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(characterModel.transform.position, PlayerSensSystem.instance.nearestItem.transform.position) < PlayerSensSystem.instance.nearestItem.interactionDistance)
            {
                switch (PlayerSensSystem.instance.nearestItem.itemType)
                {
                    case Item.ItemType.BRANCH_WEAPON:
                        Player_GUI_System.instance.SetOnScreenPickUpIcon(true);
                        if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                        {
                            PlayerManager.instance.AddItemToInventary(PlayerSensSystem.instance.nearestItem);
                            PlayerSensSystem.instance.nearestItem.CollectItem();
                            PlayerManager.instance.CheckIfHaveBranchWeaponItem();
                            ChangeState(showingWeaponState);
                        }
                        break;
                    case Item.ItemType.LEAF_WEAPON:
                        Player_GUI_System.instance.SetOnScreenPickUpIcon(true);
                        if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                        {
                            PlayerManager.instance.AddItemToInventary(PlayerSensSystem.instance.nearestItem);
                            PlayerSensSystem.instance.nearestItem.CollectItem();
                            PlayerManager.instance.CheckIfHaveLeafWeaponItem();
                            ChangeState(showingWeaponState);

                        }
                        break;
                    case Item.ItemType.POWER_GANTLET:
                        Player_GUI_System.instance.SetOnScreenPickUpIcon(true);
                        if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                        {
                            PlayerManager.instance.AddItemToInventary(PlayerSensSystem.instance.nearestItem);
                            PlayerSensSystem.instance.nearestItem.CollectItem();

                        }
                        break;
                    case Item.ItemType.KEY:
                        Player_GUI_System.instance.SetOnScreenPickUpIcon(true);
                        if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                        {
                            PlayerManager.instance.AddItemToInventary(PlayerSensSystem.instance.nearestItem);
                            PlayerSensSystem.instance.nearestItem.CollectItem();
                            PlayerManager.instance.CountKeys();
                            Player_GUI_System.instance.SetKeysCount(PlayerManager.instance.actualKeyQuantity);
                        }
                        break;
                    case Item.ItemType.LIVE_UP:
                        PlayerSensSystem.instance.nearestItem.CollectItem();
                        if (actualPlayerLive < playerLive && actualPlayerLive > 0 && !deathByFall)
                        {
                            actualPlayerLive++;
                            PlayerParticlesSystemController.instance.SetLiveUpFeedbackParticlesOnScene(transform.position + Vector3.up * 0.5f);
                        }
                        break;
                }
                //Player_GUI_System.instance.SetOnScreenPickUpIcon(true);
            }
            else
            {
                Player_GUI_System.instance.SetOnScreenPickUpIcon(false);
            }
        }
        else if (PlayerSensSystem.instance.nearestItem == null)
        {
            Player_GUI_System.instance.SetOnScreenPickUpIcon(false);
        }
    }
    private void SanctuaryDetection()
    {
        if (PlayerSensSystem.instance.nearestSanctuary != null) 
        {
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestSanctuary.transform.position, PlayerController.instance.transform.position) < 2)
            {
                Player_GUI_System.instance.SetOnScreenActivateSanctuaryIcon(true);
                if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                {
                    PlayerSensSystem.instance.nearestSanctuary.ActivateSanctuary();
                    Player_GUI_System.instance.SetOnScreenStrenfthForestIcon(true);
                }
            }
            else
            {
                Player_GUI_System.instance.SetOnScreenActivateSanctuaryIcon(false);
            }
        }
        else
        {
            Player_GUI_System.instance.SetOnScreenActivateSanctuaryIcon(false);
        }
    }
    private void ColorRockDetection()
    {
        if (PlayerSensSystem.instance.nearestColorRock != null)
        {
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestColorRock.transform.position, PlayerController.instance.transform.position) < 2)
            {
                Player_GUI_System.instance.SetOnScreenButtonBColor(true);

                if (Input.GetButtonDown("B") ||  Input.GetKeyDown(KeyCode.F))
                {
                    PlayerSensSystem.instance.nearestColorRock.ChangeColorRock();
                }
            }
            else
            {
                Player_GUI_System.instance.SetOnScreenButtonBColor(false);
            }
        }
        else
        {
            Player_GUI_System.instance.SetOnScreenButtonBColor(false);
        }
    }
    private void SimonRockDetection()
    {
        if (PlayerSensSystem.instance.nearestSimonRock != null)
        {
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestSimonRock.transform.position, PlayerController.instance.transform.position) < 2)
            {
                //Player_GUI_System.instance.SetOnScreenActivateSanctuaryIcon(true);
                Player_GUI_System.instance.SetOnScreenButtonBSimon(true);

                if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                {
                    PlayerSensSystem.instance.nearestSimonRock.InteractWithSimonRock();
                }
            }
            else
            {
                Player_GUI_System.instance.SetOnScreenButtonBSimon(false);
            }
        }
        else
        {
            Player_GUI_System.instance.SetOnScreenButtonBSimon(false);
        }
    }
    private void WoodSignDetection()
    {
        if (PlayerSensSystem.instance.nearestWoodSign != null)
        {
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerSensSystem.instance.nearestWoodSign.transform.position, PlayerController.instance.transform.position) < 2)
            {
                //Player_GUI_System.instance.SetOnScreenActivateSanctuaryIcon(true);
                Player_GUI_System.instance.SetOnScreenButtonBWoodSign(true);

                if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.F))
                {
                    PlayerSensSystem.instance.nearestWoodSign.ActivateLetterScript();
                }
            }
            else
            {
                Player_GUI_System.instance.SetOnScreenButtonBWoodSign(false);
            }
        }
        else
        {
            Player_GUI_System.instance.SetOnScreenButtonBWoodSign(false);
        }
    }
    public void ApplyGravity()
    {
        if (!imGrounded  && !dashing/*flyingDashFinished/*&& currentState != pushRockState*/)
        {
            gravity += Mathf.Exp(gravityForce);

            movement.y = movement.y - (gravity * Time.deltaTime);
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(initFallingPosition, transform.position) > 0.5f)
            {
                if (PlayerSensSystem.instance.CheckGroundDistance() >= deathHeight)
                {
                    fallingToDeath = true;
                    deathByFall = true;
                }
                else
                if (PlayerSensSystem.instance.CheckGroundDistance() > checkDistanceOffset + 0.5f)
                {
                    falling = true;
                }
            }
            
            //if (PlayerSensSystem.instance.CheckGroundDistance() > 0.5f)
            //{
            //    //if (!dashing)
            //    //{
            //    //    falling = true;
            //    //}
            //    if (PlayerSensSystem.instance.CheckGroundDistance() >= deathHeight)
            //    {
            //        fallingToDeath = true;
            //        deathByFall = true;
            //    }
            //    else
            //    {
            //        falling = true;
            //    }
            //    //Debug.Log("Player almost touching the ground");

            //    //if (GenericSensUtilities.instance.DistanceBetween2Vectors(initFallingPosition, this.transform.position) > deathHeight && actualPlayerLive > 0)
            //    //{
            //    //    deathByFall = true;
            //    //}
            //}
            //else if (PlayerSensSystem.instance.CheckGroundDistance() < 0.5f)
            //{
            //    falling = false;
            //    fallingToDeath = false;
            //}
        }
        else
        {
            if (gravity > 0 || gravity < 0)
                gravity = 0;
            falling = false;
            fallingToDeath = false;
        }
    }
    private void CheckInputsConditions()
    {
        CheckInputs();

        if (PlayerManager.instance.branchWeaponSlot != null || PlayerManager.instance.leafSwordSlot != null)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("X") && !attacking && !dashing && !gettingHit && !falling && !fallingToDeath)
            {
                ChangeState(meleeState);
            }
        }
        
        if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Space) && !attacking && imGrounded && !gettingHit)
        {
            if (!inSlowMovement && dashCooldown == dashCooldownTime) ChangeState(dashState);
        }
        if (PlayerManager.instance.powerGauntaletSlot != null)
        {
            if (!Input.GetButton("RB") || !Input.GetKey(KeyCode.E) && PlayerSensSystem.instance.nearestRock != null && GenericSensUtilities.instance.DistanceBetween2Vectors(characterModel.transform.position, PlayerSensSystem.instance.nearestRock.transform.position) < PlayerSensSystem.instance.nearestRock.attachDistance
            || PlayerSensSystem.instance.nearestLog != null && GenericSensUtilities.instance.DistanceBetween2Vectors(characterModel.transform.position, PlayerSensSystem.instance.nearestLog.transform.position) < PlayerSensSystem.instance.nearestLog.attachDistance)
            {
                /// PUSHING ROCKS ////
                if (PlayerSensSystem.instance.nearestRock != null && GenericSensUtilities.instance.DistanceBetween2Vectors(characterModel.transform.position, PlayerSensSystem.instance.nearestRock.transform.position) < PlayerSensSystem.instance.nearestRock.attachDistance
                    && characterModel.transform.position.y > PlayerSensSystem.instance.nearestRock.rockBody.transform.position.y - PlayerSensSystem.instance.nearestRock.rockBody.GetComponent<MeshRenderer>().bounds.extents.y * 0.8f
                    && characterModel.transform.position.y < PlayerSensSystem.instance.nearestRock.rockBody.transform.position.y + PlayerSensSystem.instance.nearestRock.rockBody.GetComponent<MeshRenderer>().bounds.extents.y * 0.8f)
                {
                    PlayerSensSystem.instance.nearestRock.CheckSideToPush();
                    if (Input.GetButtonDown("RB") || Input.GetKeyDown(KeyCode.E))
                    {
                        //Debug.Log("Enter PushRockState");
                        //PlayerSensSystem.instance.nearestRock.CheckSideToPush();
                        if (PlayerSensSystem.instance.nearestRock.attchAviable)
                        {
                            //Debug.Log("Enter PushRockStateFase2");
                            ChangeState(pushRockState);
                        }
                    }
                    if (PlayerSensSystem.instance.nearestRock.attchAviable && !PlayerSensSystem.instance.nearestRock.CheckIfFalling())
                    {
                        Player_GUI_System.instance.SetOnScreenPushIcon(true);
                    }
                    else
                    {
                        Player_GUI_System.instance.SetOnScreenPushIcon(false);
                    }
                }

                /// PUSHING LOGS ////
                if (PlayerSensSystem.instance.nearestLog != null && GenericSensUtilities.instance.DistanceBetween2Vectors(characterModel.transform.position, PlayerSensSystem.instance.nearestLog.transform.position) < PlayerSensSystem.instance.nearestLog.attachDistance
                    && characterModel.transform.position.y > PlayerSensSystem.instance.nearestLog.logBody.transform.position.y - PlayerSensSystem.instance.nearestLog.logBody.GetComponent<MeshRenderer>().bounds.extents.y * 0.8f
                    && characterModel.transform.position.y < PlayerSensSystem.instance.nearestLog.logBody.transform.position.y + PlayerSensSystem.instance.nearestLog.logBody.GetComponent<MeshRenderer>().bounds.extents.y * 0.8f)
                {
                    PlayerSensSystem.instance.nearestLog.CheckSideToPush();
                    if (Input.GetButtonDown("RB") || Input.GetKeyDown(KeyCode.E))
                    {
                        //Debug.Log("Enter PushRockState");
                        //PlayerSensSystem.instance.nearestLog.CheckSideToPush();
                        if (PlayerSensSystem.instance.nearestLog.attchAviable)
                        {
                            ChangeState(pushLogState);
                        }
                    }
                    if (PlayerSensSystem.instance.nearestLog.attchAviable && !PlayerSensSystem.instance.nearestLog.CheckIfFalling())
                    {
                        Player_GUI_System.instance.SetOnScreenPushIcon(true);
                    }
                    else
                    {
                        Player_GUI_System.instance.SetOnScreenPushIcon(false);
                    }
                }
            }
            else
            {
                //Debug.Log("HidingPushIcon3");
                Player_GUI_System.instance.SetOnScreenPushIcon(false);
            }
        }
        
    }
    private void SetDashCooldown()
    {
        if (dashCooldown < dashCooldownTime * 0.97f)
        {
            dashCooldown = Mathf.Lerp(dashCooldown, dashCooldownTime, dashCooldownSmoothRecover * Time.deltaTime);
        }
        else
        {
            if (dashCooldown != dashCooldownTime) dashCooldown = dashCooldownTime;
        }
    }
    public void CheckInputs()
    {
        if (noInput)
        {
            X_Input = 0;
            Z_Input = 0;
        }
        else
        {
            X_Input = Input.GetAxis("Horizontal");
            Z_Input = Input.GetAxis("Vertical");
        }
        if (no_X_Input)
        {
            X_Input = 0;
        }
        if (no_Y_Input)
        {
            Z_Input = 0;
        }

        if (canMove && !gettingHit && !attacking && imGrounded)
        {
            direction.x = X_Input;
            direction.y = Z_Input;
        }
        else
        {
            direction.x = Mathf.Lerp(direction.x, 0, Time.deltaTime * smooth);
            direction.y = Mathf.Lerp(direction.y, 0, Time.deltaTime * smooth);
        }

        movement.x = direction.x;
        movement.z = direction.y;
        movement = movement.normalized;
        movement.x *= actualSpeedMultipler;
        movement.z *= actualSpeedMultipler;

        modelForwardDirection.x = direction.x;
        modelForwardDirection.z = direction.y;
    }
    public void OnTriggerStay(Collider other)
    {
        LevelGates gate = other.gameObject.GetComponent<LevelGates>();
        if (gate != null)
        {
            //gate.UseGate();
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("B"))
            {
                if (GenericSensUtilities.instance.DistanceBetween2Vectors(gate.transform.position, transform.position) < 1.0f) gate.UseGate();
            }
        }
        //if (other.tag == "DeathZone" && playerAlive)
        //{
        //    if (playerAlive)
        //    actualPlayerLive = 0;
        //}
        StaticCameraZone scz = other.gameObject.GetComponent<StaticCameraZone>();
        if (scz != null)
        {
            if (CameraController.instance.GetActualBehavior() != CameraController.Behavior.STATIC_CAMERA_ZONE)
            {
                CameraController.instance.StaticCameraZone(scz.cameraTarget, scz.cameraDistance);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        StaticCameraZone scz = other.gameObject.GetComponent<StaticCameraZone>();
        if (scz != null)
        {
            if (CameraController.instance.GetActualBehavior() != CameraController.Behavior.TRANSITION_TO_FOLLOW) CameraController.instance.SetActualBehavior(CameraController.Behavior.TRANSITION_TO_FOLLOW);
        }
    }

    public void SetCanMove(bool b)
    {
        canMove = b;
    }
    public bool GetCanMove()
    {
        return canMove;
    }
    //public void RangedAttack()
    //{
    //    dart.Shoot(shootPoint.transform.position, playerRoot.transform.forward);
    //}
    public void MoveBetweenGates(GameObject _gate)
    {
        p_controller.enabled = false;
        //Debug.Log("Player Transform Position: " + transform.position);
        //Debug.Log("Target Gate Position: " + _gate.transform.position);
        this.transform.position = _gate.transform.position;
        //Debug.Log("Player Transform Position Once Moved: " + transform.position);
        playerRoot.transform.forward = _gate.transform.forward;
        p_controller.enabled = true;
    }
    public void GetDamage(float damage)
    {
        if (!gettingHit)
        {
            if (actualPlayerLive > 0)
            {
                actualPlayerLive -= damage;
                gettingHit = true;
            }
        }
    }
    public void MovingInSlowZone(bool b)
    {
        if (b)
        {
            actualSpeedMultipler = bushMovementSpeed;
        }
        else
        {
            actualSpeedMultipler = movementSpeed;
        }
        inSlowMovement = b;
    }
    public void ChangeState(State state)
    {
        p_StateMachine.ChangeState(state);

        currentState = p_StateMachine.currentState;
    }

    ///////GAMEPAD TEST//////////
    private void XboxGamePadKeyTest()
    {
        if (Input.GetButtonDown("A"))
        {
            Debug.Log("A");
        }
        if (Input.GetButtonDown("B"))
        {
            Debug.Log("B");
        }
        if (Input.GetButtonDown("X"))
        {
            Debug.Log("X");
        }
        if (Input.GetButtonDown("Y"))
        {
            Debug.Log("Y");
        }
        if (Input.GetButtonDown("LB"))
        {
            Debug.Log("LB");
        }
        if (Input.GetButtonDown("RB"))
        {
            Debug.Log("RB");
        }
        if (Input.GetAxis("LT") < 0)
        {
            Debug.Log("LT: " + Input.GetAxis("LT"));
        }
        if (Input.GetAxis("RT") > 0)
        {
            Debug.Log("RT :" + Input.GetAxis("RT"));
        }
    }
}
