using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonController : MonoBehaviour
{
    public List<GameObject> colorRocks;
    public List<GameObject> enemySimonList;
    public List<int> numList;

    public List<Vector3> enemyPositionList;

    public Switch_Behavior switch_Act;

    public GameObject key;

    public bool puzzleActivated;
    private bool randomPatronActivated;
    private bool randomDone;
    public bool sequenceDone;
    public bool resetDone;
    public bool fail;
    public bool win;
    public bool spawnkey;

    public int i;
    public int randomNumber;
    public int timesGood;
    public int sequenceCount;
    public int countRandom;

    public float timeColored;
    public float timer;

    #region Singleton

    public static SimonController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    void Start()
    {
        puzzleActivated = false;
        randomDone = false;
        resetDone = true;
        randomPatronActivated = false;
        i = 0;
        timesGood = 0;
        timer = 0;
        sequenceCount = 0;
        fail = false;
        win = false;
        spawnkey = false;

        key.SetActive(false);

        foreach (GameObject enemy in enemySimonList)
        {
            enemyPositionList.Add(enemy.transform.position);
            enemy.SetActive(false);
        }
    }

    void Update()
    {
        if (!win)
        {
            if (puzzleActivated && !randomDone)
            {
                GenerateRandomPatron();
            }
            if (puzzleActivated && randomDone && !sequenceDone)
            {
                DoSequence();
            }
            if (puzzleActivated && randomDone && sequenceDone && i >= numList.Count)
            {
                timesGood++;

                if (timesGood < 3)
                {
                    AddNewColor();
                }

                else
                {
                    win = true;
                }
            }

            if (switch_Act.holdedSwitched)
            {
                puzzleActivated = true;
                resetDone = false;
            }

            else
            {
                if (!resetDone)
                {
                    puzzleActivated = false;
                    randomDone = false;
                    sequenceDone = false;
                    countRandom = 0;
                    timesGood = 0;
                    timer = 0;
                    i = 0;
                    sequenceCount = 0;

                    ChangeColorBlack();

                    numList.Clear();

                    resetDone = true;
                }
            }

            if (fail && puzzleActivated)
            {
                for (int count = 0; count < enemySimonList.Count; count++)
                {
                    if (enemySimonList[count].GetComponent<Enemy>().currentHealth <= 0)
                    {
                        sequenceDone = false;
                        fail = false;
                    }
                }
            }
        }
        else
        {
            if (!spawnkey)
            {
                key.SetActive(true);
                spawnkey = true;
            }
            
        }
    }

    public void CheckSimon(GameObject selectedRock)
    {
        selectedRock.GetComponent<SimonRock>().materialRock.color =  selectedRock.GetComponent<SimonRock>().colorRock;

        if (i < numList.Count)
        {
            if (selectedRock == colorRocks[numList[i]])
            {
                i++;
            }
            else
            {
                i = 0;
                sequenceCount = 0;
                fail = true;
                for (int count = 0; count < enemySimonList.Count; count++)
                {

                    enemySimonList[count].GetComponent<Enemy>().currentHealth = 3;
                    enemySimonList[count].GetComponent<Enemy>().HealthBar.transform.localScale = new Vector3(1,1,1);
                    enemySimonList[count].GetComponent<Enemy>().HealthBar.SetActive(false);


                    enemySimonList[count].transform.position = enemyPositionList[count];
                    enemySimonList[count].SetActive(true);


                }
            }
        }



        Invoke("ChangeColorBlack", 0.2f);
    }


    private void GenerateRandomPatron()
    {
        if(countRandom < 3)
        {
            randomNumber = Random.Range(0, colorRocks.Count);
            if(countRandom > 0)
            {
                while (randomNumber == numList[countRandom - 1])
                {
                    randomNumber = Random.Range(0, colorRocks.Count);
                }
            }
            numList.Add(randomNumber);

            countRandom++;
        }
        else
        {
            randomDone = true;
        }
    }

    private void AddNewColor()
    {
        randomNumber = Random.Range(0, colorRocks.Count);
        numList.Add(randomNumber);
        while (randomNumber == numList[countRandom - 1])
        {
            randomNumber = Random.Range(0, colorRocks.Count);
        }
        countRandom++;
        i = 0;
        sequenceDone = false;
        sequenceCount = 0;
    }

    private void DoSequence()
    {
        if(sequenceCount < numList.Count)
        {
            colorRocks[numList[sequenceCount]].GetComponent<SimonRock>().materialRock.color = colorRocks[numList[sequenceCount]].GetComponent<SimonRock>().colorRock;
            timer += Time.deltaTime;
            if (timer >= timeColored)
            {
                colorRocks[numList[sequenceCount]].GetComponent<SimonRock>().materialRock.color = Color.black;
                sequenceCount++;
                timer = 0;
            }
        }

        else
        {
            ChangeColorBlack();
            sequenceDone = true;
        }
    }

    public void ChangeColorBlack()
    {
        foreach (GameObject rock in colorRocks)
        {
            rock.GetComponent<Renderer>().material.color = Color.black;
        }
    }
}
