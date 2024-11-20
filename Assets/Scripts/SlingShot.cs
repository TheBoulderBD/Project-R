using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign the stone prefab
    public Transform launchPoint; // Assign the location to fire from
    public float projectileSpeed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        // Calculate direction
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - launchPoint.position).normalized;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);

        // Apply velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }
}
