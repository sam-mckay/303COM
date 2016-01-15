using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;



public class CustomEventTest : MonoBehaviour {

    int startNum = 1;


 

	// Use this for initialization
	void Awake ()
    {
        Debug.Log("START DATA: READY");
       /* Analytics.CustomEvent("gameStarted", new Dictionary<string, object>
        {
            {"started", startNum}
        });*/
        

        int totalPotions = 5;
        int totalCoins = 100;
        string weaponID = "Weapon_102";
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
          {
            { "potions", totalPotions },
            { "coins", totalCoins },
            { "activeWeapon", weaponID }
          });

        string testProductID = "TEST PRODUCT";
        decimal price = 1.25m;

        Analytics.Transaction("12345abcde", 0.99m, "USD", null, null);

        Debug.Log("START DATA: SENT");
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
