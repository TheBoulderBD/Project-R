using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        IncreasePlayerSpeed,
        IncreasePlayerHealth,
        IncreaseProjectileSpeed,
        IncreaseProjectileDamage,
        IncreaseProjectileLifetime,
        DecreaseShootCooldown
    }

    public Sprite speedSprite;
    public Sprite healthSprite;
    public Sprite projSpeedSprite;
    public Sprite projDamageSprite;
    public Sprite projLifetimeSprite;
    public Sprite cooldownSprite;

    private PowerUpType powerUpType;
    private PlayerScript playerScript;

    void Start()
    {
        // Randomly select a power-up type
        powerUpType = (PowerUpType)Random.Range(0, System.Enum.GetValues(typeof(PowerUpType)).Length);

        // Set sprite based on selected power-up type
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (powerUpType)
        {
            case PowerUpType.IncreasePlayerSpeed:
                spriteRenderer.sprite = speedSprite;
                break;
            case PowerUpType.IncreasePlayerHealth:
                spriteRenderer.sprite = healthSprite;
                break;
            case PowerUpType.IncreaseProjectileSpeed:
                spriteRenderer.sprite = projSpeedSprite;
                break;
            case PowerUpType.IncreaseProjectileDamage:
                spriteRenderer.sprite = projDamageSprite;
                break;
            case PowerUpType.IncreaseProjectileLifetime:
                spriteRenderer.sprite = projLifetimeSprite;
                break;
            case PowerUpType.DecreaseShootCooldown:
                spriteRenderer.sprite = cooldownSprite;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<PlayerScript>();

            // Apply the effect based on the power-up type
            switch (powerUpType)
            {
                case PowerUpType.IncreasePlayerSpeed:
                    playerScript.SetPlayerSpeed(playerScript.GetPlayerSpeed() + 1f);
                    break;
                case PowerUpType.IncreasePlayerHealth:
                    playerScript.SetPlayerHealth(playerScript.GetPlayerHealth() + 10);
                    break;
                case PowerUpType.IncreaseProjectileSpeed:
                    playerScript.SetProjectileSpeed(playerScript.GetProjectileSpeed() + 1f);
                    break;
                case PowerUpType.IncreaseProjectileDamage:
                    playerScript.SetProjectileDamage(playerScript.GetProjectileDamage() + 1f);
                    break;
                case PowerUpType.IncreaseProjectileLifetime:
                    playerScript.SetProjectileLifetime(playerScript.GetProjectileLifetime() + 0.5f);
                    break;
                case PowerUpType.DecreaseShootCooldown:
                    playerScript.SetShootCooldown(Mathf.Max(0.1f, playerScript.GetShootCooldown() - 0.1f));
                    break;
            }

            // Destroy the power-up after use
            Destroy(gameObject);
        }
    }
}
