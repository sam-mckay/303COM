using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject[] screens;
    int screenCount;
    bool isPCG;
	// Use this for initialization
	void Start ()
    {
        PlayerPrefs.DeleteAll();
        screenCount = PlayerPrefs.GetInt(SaveManager.screenCount);
        //screenCount = 0;
        foreach(GameObject currentScreen in screens)
        {
            currentScreen.SetActive(false);
        }
        screens[screenCount].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Continue()
    {
        screens[screenCount].SetActive(false);
        screenCount++;
        PlayerPrefs.SetInt(SaveManager.screenCount, screenCount);
        screens[screenCount].SetActive(true);
    }

    public void SetGameType(bool type)
    {
        //false = game a 
        //true = game b
        isPCG = type;
    }

    public void SetDifficulty(int difficultyLevel)
    {
        if(!isPCG)
        {
            PlayerPrefs.SetInt(SaveManager.difficultyLevel, difficultyLevel);
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
}
