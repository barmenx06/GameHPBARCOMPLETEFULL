using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float updateSpeed = 5f;
    [SerializeField] private PlayerHealth playerHealth;
    
    private float targetFill;

    private void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("Health bar referansı eksik!");
            enabled = false;
            return;
        }

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
        }
        else
        {
            Debug.LogError("PlayerHealth referansı atanmadı!");
        }
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFill, Time.deltaTime * updateSpeed);
    }

    private void UpdateHealthBar(float healthPercentage)
    {
        targetFill = healthPercentage;
    }
} 