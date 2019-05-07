using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensSystem : MonoBehaviour
{

    #region Singleton

    public static PlayerSensSystem instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public float sensRange;
    public SphereCollider colliderSens;
    public Enemy nearestEnemy;
    public MovableRocks nearestRock;
    public MovableLog nearestLog;
    public Item nearestItem;
    public OpenableDoors nearestDoor;
    public bool overGrass;

    [SerializeField]
    public List<Enemy> onRangeEnemyList;
    public List<MovableRocks> movableRocksList;
    public List<MovableLog> movableLogsList;
    public List<BushGrass_Behavior> grassesList;
    public List<Item> itemsList;
    public List<OpenableDoors> doorsList;

    void Start()
    {
        colliderSens.radius = sensRange;
    }

    void Update()
    {
        GetNearestEnemy();
        GetNearestRock();
        GetNearestLog();
        GetNearestItem();
        GetNearestDoor();
    }     

    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                if (!FindSameEnemyOnList(e))
                {
                    onRangeEnemyList.Add(e);
                }
            }
            MovableRocks rock = other.GetComponent<MovableRocks>();
            if (rock != null)
            {
                if (!FindSameMovableRockOnList(rock))
                {
                    movableRocksList.Add(rock);
                }
            }
            MovableLog log = other.GetComponent<MovableLog>();
            if (log != null)
            {
                if (!FindSameMovableLogOnList(log))
                {
                    movableLogsList.Add(log);
                }
            }
            BushGrass_Behavior grass = other.GetComponent<BushGrass_Behavior>();
            if (grass != null)
            {
                if (!FindSameGrassOnList(grass))
                {
                    grassesList.Add(grass);
                }
            }
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                if (!FindSameItemOnList(item))
                {
                    itemsList.Add(item);
                }
            }
            OpenableDoors door = other.GetComponent<OpenableDoors>();
            if (door!= null)
            {
                if (!FindSameDoorOnList(door))
                {
                    doorsList.Add(door);
                }
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other != null)
        {

            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                if (!FindSameEnemyOnList(e))
                {
                    onRangeEnemyList.Add(e);
                }
            }
            MovableRocks rock = other.GetComponent<MovableRocks>();
            if (rock != null)
            {
                if (!FindSameMovableRockOnList(rock))
                {
                    movableRocksList.Add(rock);
                }
            }
            MovableLog log = other.GetComponent<MovableLog>();
            if (log != null)
            {
                if (!FindSameMovableLogOnList(log))
                {
                    movableLogsList.Add(log);
                }
            }
            BushGrass_Behavior grass = other.GetComponent<BushGrass_Behavior>();
            if (grass != null)
            {
                if (!FindSameGrassOnList(grass))
                {
                    grassesList.Add(grass);
                }
            }
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                if (!FindSameItemOnList(item))
                {
                    itemsList.Add(item);
                }
            }
            OpenableDoors door = other.GetComponent<OpenableDoors>();
            if (door != null)
            {
                if (!FindSameDoorOnList(door))
                {
                    doorsList.Add(door);
                }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                foreach (Enemy e in onRangeEnemyList)
                {
                    if (enemy == e)
                    {
                        onRangeEnemyList.Remove(e);
                        return;
                    }
                }
            }
            MovableRocks rock = other.GetComponent<MovableRocks>();
            if (rock != null)
            {
                foreach (MovableRocks r in movableRocksList)
                {
                    if (rock == r)
                    {
                        movableRocksList.Remove(r);
                        return;
                    }
                }
            }
            MovableLog log = other.GetComponent<MovableLog>();
            if (log != null)
            {
                foreach (MovableLog l in movableLogsList)
                {
                    if (log == l)
                    {
                        movableLogsList.Remove(l);
                        return;
                    }
                }
            }
            BushGrass_Behavior grass = other.GetComponent<BushGrass_Behavior>();
            if (grass != null)
            {
                foreach (BushGrass_Behavior g in grassesList)
                {
                    if (grass == g)
                    {
                        grassesList.Remove(g);
                        return;
                    }
                }
            }
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                foreach (Item i in itemsList)
                {
                    if (item == i)
                    {
                        itemsList.Remove(i);
                        return;
                    }
                }
            }
            OpenableDoors door = other.GetComponent<OpenableDoors>();
            if (door != null)
            {
                foreach (OpenableDoors d in doorsList)
                {
                    if (door == d)
                    {
                        doorsList.Remove(d);
                        return;
                    }
                }
            }
        }
    }

    public void GetNearestEnemy()
    {
        Enemy enemy = null;
        if (onRangeEnemyList.Count > 0)
        {
            foreach (Enemy e in onRangeEnemyList)
            {
                if (enemy == null)
                {
                    enemy = e;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, e.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, enemy.transform.position))
                    {
                        enemy = e;
                    }
                }
            }
        }
        
        nearestEnemy = enemy;
    }
    public void GetNearestRock()
    {
        MovableRocks rock = null;

        if (movableRocksList.Count > 0)
        {
            foreach (MovableRocks r in movableRocksList)
            {
                if (rock == null)
                {
                    rock = r;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, r.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, rock.transform.position))
                    {
                        rock = r;
                    }
                }
            }
        }

        nearestRock = rock;
    }
    public void GetNearestLog()
    {
        MovableLog log = null;

        if (movableRocksList.Count > 0)
        {
            foreach (MovableLog l in movableLogsList)
            {
                if (log == null)
                {
                    log = l;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, l.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, log.transform.position))
                    {
                        log = l;
                    }
                }
            }
        }

        nearestLog = log;
    }
    public void GetNearestItem()
    {
        Item item = null;

        if (itemsList.Count > 0)
        {
            foreach (Item i in itemsList)
            {
                if (item == null)
                {
                    item = i;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, i.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, item.transform.position))
                    {
                        item = i;
                    }
                }
            }
        }

        nearestItem = item;
    }
    public void GetNearestDoor()
    {
        OpenableDoors door = null;

        if (doorsList.Count > 0)
        {
            foreach (OpenableDoors d in doorsList)
            {
                if (door == null)
                {
                    door = d;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, d.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, door.transform.position))
                    {
                        door = d;
                    }
                }
            }
        }

        nearestDoor = door;
    }
    public bool CheckIfOverGrass()
    {
        foreach (BushGrass_Behavior g in grassesList)
        {
            if (GenericSensUtilities.instance.DistanceBetween2Vectors(PlayerController.instance.playerRoot.transform.position, g.transform.position) < g.bushContactDistance - 0.1f
                && PlayerController.instance.playerRoot.transform.position.y >= g.transform.position.y - 0.2f
                && PlayerController.instance.playerRoot.transform.position.y < g.transform.position.y + 0.2f)
            {
                return true;
            }
        }
        return false;
    }
    private void UpdateEnemyList()
    {
        foreach(Enemy e in onRangeEnemyList)
        {
            
        }
    }
    private bool FindSameEnemyOnList(Enemy _enemy)
    {
        foreach (Enemy e in onRangeEnemyList)
        {
            if (e == _enemy) return true;
        }
        return false;
    }
    private bool FindSameMovableRockOnList(MovableRocks _mRock)
    {
        foreach (MovableRocks e in movableRocksList)
        {
            if (e == _mRock) return true;
        }
        return false;
    }
    private bool FindSameMovableLogOnList(MovableLog _mLog)
    {
        foreach (MovableLog l in movableLogsList)
        {
            if (l == _mLog) return true;
        }
        return false;
    }
    private bool FindSameGrassOnList(BushGrass_Behavior _mGrass)
    {
        foreach (BushGrass_Behavior g in grassesList)
        {
            if (g == _mGrass) return true;
        }
        return false;
    }
    private bool FindSameItemOnList(Item _mItem)
    {
        foreach (Item i in itemsList)
        {
            if (i == _mItem) return true;
        }
        return false;
    }
    private bool FindSameDoorOnList(OpenableDoors _mDoor)
    {
        foreach (OpenableDoors d in doorsList)
        {
            if (d == _mDoor) return true;
        }
        return false;
    }
    public float CheckGroundDistance()
    {
        float dis;
        Ray ray = new Ray(PlayerController.instance.characterModel.transform.position, Vector3.down);
        RaycastHit rayHit;
        Physics.Raycast(ray, out rayHit);

        if (rayHit.collider != null)
        {
            dis = GenericSensUtilities.instance.DistanceBetween2Vectors(transform.position, rayHit.point);
            //Debug.Log("Ground Distance = " + dis);
            return dis;
        }
        return 0;
    }
    public BushGrass_Behavior FindNearestGrassBushToPushingRock(MovableRocks _rock)
    {
        BushGrass_Behavior bushGrass = null;
        if (grassesList.Count > 0)
        {
            foreach (BushGrass_Behavior b in grassesList)
            {
                if (bushGrass == null)
                {
                    bushGrass = b;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(_rock.transform.position, b.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(_rock.transform.position, bushGrass.transform.position))
                    {
                        bushGrass = b;
                    }
                }
            }
        }
        return bushGrass;
    }
    public BushGrass_Behavior FindNearestGrassBushToPushingLog(MovableLog _log)
    {
        BushGrass_Behavior bushGrass = null;
        if (grassesList.Count > 0)
        {
            foreach (BushGrass_Behavior b in grassesList)
            {
                if (bushGrass == null)
                {
                    bushGrass = b;
                }
                else
                {
                    if (GenericSensUtilities.instance.DistanceBetween2Vectors(_log.transform.position, b.transform.position) < GenericSensUtilities.instance.DistanceBetween2Vectors(_log.transform.position, bushGrass.transform.position))
                    {
                        bushGrass = b;
                    }
                }
            }
        }
        return bushGrass;
    }
}
