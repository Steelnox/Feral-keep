/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class DartBehavior : MonoBehaviour
{
    public float speed;
    public float lifeTimeDistance;
    public GameObject dartBody;
    public Vector3 hidePos;
    public TrailRenderer myTrail;

    private Vector3 direction;
    private bool shooted;
    private Vector3 originShootPos;

    void Start ()
    {
        transform.position = hidePos;
        myTrail.enabled = false;
	}
	
	void Update ()
    {
        if (shooted)
        {
            transform.position += direction * speed;
            if (!myTrail.enabled) myTrail.enabled = true;
            CheckLifeTime();
        }
	}
    private void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        dartBody.transform.forward = direction.normalized;
    }
    private void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        
    }
    private void SetShooted(bool b)
    {
        shooted = b;
    }
    private void CheckLifeTime()
    {
        if ((originShootPos - transform.position).magnitude > lifeTimeDistance)
        {
            ResetDart();
        }
    }
    private void ResetDart()
    {
        if (myTrail.enabled) myTrail.enabled = false;
        SetShooted(false);
        SetPosition(hidePos);
    }
    public void Shoot(Vector3 position, Vector3 direction)
    {
        if (!IsShooted())
        {
            originShootPos = position;
            SetPosition(position);
            SetDirection(direction);
            SetShooted(true);
        }    
    }
    /*public void OnTriggerExit(Collider col)
    {
        SetShooted(false);
        SetPosition(hidePos);
        if (col != null)
        {
            SetShooted(false);
            SetPosition(hidePos);
            if (col.gameObject.tag == "Scenario")
            {
                SetShooted(false);
                SetPosition(hidePos);
            }
        }
        
    }*/
    public bool IsShooted()
    {
        return shooted;
    }
}
