﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    //Variables
    public float gameSpeed;
    public float platformSpeed;
    public GameObject startPlatform;
    public GameObject smallPlatform;
    public GameObject mediumPlatform;
    public GameObject largePlatform;
    public GameObject player;
    public GameObject scoreText;
    public GameObject deathScene;
    
    LinkedList<GameObject> platforms;
    LinkedList<GameObject> overheadPlatforms;

    public float platformSize = 2.5f;
    bool isStartPlatform = true;
    Vector3 offset;
    float platformTime;
    float speedTimer;
    float overheadPlatformTime;
    int score;

    //PEM Player Experience Model
    float gameTime;
    float distanceToStart;
    float previousDistance;
    UnityStandardAssets._2D.Platformer2DUserControl playerScript;
    float currentAverageSpeed;
    static float AVERAGE_SPEED = 40;
    
    //PCG Procedural Content Generation
    float platformGap; //space between platforms
    float platformLength; //length of PCG platform
    public bool isPCG_On; //PCG mode enabled or disabled 
    public int difficultyLevel; //difficulty level for when PCG disabled //Levels: 0 = easy, 1 = normal, 2 = hard

    // Use this for initialization
    void Awake ()
    {
        //platforms = new Queue<GameObject>();
        platforms = new LinkedList<GameObject>();
        overheadPlatforms = new LinkedList<GameObject>();
        platformTime = 2.0f;
        speedTimer = 0.0f;
        score = 0;
        startPlatform.GetComponent<Platform>().setSize(8);
        Debug.Log("SIZE SET: "+ startPlatform.GetComponent<Platform>().getSize());
        //add starting platform
        platforms.AddLast(startPlatform);
        //add a few platforms to act as a buffer
        createDifficultyPlatform();
        createDifficultyPlatform();
        createDifficultyPlatform();
        //PEM
        gameTime = 0;
        playerScript = player.GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>();
        currentAverageSpeed = 0;
        distanceToStart = 0;
        previousDistance = 0;
        platformGap = 5;

        //get difficulty level if PCG mode is off
        if (!isPCG_On)
        {
            difficultyLevel = PlayerPrefs.GetInt(SaveManager.difficultyLevel);
        }
        gameSpeed = 0.01f;
        deathScene.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateTimer();
        updateDistanceTravelled();

        platformTime += Time.deltaTime;
        overheadPlatformTime += Time.deltaTime;        
        //move platforms
        LinkedListNode<GameObject> currentPlatform = platforms.First;
            //calc distance to move
        float distance = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
        offset = new Vector3((distance/6)+ gameSpeed, 0f, 0f);
        //normal
        for (int i = 0; i < platforms.Count; i++) 
        {
            currentPlatform.Value.transform.position -= offset;
            float sizeOffset = currentPlatform.Value.GetComponent<Platform>().getSize();
            isPlatformVisible(currentPlatform, sizeOffset);
            currentPlatform = currentPlatform.Next;
        }
        //overhead
        if (overheadPlatforms.Count != 0)
        {
            currentPlatform = overheadPlatforms.First;
            for (int i = 0; i < overheadPlatforms.Count; i++)
            {
                currentPlatform.Value.transform.position -= offset;
                float sizeOffset = currentPlatform.Value.GetComponent<Platform>().getSize();
                isPlatformVisible(currentPlatform, sizeOffset, true);
                currentPlatform = currentPlatform.Next;
            }
        }
        //generate platforms 
        //normal
        if (platforms.Count < 5)
        {
                //if not PCG mode
            if (!isPCG_On)
            {
                createDifficultyPlatform();
            }
            else
            {
                CreatePCGPlatform();
            }
            platformTime = 0.0f;
        }
        //overhead //REMOVED TEMPORILY UNTIL CONSTRAINTS ADDED
        if (overheadPlatformTime > gameSpeed && overheadPlatforms.Count < 3)
        {
            //createDifficultyPlatform(true);
            //overheadPlatformTime = 0.0f;
        }


    }

    void updateTimer()
    {
        gameTime += Time.deltaTime;
        speedTimer += Time.deltaTime;

        if (speedTimer > 15.0f)
        {
            gameSpeed += 0.05f;
            speedTimer = 0.0f;
        }
        //Debug.Log("TIME: " + gameTime);
    }

    void updateDistanceTravelled()
    {        
        if(playerScript.h > 0)
        {
            distanceToStart += playerScript.h;
        }
        //Debug.Log("DISTANCE: " + distanceToStart);
        if (distanceToStart - previousDistance > 25)
        {
            previousDistance = distanceToStart;
            updateAverageSpeed();
        }
        
    }

    void updateAverageSpeed()
    {
        float newSpeed = distanceToStart / gameTime;
        if (currentAverageSpeed != 0)
        {
            currentAverageSpeed = (currentAverageSpeed + newSpeed) / 2;
        }
        else
        {
            currentAverageSpeed = newSpeed;
        }
        Debug.Log("AVERAGE SPEED: " + currentAverageSpeed);
    }

    void CreatePCGPlatform()
    {
        Debug.Log("PCG_LOOP");
        if (currentAverageSpeed != 0)
        {
            //get game average speed
            //get player average speed
            //player/game
            //float platformScaler = AVERAGE_SPEED / currentAverageSpeed;//NEEDS FLIPPING? 

            
            float platformScaler = currentAverageSpeed / AVERAGE_SPEED;
            if (platformScaler >1)
            {
                platformScaler -= 1;
            }
            else
            {
                platformScaler = 1+(1-platformScaler);
            }
            //create new platform
                //cap platform scale
            if(platformScaler > 1.5f)
            {
                platformGap = 7.0f;
                platformScaler = 1.5f;
            }
            else if(platformScaler < 0.5f)
            {
                platformGap = 2.0f;
                platformScaler = 0.5f;
            }
            else
            {
                platformGap = 5.0f;
            }
            
            
            if(platformScaler < 1)
            {
                //cap platform size at 4
                if(8*(1-platformScaler)<4)
                {
                    createNewPlatform(4, mediumPlatform);
                }
                else
                {
                    createNewPlatform((int)(8 *(1- platformScaler)), mediumPlatform);
                }
            }
            else
            {
                //cap platform size at 12
                if(8*platformScaler>12)
                {
                    createNewPlatform(12, mediumPlatform);
                }
                else
                {
                    createNewPlatform((int)(8 * platformScaler), mediumPlatform);
                }
            }
             
            //scale platform
            GameObject newPlatform = platforms.Last.Value;
            //make bigger platforms
            if (platformScaler >= 1)
            {
                //cap platform size at 1.5f
                if (newPlatform.transform.localScale.x +(platformScaler - 1) > 1.5f)
                {
                    newPlatform.transform.localScale += new Vector3(0.5f, 0, 0);
                }
                else
                {
                    newPlatform.transform.localScale += new Vector3(platformScaler - 1, 0, 0);
                }
            }
            //make smaller platforms
            else
            {
                //cap platform size at 0.5f
                if (newPlatform.transform.localScale.x - platformScaler < 0.5f)
                {
                    newPlatform.transform.localScale -= new Vector3(0.5f, 0, 0);
                }
                else
                {
                    newPlatform.transform.localScale -= new Vector3(platformScaler, 0, 0);
                }
            }
        }
        else
        {
            createDifficultyPlatform();
        }
        
    }

    public GameObject getCurrentPlatform()
    {
        return platforms.First.Value;
    }

    //platform creation
    void createNewPlatform(int size, GameObject platformType, int overhead = 0)
    {
        Platform lastPlatform = platforms.Last.Value.GetComponent<Platform>();
        float lastPosX = lastPlatform.transform.position.x + (lastPlatform.getSize()*platformSize);
        Vector3 newPosition = new Vector3(lastPosX-platformGap+(overhead*size),-5+overhead,0);
        GameObject newPlatform = (GameObject) Instantiate(platformType, newPosition, Quaternion.identity);
        newPlatform.GetComponent<Platform>().setSize(size);
        if (overhead > 0)
        {
            overheadPlatforms.AddLast(newPlatform);
            Debug.Log("CREATED OVERHEAD");
        }
        else
        {
            //Debug.Log("PLATFORM PRE TEST: INITIAL SIZE: " +size);
            platforms.AddLast(newPlatform);
            //Debug.Log("PLATFORM PRE TEST: SIZE: " + newPlatform.GetComponent<Platform>().getSize());
        }
    }

    //random platform generation // generates based on difficulty level
    void createDifficultyPlatform(bool isOverhead = false)
    {
        int platformNumber = Random.Range(1, 4);
        //Debug.Log("PLATFORM NUMBER: " + difficultyLevel);
        GameObject newPlatform;
        switch (difficultyLevel)
        {
            case 0:
                platformNumber = 8;
                newPlatform = largePlatform;
                platformGap = 5.0f;
                break;
            case 1:
                platformNumber = 7;
                newPlatform = mediumPlatform;
                platformGap = 5.0f;
                break;
            case 2:
                platformNumber = 6;
                newPlatform = smallPlatform;
                platformGap = 2.0f;
                break;
            default:
                Debug.Log("ERROR");
                platformNumber = 6;
                newPlatform = smallPlatform;
                break;
        }
        if (isOverhead)
        {
            Debug.Log("OVERHEAD");
            createNewPlatform(platformNumber, newPlatform, 3);
        }
        else
        {
            createNewPlatform(platformNumber, newPlatform);
        }
    }

    //platform deletion
    void isPlatformVisible(LinkedListNode<GameObject> currentPlatform, float sizeOffset, bool isOverhead = false)
    {
        if(currentPlatform.Value.transform.position.x < 0 - (platformSize*sizeOffset) -5)
        {
            if (isOverhead)
            {
                overheadPlatforms.Remove(currentPlatform);
                Debug.Log("DESTROYED OVERHEAD");
            }
            else
            {
                platforms.Remove(currentPlatform);
                Debug.Log("DESTROYED NORMAL");
            }
            Destroy(currentPlatform.Value);
            score += 100;
            scoreText.GetComponent<Text>().text = ""+score;
            Debug.Log("Destroyed");
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        deathScene.SetActive(true);
        if(isPCG_On)
        {
            PlayerPrefs.SetInt(SaveManager.isPCGPlayed, 1);
        }
        else
        {
            PlayerPrefs.SetInt(SaveManager.isManualPlayed, 1);
        }
    }

    public void Retry()
    {
        Time.timeScale = 1;
        if (isPCG_On)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
