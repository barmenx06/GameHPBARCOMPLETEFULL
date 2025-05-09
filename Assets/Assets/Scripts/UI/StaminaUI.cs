using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private float updateSpeed = 5f;
    
    private float targetFill;

    private void Start()
    {
        if (staminaBar == null)
        {
            Debug.LogError("Stamina bar referansı eksik!");
            enabled = false;
            return;
        }

        PlayerController.Instance.OnStaminaChanged += UpdateStaminaBar;
    }

    private void OnDestroy()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnStaminaChanged -= UpdateStaminaBar;
        }
    }

    private void Update()
    {
        staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, targetFill, Time.deltaTime * updateSpeed);
    }

    private void UpdateStaminaBar(float staminaPercentage)
    {
        targetFill = staminaPercentage;
    }
} 