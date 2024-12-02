using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the Player tag
        if (collision.CompareTag("Player"))
        {
            // Load the specified scene
            SceneManager.LoadScene("Boss Room");
        }
    }
}
