using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject[] screens;
    public GameObject optionsMenu;
    public GameObject surveyScreen;
    public GameObject a_score;
    public GameObject b_score;
    int screenCount;
    bool isPCG;
    bool isOptionsMenuOn;

	// Use this for initialization
	void Start ()
    {
        //PlayerPrefs.DeleteAll();
        screenCount = PlayerPrefs.GetInt(SaveManager.screenCount);
        foreach (GameObject currentScreen in screens)
        {
            currentScreen.SetActive(false);
        }
        if (PlayerPrefs.GetInt(SaveManager.isPCGPlayed) == 1 && PlayerPrefs.GetInt(SaveManager.isManualPlayed) == 1)
        {
            loadScores();
        }
        else
        {
            screens[screenCount].SetActive(true);
        }
        
        isOptionsMenuOn = false;
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
        PlayerPrefs.SetInt(SaveManager.screenCount, screenCount-1);
        if (!isPCG)
        {
            PlayerPrefs.SetInt(SaveManager.difficultyLevel, difficultyLevel);
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void OptionsMenu()
    {
        isOptionsMenuOn = !isOptionsMenuOn;
        if(isOptionsMenuOn)
        {
            optionsMenu.SetActive(true);
            screens[screenCount].SetActive(false);
        }
        else
        {
            optionsMenu.SetActive(false);
            screens[screenCount].SetActive(true);
        }
    }

    void loadScores()
    {
        a_score.GetComponent<Text>().text = PlayerPrefs.GetInt(SaveManager.ManualHighscore).ToString();
        b_score.GetComponent<Text>().text = PlayerPrefs.GetInt(SaveManager.PCGHighscore).ToString();
        screens[screenCount].SetActive(false);
        surveyScreen.SetActive(true);
        transform.FindChild("ControlsButton").gameObject.SetActive(false);
    }

    public void Survey()
    {
        PlayerPrefs.DeleteAll();
        Application.OpenURL("https://coventry.onlinesurveys.ac.uk/sm303com");
        Application.Quit();
    }

    public void Retry(bool type)
    {
        PlayerPrefs.SetInt(SaveManager.screenCount, 2);
        transform.FindChild("ControlsButton").gameObject.SetActive(true);
        surveyScreen.SetActive(false);
        SetGameType(type);
        Continue();
    }
}
