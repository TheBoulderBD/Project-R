using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    public GameObject tutorialOverlay;


    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the Player touches the crown
        {
            // Activate the tutorial overlay
            if (tutorialOverlay != null)
            {
                tutorialOverlay.SetActive(true);
            }

            // Destroy the crown object
            Destroy(gameObject);
        }
    }
}
