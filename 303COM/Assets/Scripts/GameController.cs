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
    public GameObject mediumPlatform;
    public GameObject largePlatform;
    public GameObject player;
    public GameObject scoreText;

    //Queue<GameObject> platforms;
    LinkedList<GameObject> platforms;

    float platformSize = 2.5f;
    bool isStartPlatform = true;
    Vector3 offset;
    float platformTime;
    int score;
    // Use this for initialization
    void Awake ()
    {
        //platforms = new Queue<GameObject>();
        platforms = new LinkedList<GameObject>();
        platformTime = 2.0f;
        offset = new Vector3(platformSpeed, 0f, 0f);
        score = 0;
        startPlatform.GetComponent<Platform>().setSize(8);
        Debug.Log("SIZE SET: "+ startPlatform.GetComponent<Platform>().getSize());
        //platforms.Enqueue(startPlatform);
        platforms.AddLast(startPlatform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        platformTime += Time.deltaTime;
        float playerSpeed = player.GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().h;
        if (playerSpeed == 0)
        {
            player.transform.position -= offset/3;
        }
        LinkedListNode<GameObject> currentPlatform = platforms.First;
        for (int i =0; i < platforms.Count; i++)
        {
          
            currentPlatform.Value.transform.position -= offset;
            float sizeOffset = currentPlatform.Value.GetComponent<Platform>().getSize();
            isPlatformVisible(currentPlatform, sizeOffset);
            currentPlatform = currentPlatform.Next;
        }

        if (platformTime > gameSpeed && platforms.Count < 5)
        {
            createRandomPlatform();
            platformTime = 0.0f;
        }
    }

    //platform creation
    void createNewPlatform(int size, GameObject platformType)
    {
        Platform lastPlatform = platforms.Last.Value.GetComponent<Platform>();
        float lastPosX = lastPlatform.transform.position.x + (lastPlatform.getSize()*platformSize);
        Vector3 newPosition = new Vector3(lastPosX-5.0f,-5,0);
        GameObject newPlatform = (GameObject) Instantiate(platformType, newPosition, Quaternion.identity);
        newPlatform.GetComponent<Platform>().setSize(size);
        platforms.AddLast(newPlatform);
    }

    //random platform generation
    void createRandomPlatform()
    {
        int platformNumber = Random.Range(1, 4);
        Debug.Log("PLATFORM NUMBER: " + platformNumber);
        GameObject newPlatform;
        switch (platformNumber)
        {
            case 1:
                platformNumber = 6;
                newPlatform = smallPlatform;
                break;
            case 2:
                platformNumber = 8;
                newPlatform = mediumPlatform;
                break;
            case 3:
                platformNumber = 24;
                newPlatform = largePlatform;
                break;
            default:
                Debug.Log("ERROR");
                platformNumber = 6;
                newPlatform = smallPlatform;
                break;
        }
        createNewPlatform(platformNumber, newPlatform);        
    }

    //platform deletion
    void isPlatformVisible(LinkedListNode<GameObject> currentPlatform, float sizeOffset)
    {
        if(currentPlatform.Value.transform.position.x < 0 - (platformSize*sizeOffset))
        {
            platforms.Remove(currentPlatform);
            Destroy(currentPlatform.Value);
            score += 100;
            scoreText.GetComponent<Text>().text = ""+score;
            Debug.Log("Destroyed");
        }
    }
}
