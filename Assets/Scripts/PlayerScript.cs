using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 2.0f;
    private Rigidbody2D rb;

    private float health = 200;
    private float startHealth;

    public GameObject projectilePrefab;  // The projectile prefab
    public Transform launchPoint;        // The point from where the projectile is launched
    public float projectileDamage = 1f; // Damage of the projectile
    public float projectileSpeed = 1f;   // Speed of the projectile
    public float projectileLifetime = 1f; // Time before the projectile disappears
    public float shootCooldown = 0.2f;     // Cooldown time between shots in seconds

    private float currentCooldown = 0f;  // Tracks current cooldown timer
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startHealth = health;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Movement logic
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);

        // Only play movement animations if player is not shooting
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ShootRight") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("ShootLeft") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("ShootUp") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("ShootDown"))
        {
            // Play movement animations
            if (horizontal > 0)
            {
                animator.Play("Right");
            }
            else if (horizontal < 0)
            {
                animator.Play("Left");
            }
            else if (vertical > 0)
            {
                animator.Play("Up");
            }
            else if (vertical < 0)
            {
                animator.Play("Down");
            }
            else if (horizontal == 0 && vertical == 0)
            {
                animator.Play("Idle");
            }
        }

        // Shooting logic
        if (Input.GetMouseButton(0) && currentCooldown <= 0f)
        {
            FireProjectile();
            currentCooldown = shootCooldown;  // Reset cooldown after shooting
        }

        // Update cooldown timer
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    void FireProjectile()
    {
        // Convert mouse position to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        // Calculate direction to the mouse and normalize
        Vector2 direction = (mousePosition - launchPoint.position).normalized;

        // Instantiate the projectile at the launch point
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);

        // Get the Rigidbody2D of the projectile
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

        if (rbProjectile != null)
        {
            // Reset velocity and apply velocity in the direction of the mouse
            rbProjectile.velocity = direction * projectileSpeed;
        }

        // Destroy the projectile after a set time
        Destroy(projectile, projectileLifetime);

        // Trigger the shooting animation based on direction
        if (direction.x > 0)
        {
            animator.SetTrigger("isShooting");
            animator.Play("ShootRight");
        }
        else if (direction.x < 0)
        {
            animator.SetTrigger("isShooting");
            animator.Play("ShootLeft");
        }
        else if (direction.y > 0)
        {
            animator.SetTrigger("isShooting");
            animator.Play("ShootUp");
        }
        else if (direction.y < 0)
        {
            animator.SetTrigger("isShooting");
            animator.Play("ShootDown");
        }

        // Reset cooldown to allow shooting again
        currentCooldown = shootCooldown;
    }

    public float GetProjectileDamage()
    {
        return projectileDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= collision.gameObject.GetComponent<EnemyScript>().GetHitStrength();
            if (health < 1)
            {
                Debug.LogError("HIT");
                // Handle death logic here
            }
        }
    }
}
