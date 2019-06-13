using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class TargetLockVisualFeedbackSystem : MonoBehaviour
//{
//    #region Singleton

//    public static TargetLockVisualFeedbackSystem instance;

//    private void Awake()
//    {
//        if (instance == null) instance = this;
//        if (instance != this) Destroy(this);
//    }

//    #endregion

//    public enum TargetFeedback_Mode {NONE_MODE, NEAREST_TARGET, LOCKED_TARGET};

//    public TargetFeedback_Mode actualMode;

//    public GameObject mark;
//    public Vector3 markOffset;
//    public Color lockedTargetMarkColor;
//    public Color nearestEnemyMarkColor;

//    private Renderer markRender;

//    void Start()
//    {
//        actualMode = TargetFeedback_Mode.NONE_MODE;
//        markRender = mark.gameObject.GetComponent<Renderer>();

//        //Set the main Color of the Material to green
//        markRender.material.shader = Shader.Find("_Color");
//        markRender.material.SetColor("_Color", nearestEnemyMarkColor);

//        //Find the Specular shader and change its Color to red
//        markRender.material.shader = Shader.Find("Specular");
//        markRender.material.SetColor("_SpecColor", nearestEnemyMarkColor);

//    }

//    void Update()
//    {
//        switch(actualMode)
//        {
//            case TargetFeedback_Mode.NONE_MODE:
//                break;
//            case TargetFeedback_Mode.NEAREST_TARGET:
//                MarkNearestTarget();
//                break;
//            case TargetFeedback_Mode.LOCKED_TARGET:
//                MarkLockedTarget();
//                break;
//            default:
//                break;
//        }
//    }
//    public void ChangeMode (TargetFeedback_Mode newMode)
//    {
//        //EXIT MODE
//        switch (actualMode)
//        {
//            case TargetFeedback_Mode.NONE_MODE:
//                mark.transform.position = Vector3.down * 1000;
//                break;
//            case TargetFeedback_Mode.NEAREST_TARGET:
//                mark.transform.position = Vector3.down * 1000;
//                break;
//            case TargetFeedback_Mode.LOCKED_TARGET:
//                mark.transform.position = Vector3.down * 1000;
//                break;
//            default:
//                break;
//        }

//        //ENTER MODE
//        switch (newMode)
//        {
//            case TargetFeedback_Mode.NONE_MODE:
//                mark.transform.position = Vector3.down * 1000;
//                break;
//            case TargetFeedback_Mode.NEAREST_TARGET:
//                //Set the main Color of the Material to green
//                markRender.material.shader = Shader.Find("_Color");
//                markRender.material.SetColor("_Color", nearestEnemyMarkColor);

//                //Find the Specular shader and change its Color to red
//                markRender.material.shader = Shader.Find("Specular");
//                markRender.material.SetColor("_SpecColor", nearestEnemyMarkColor);
//                break;
//            case TargetFeedback_Mode.LOCKED_TARGET:
//                //Set the main Color of the Material to green
//                markRender.material.shader = Shader.Find("_Color");
//                markRender.material.SetColor("_Color", lockedTargetMarkColor);

//                //Find the Specular shader and change its Color to red
//                markRender.material.shader = Shader.Find("Specular");
//                markRender.material.SetColor("_SpecColor", nearestEnemyMarkColor);
//                break;
//            default:
//                break;
//        }

//        actualMode = newMode;
//    }
//    private void MarkNearestTarget ()
//    {
//        if (PlayerSensSystem.instance.nearestEnemy != null) mark.transform.position = PlayerSensSystem.instance.nearestEnemy.transform.position + markOffset;
//    }
//    private void MarkLockedTarget()
//    {
//        mark.transform.position = PlayerController.instance.targetLocked.transform.position + markOffset;
//    }
//}
