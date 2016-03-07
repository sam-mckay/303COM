using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    public class Restarter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().GameOver();
                Debug.Log("GAME OVER");
            }
        }
    }
}
