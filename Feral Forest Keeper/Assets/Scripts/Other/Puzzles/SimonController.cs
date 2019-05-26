using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonController : MonoBehaviour
{
    public List<GameObject> colorRocks;
    public List<GameObject> enemySimonList;
    public List<int> numList;

    public List<Vector3> enemyPositionList;

    public bool puzzleActivated;
    private bool randomPatronActivated;
    private bool randomDone;
    public bool sequenceDone;

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
        randomPatronActivated = false;
        i = 0;
        timesGood = 0;
        timer = 0;
        sequenceCount = 0;

        foreach (GameObject enemy in enemySimonList)
        {
            enemyPositionList.Add(enemy.transform.position);
            enemy.SetActive(false);
        }
    }

    void Update()
    {
        if (puzzleActivated && !randomDone)
        {
            GenerateRandomPatron();
        }
        if(puzzleActivated && randomDone && !sequenceDone)
        {
            DoSequence();
        }
        if (puzzleActivated && randomDone && sequenceDone && i >= numList.Count)
        {
            timesGood++;

            if(timesGood < 3)
            {
                AddNewColor();
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
                for (int count = 0; count < enemySimonList.Count; count++)
                {
                    enemySimonList[count].SetActive(true);
                    enemySimonList[count].GetComponent<Enemy>().currentHealth = enemySimonList[count].GetComponent<Enemy>().maxHealth;
                    enemySimonList[count].transform.position = enemyPositionList[count];
                }
            }
        }

        Invoke("ChangeColorBlack", 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NewPlayer")
        {
            puzzleActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            puzzleActivated = false;
            randomDone = false;
            timesGood = 0;
            i = 0;
            sequenceCount = 0;

        }
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
