using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    // This function will be called when the "New Game" button is clicked
    public void StartNewGame()
    {
        // Replace "MainGame" with the name of your main game scene
        SceneManager.LoadScene("Main Game");
    }
}