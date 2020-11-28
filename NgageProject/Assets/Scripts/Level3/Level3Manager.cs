using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    private Player3Controller player;

    public Transform left;
    public Transform right;

    public Transform leftSpawner; // if player goes to left from this point, a level part will be activated on left side 
    public Transform rightSpawner; // if player goes to right from this point, a level part will be activated on right side

    public bool spawnedLeft = false; //if there is level part on left side of current part
    public bool spawnedRight = false; //if there is level part on right side of current part
    public bool otherDc = true; //if all other level parts are disabled

    public bool isCurrent; //if this level part is current, by current we mean that player is on the level part
    public bool coinSpawned; // if coins are spawned or can be spawned on level part
    public bool canSpawnEnemies; // if we can spawn enemies on level part

    public List<GameObject> enemyList; //list of enemies
    public GameObject enemyPrefab;

    public List<GameObject> pooledCoins; //pool list of coins
    public GameObject coinPrefab;

    public List<GameObject> platformList;
    public GameObject platformPrefab;
    public bool canSpawnPlatform;

    public List<GameObject> platformCoins;

    private void Awake()
    {
        player = FindObjectOfType<Player3Controller>();
        pooledCoins = new List<GameObject>();
        for (int i = 0; i < 5; i++) //instantiating coins and adding them to pool list
        {
            GameObject coin = (GameObject)Instantiate(coinPrefab);
            coin.SetActive(false);
            pooledCoins.Add(coin);
        }
        coinSpawned = false;

        enemyList = new List<GameObject>();
        for (int i = 0; i < 3; i++) //instantiating enemies and adding them to enemy list
        {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyList.Add(enemy);
        }

        platformList = new List<GameObject>();
        for (int i = 0; i < 4; i++) //instantiating enemies and adding them to enemy list
        {
            GameObject platform = (GameObject)Instantiate(platformPrefab);
            platform.SetActive(false);
            platformList.Add(platform);
        }

        platformCoins = new List<GameObject>();
        for (int i = 0; i < 4; i++) //instantiating enemies and adding them to enemy list
        {
            GameObject platformCoin = (GameObject)Instantiate(coinPrefab);
            platformCoin.SetActive(false);
            platformCoins.Add(platformCoin);
        }

        canSpawnEnemies = true;
        canSpawnPlatform = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > left.position.x && player.transform.position.x < right.position.x) //to check if the player is on current level part or not
        {
            isCurrent = true;
        }
        else
        {
            isCurrent = false;
        }


        if (isCurrent)
        {
            if (player.transform.position.x < leftSpawner.position.x)
            {
                spawnLeft(); //if this level part is current and player goes to left side, we will pool a level part at left
                otherDc = false;
            }
            else if (player.transform.position.x > rightSpawner.position.x)
            {
                spawnRight(); //if this level part is current and player goes to right side, we will pool a level part at right
                otherDc = false;
            }
            else
            {   //if the player is in between the current part, we will not pool any other level parts and will disable all other level parts 
                spawnedLeft = false;
                spawnedRight = false;
                deactiveOther();
                enableCoins(); //enabling the coins
                enableEnemies(); //enabling the enemies
                enablePlatform();
            }
        }
    }

    public void spawnLeft() //to pool a level part at left side
    {
        if (!spawnedLeft)
        {
            GameObject newLevelObj = Spawn3Manager.instant.getPooledLevel();
            newLevelObj.transform.position = new Vector3(this.transform.position.x - 191.75f, transform.position.y, transform.position.z);
            newLevelObj.SetActive(true);
            spawnedLeft = true;
            newLevelObj.GetComponent<Level3Manager>().spawnedRight = true; //to check if there is any level part on the right side
            newLevelObj.GetComponent<Level3Manager>().coinSpawned = true; //this will prevent activating coins of left side
            newLevelObj.GetComponent<Level3Manager>().canSpawnEnemies = false; //this will prevent activating enemies at left side
        }

    }

    public void spawnRight()
    {
        if (!spawnedRight)
        {
            GameObject newLevelObj = Spawn3Manager.instant.getPooledLevel();
            newLevelObj.transform.position = new Vector3(this.transform.position.x + 191.75f, transform.position.y, transform.position.z);
            newLevelObj.SetActive(true);
            spawnedRight = true;
            newLevelObj.GetComponent<Level3Manager>().spawnedLeft = true; //to check if there is any level part on left side
            newLevelObj.GetComponent<Level3Manager>().coinSpawned = false; // this will indicate that we can activate coins on that level part
            newLevelObj.GetComponent<Level3Manager>().canSpawnEnemies = true; // this will indicate that we can actictivate enemies on that level part
        }

    }

    public void deactiveOther() //deactivating other level parts
    {
        if (!otherDc && isCurrent)
        {
            Spawn3Manager.instant.removeOthers();
            otherDc = true;
        }
    }

    public void enableCoins() //enabling the coins on the current level part
    {
        if (!coinSpawned)
        {
            float tempPosX = player.transform.position.x;
            for (int i = 0; i < pooledCoins.Count; i++)
            {
                pooledCoins[i].transform.position = new Vector3(Random.Range(tempPosX + 15, tempPosX + 30), Random.Range(-4.5f, -3), 0); //this will prevent overlapping of the coins
                tempPosX = pooledCoins[i].transform.position.x;
                pooledCoins[i].SetActive(true);
            }
            coinSpawned = true; //indicates that coins are activated one time
        }
    }

    public void disbleCoins()
    {
        for (int i = 0; i < pooledCoins.Count; i++)
        {
            if (pooledCoins[i].activeInHierarchy)
            {
                pooledCoins[i].SetActive(false);
            }
        }
    }

    public void enableEnemies()
    {
        if (canSpawnEnemies)
        {
            float tempPosX = player.transform.position.x;
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].transform.position = new Vector3(Random.Range(tempPosX + 25, tempPosX + 50), -4f, 0); //this will prevent overlapping of enemies
                tempPosX = enemyList[i].transform.position.x;
                enemyList[i].SetActive(true);
                canSpawnEnemies = false; //indicates that enemies are activated 
            }
        }
    }

    public void disableEnemies()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetActive(false);
        }
    }

    public void enablePlatform()
    {
        if (canSpawnPlatform)
        {
            float tempPosX = player.transform.position.x;
            for (int i = 0; i < platformList.Count; i++)
            {
                platformList[i].transform.position = new Vector3(Random.Range(tempPosX + 30, tempPosX + 40), -1f, 0); //this will prevent overlapping of enemies
                tempPosX = platformList[i].transform.position.x;
                platformList[i].SetActive(true);

                platformCoins[i].transform.position = new Vector3(Random.Range(tempPosX - 2.5f, tempPosX + 2.5f), 1f, 0);
                platformCoins[i].SetActive(true);

                canSpawnPlatform = false; //indicates that enemies are activated 
            }
        }
    }

    public void disablePlatforms()
    {
        for (int i = 0; i < platformList.Count; i++)
        {
            platformList[i].SetActive(false);
        }

        for (int i = 0; i < platformList.Count; i++)
        {
            platformCoins[i].SetActive(false);
        }
    }
}
