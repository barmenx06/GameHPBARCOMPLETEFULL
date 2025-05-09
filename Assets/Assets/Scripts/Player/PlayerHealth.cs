using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityDuration = 1f;
    
    public event Action<float> OnHealthChanged;
    public event Action OnPlayerDamaged;
    public event Action OnPlayerHealed;

    private float currentHealth;
    private bool isInvincible;
    private float invincibilityTimer;

    public PlayerAttack playerAttack; // Inspector'dan atayabilirsin

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        // Eğer Inspector'dan atamadıysan, otomatik bulmak için:
        if (playerAttack == null)
            playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        OnPlayerDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            PlayerController.Instance.Die();
        }
        else
        {
            // Hasar aldıysa HitReact animasyonunu tetikle
            if (playerAttack != null)
                playerAttack.TriggerHitReact();

            StartInvincibility();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        OnPlayerHealed?.Invoke();
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}