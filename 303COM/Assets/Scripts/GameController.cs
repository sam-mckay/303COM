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
    LinkedList<GameObject> overheadPlatforms;

    public float platformSize = 2.5f;
    bool isStartPlatform = true;
    Vector3 offset;
    float platformTime;
    float overheadPlatformTime;
    int score;
    // Use this for initialization
    void Awake ()
    {
        //platforms = new Queue<GameObject>();
        platforms = new LinkedList<GameObject>();
        overheadPlatforms = new LinkedList<GameObject>();
        platformTime = 2.0f;
        
        score = 0;
        startPlatform.GetComponent<Platform>().setSize(8);
        Debug.Log("SIZE SET: "+ startPlatform.GetComponent<Platform>().getSize());
        //add starting platform
        platforms.AddLast(startPlatform);
        //add a few platforms to act as a buffer
        createRandomPlatform();
        createRandomPlatform();
        createRandomPlatform();

    }
	
	// Update is called once per frame
	void Update ()
    {
        platformTime += Time.deltaTime;
        overheadPlatformTime += Time.deltaTime;        
        //move platforms
        LinkedListNode<GameObject> currentPlatform = platforms.First;
            //calc distance to move
        float distance = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
        offset = new Vector3((distance/4)+0.01f, 0f, 0f);
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
        if (platformTime > gameSpeed && platforms.Count < 5)
        {
            createRandomPlatform();
            platformTime = 0.0f;
        }
        //overhead
        if (overheadPlatformTime > gameSpeed && overheadPlatforms.Count < 3)
        {
            createRandomPlatform(true);
            overheadPlatformTime = 0.0f;
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
        Vector3 newPosition = new Vector3(lastPosX-5.0f+(overhead*size),-5+overhead,0);
        GameObject newPlatform = (GameObject) Instantiate(platformType, newPosition, Quaternion.identity);
        newPlatform.GetComponent<Platform>().setSize(size);
        if (overhead > 0)
        {
            overheadPlatforms.AddLast(newPlatform);
            Debug.Log("CREATED OVERHEAD");
        }
        else
        {
            platforms.AddLast(newPlatform);
        }
    }

    //random platform generation
    void createRandomPlatform(bool isOverhead = false)
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
        if(currentPlatform.Value.transform.position.x < 0 - (platformSize*sizeOffset))
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
}
