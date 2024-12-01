using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the orb
    public int damage = 10;   // Damage the orb deals to the player
    public float lifetime = 5.0f; // Lifetime of the orb before destruction

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void Start()
    {
        // Destroy the orb after its lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the orb in the given direction
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the orb hits the player, deal damage
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>()?.TakeDamage(damage);
            Destroy(gameObject); // Destroy the orb after dealing damage
        }
        // Prevent the orb from affecting the boss
        else if (collision.gameObject.CompareTag("Boss"))
        {
            return; // Do nothing if the orb touches the boss
        }
        // Destroy the orb when it hits walls or other obstacles
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
