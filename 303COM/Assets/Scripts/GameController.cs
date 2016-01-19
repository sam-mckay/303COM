using UnityEngine;
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
    public GameObject largePlatform;

    public GameObject scoreText;

    Queue<GameObject> platforms;
    
    float platformSize = 2.5f;
    bool isStartPlatform = true;
    Vector3 offset;
    float platformTime;
    int score;
    // Use this for initialization
    void Awake ()
    {
        platforms = new Queue<GameObject>();
        platformTime = 2.0f;
        offset = new Vector3(platformSpeed, 0f, 0f);
        score = 0;
        startPlatform.GetComponent<Platform>().setSize(8);
        Debug.Log("SIZE SET: "+ startPlatform.GetComponent<Platform>().getSize());
        platforms.Enqueue(startPlatform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        platformTime += Time.deltaTime;

        GameObject[] platformTempArray = platforms.ToArray();
        for(int i =0; i < platforms.Count; i++)
        {
            GameObject currentPlatform = platformTempArray[i];
            currentPlatform.transform.position -= offset;
            int sizeOffset = currentPlatform.GetComponent<Platform>().getSize();
            isPlatformVisible(currentPlatform, sizeOffset);
        }

        if(platformTime > gameSpeed)
        {
            createNewPlatform(8);
            platformTime = 0.0f;
        }
    }

    //platform creation
    void createNewPlatform(int size)
    {
        Vector3 newPosition = new Vector3(10,-5,0);
        GameObject newPlatform = (GameObject) Instantiate(largePlatform, newPosition, Quaternion.identity);
        newPlatform.GetComponent<Platform>().setSize(size);
        platforms.Enqueue(newPlatform);
    }

    

    //platform deletion
    void isPlatformVisible(GameObject currentPlatform, int sizeOffset)
    {
        if(currentPlatform.transform.position.x < 0 - (platformSize*sizeOffset))
        {
            platforms.Dequeue();
            Destroy(currentPlatform);
            score += 100;
            scoreText.GetComponent<Text>().text = ""+score;
            Debug.Log("Destroyed");
        }
    }
}
