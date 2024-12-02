using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Rigidbody2D rb;

    public GameObject projectilePrefab;  // The projectile prefab
    public Transform launchPoint;        // The point from where the projectile is launched

    public GameOverScreen gameOverScreen;

    public float playerSpeed = 2.0f;
    public int playerHealth; // playerHealth of the player
    public int maxHealth = 100;
    public HealthBar healthBar;
    public int projectileDamage = 1; // Damage of the projectile
    public float projectileSpeed = 1f;   // Speed of the projectile
    public float projectileLifetime = 1f; // Time before the projectile disappears
    public float shootCooldown = 0.2f;     // Cooldown time between shots in seconds

    public AudioSource footstep;
    public AudioSource shooting;

    private float currentCooldown = 0f;  // Tracks current cooldown timer
    private Animator animator;

    void Start()
    {
        playerHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        footstep = GetComponent<AudioSource>();
        shooting = GetComponent<AudioSource>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Movement logic
        rb.velocity = new Vector2(horizontal * playerSpeed, vertical * playerSpeed);

        healthBar.setHealth(playerHealth);

        if (playerHealth < 1)
        {
            GameOver();
        }

        // Only play movement animations if player is not shooting
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ShootRight") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("ShootLeft") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("ShootUp") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("ShootDown"))
        {
            // Play movement animations
            if (horizontal > 0)
            {
                footstep.enabled = true;
                animator.Play("Right");
            }
            else if (horizontal < 0)
            {
                footstep.enabled = true;
                animator.Play("Left");
            }
            else if (vertical > 0)
            {
                footstep.enabled = true;
                animator.Play("Up");
            }
            else if (vertical < 0)
            {
                footstep.enabled = true;
                animator.Play("Down");
            }
            else if (horizontal == 0 && vertical == 0)
            {
                footstep.enabled = false;
                animator.Play("Idle");
            }
        }

        // Shooting logic
        if (Input.GetMouseButton(0) && currentCooldown <= 0f)
        {
            FireProjectile();
            shooting.enabled = true;
            shooting.Play();
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

    public int GetProjectileDamage()
    {
        return projectileDamage;
    }

    public void SetProjectileDamage(int damage)
    {
        projectileDamage = damage;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public void SetPlayerSpeed(float playSpeed)
    {
        playerSpeed = playSpeed;
    }

    public float GetShootCooldown()
    {
        return shootCooldown;
    }

    public void SetShootCooldown(float cooldown)
    {
        shootCooldown = cooldown;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public void SetProjectileSpeed(float projSpeed)
    {
        projectileSpeed = projSpeed;
    }

    public float GetProjectileLifetime()
    {
        return projectileLifetime;
    }

    public void SetProjectileLifetime(float lifetime)
    {
        projectileLifetime = lifetime;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        healthBar.setHealth(playerHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= collision.gameObject.GetComponent<EnemyScript>().GetHitStrength();

            
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            playerHealth -= collision.gameObject.GetComponent<BossScript>().GetHitStrength();

        }
    }

    public void GameOver()
    {
        gameOverScreen.Setup();
    }

}