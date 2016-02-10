using UnityEngine;
using System.Collections;

public class AI_Agent : MonoBehaviour
{
    GameObject currentPlatform;
    UnityStandardAssets._2D.PlatformerCharacter2D movementController;
    float platformSize;
    // Use this for initialization
    void Start ()
    {
        platformSize = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().platformSize;
        movementController = this.GetComponent<UnityStandardAssets._2D.PlatformerCharacter2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentPlatform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().getCurrentPlatform();
        float sizeOffset = currentPlatform.GetComponent<Platform>().getSize();
        //check if near edge
        //Debug.Log("PLATFORM POS: " + (currentPlatform.transform.position.x - (platformSize * sizeOffset))+" AI POS: "+this.transform.position.x);


        Vector3 platformEdge = new Vector3(currentPlatform.transform.position.x + (platformSize * sizeOffset), currentPlatform.transform.position.y, currentPlatform.transform.position.z);
        float distance = Vector3.Distance(this.transform.position, platformEdge);
        Debug.Log("Distance" + distance);
        //((currentPlatform.transform.position.x + (platformSize * sizeOffset)) - this.transform.position.x
        if (distance < 13.0)
        {
            movementController.Move(0.0f, false, true);
        }
    }
}
