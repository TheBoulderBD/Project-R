using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private float range;
    public Transform target;
    private float minDistance = 5.0f;
    private bool targetCollision = false;
    public float speed = 1.0f;
    private float thrust = 1.5f;
    public int maxHealth = 500;
    public int health;
    public int hitStrength = 30;
    public HealthBar healthBar;

    public Sprite deathSprite;
    private bool isDead = false;

    // Parameters for separation
    public float separationRadius = 1.0f; // Distance to maintain from other enemies
    public float separationStrength = 0.5f; // How strongly to avoid others

    private AudioSource deathNoise;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public GameObject orbPrefab; // Assign your orb prefab here
    public float orbCooldown = 5f; // Time between firing orbs
    private float orbTimer = 0f;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        deathNoise = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        healthBar.setHealth(health);

        if (isDead) return;

        // Calculate distance to the player
        range = Vector2.Distance(transform.position, target.position);

        if (range < minDistance && !targetCollision)
        {
            // Move toward the player
            Vector3 direction = (target.position - transform.position).normalized;

            // Apply separation
            Vector3 separation = GetSeparationVector();
            direction += separation * separationStrength;

            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        // Reset rotation (optional to keep sprites upright)
        transform.rotation = Quaternion.identity;

        orbTimer += Time.deltaTime;

        if (orbTimer >= orbCooldown)
        {
            FireOrbsInAllDirections();
            orbTimer = 0f;
        }
    }

    private Vector3 GetSeparationVector()
    {
        Vector3 separation = Vector3.zero;

        // Find all nearby enemies
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (Collider2D collider in nearbyEnemies)
        {
            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("Enemy"))
            {
                // Calculate the repulsion vector
                Vector3 repulsion = transform.position - collider.transform.position;
                separation += repulsion.normalized / repulsion.magnitude; // Normalize and weight by distance
            }
        }

        return separation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !targetCollision)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;

            targetCollision = true;

            bool right = contactPoint.x > center.x;
            bool left = contactPoint.x < center.x;
            bool top = contactPoint.y > center.y;
            bool bottom = contactPoint.y < center.y;

            if (right) GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
            if (left) GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
            if (top) GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
            if (bottom) GetComponent<Rigidbody2D>().AddForce(-transform.up * thrust, ForceMode2D.Impulse);

            Invoke("FalseCollision", 0.5f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FriendlyProjectile"))
        {
            health -= collision.gameObject.GetComponent<PlayerScript>().GetProjectileDamage();
            Destroy(collision.gameObject);
            if (health < 1)
            {
                isDead = true;
                spriteRenderer.sprite = deathSprite;
                deathNoise.Play();
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                boxCollider.enabled = false;
                Invoke("DestroyEnemy", deathNoise.clip.length);
            }
        }
    }

    void FireOrbsInAllDirections()
    {
        int orbCount = 8; // Number of directions to fire orbs
        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * (360f / orbCount);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Instantiate the orb and initialize its direction
            GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<OrbProjectile>().Initialize(direction);
        }
    }

    void FalseCollision()
    {
        targetCollision = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public int GetHitStrength()
    {
        return hitStrength;
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}