using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { BRANCH_WEAPON, LEAF_WEAPON, POWER_GANTLET, KEY, LIVE_UP};

    public ItemType itemType;
    public float interactionDistance;

    public Vector3 hidePos;
    public bool collected;
    private Vector3 onScenPoition;
    private AnimationCurve curve;
    private ParticlesCompositeSystem myParticles;

    public void Start()
    {
        onScenPoition = transform.position;
        curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        curve.postWrapMode = WrapMode.PingPong;
        if (!collected)
        {
            myParticles = ParticlesFeedback_Control.instance.GetNOActiveCompoisteOnList(ParticlesFeedback_Control.instance.itemsScrollUpSparksParticlesPOOL);
            myParticles.PlayComposition(transform.position);
        }
    }
    public void Update()
    {
        if (!collected)
        {
            transform.position = Vector3.Lerp(onScenPoition + Vector3.down * 0.1f, onScenPoition + Vector3.up * 0.1f, curve.Evaluate(Time.time));
        }
    }
    public void CollectItem()
    {
        collected = true;
        transform.position = hidePos;
        myParticles.HideComposition();
        myParticles = null;
    }
    public void SetItem(Vector3 pos, Vector3 rot)
    {
        if (collected == true) collected = false;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
        onScenPoition = pos;
        myParticles = ParticlesFeedback_Control.instance.GetNOActiveCompoisteOnList(ParticlesFeedback_Control.instance.itemsScrollUpSparksParticlesPOOL);
        if (myParticles != null) myParticles.PlayComposition(transform.position);
    }
}
