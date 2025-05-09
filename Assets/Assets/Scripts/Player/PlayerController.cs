using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Joystick ve Hareket Ayarları")]
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float runThreshold = 0.7f;
    [SerializeField] private bool rotateToMoveDirection = true;

    [Header("Stamina Ayarları")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float runStaminaCost = 20f;
    private float currentStamina;

    [Header("Animasyon Parametreleri")]
    [SerializeField] private string speedParameterName = "Speed";
    [SerializeField] private string isAttackParameterName = "isAttack";

    // Events
    public event Action<float> OnStaminaChanged;
    public event Action OnPlayerDeath;

    // Components
    private Rigidbody rb;
    private Animator animator;
    private float currentSpeed;
    private bool isRunning = false;
    private bool isDead = false;

    private void Start()
    {
        InitializeComponents();
        currentStamina = maxStamina;
    }

    private void InitializeComponents()
    {
        try
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            if (rb == null)
                throw new MissingComponentException("Rigidbody component eksik!");
            if (animator == null)
                throw new MissingComponentException("Animator component eksik!");
            if (joystick == null)
                throw new MissingComponentException("Joystick referansı eksik!");
        }
        catch (Exception e)
        {
            Debug.LogError($"PlayerController başlatma hatası: {e.Message}");
            enabled = false;
        }
    }

    private void Update()
    {
        if (isDead) return;
        if (IsInHitReact()) return;
        HandleStamina();
    }

    private void HandleStamina()
    {
        if (isRunning && currentStamina > 0)
        {
            currentStamina -= runStaminaCost * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
        }
        else if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }

        OnStaminaChanged?.Invoke(currentStamina / maxStamina);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (IsInHitReact()) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        float inputMagnitude = direction.magnitude;

        // Koşma kontrolü ve stamina kontrolü
        isRunning = inputMagnitude > runThreshold && currentStamina > 0;
        currentSpeed = isRunning ? runSpeed : moveSpeed;

        UpdateAnimations(inputMagnitude);
        ApplyMovement(direction, inputMagnitude);
    }

    private void UpdateAnimations(float inputMagnitude)
    {
        if (animator == null) return;

        float animSpeedValue = 0;
        if (inputMagnitude > 0.1f)
        {
            animSpeedValue = isRunning ? 1.0f : 0.5f;
        }

        animator.SetFloat(speedParameterName, animSpeedValue);
    }

    private void ApplyMovement(Vector3 direction, float inputMagnitude)
    {
        if (inputMagnitude > 0.1f)
        {
            Vector3 move = direction.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            if (rotateToMoveDirection)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, toRotation, 10f * Time.fixedDeltaTime);
            }
        }
    }

    public void Attack()
    {
        if (isDead || animator == null) return;
        animator.SetTrigger(isAttackParameterName);
    }

    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        OnPlayerDeath?.Invoke();
        this.enabled = false;
    }

    public float GetStaminaPercentage()
    {
        return currentStamina / maxStamina;
    }

    private bool IsInHitReact()
    {
        return animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterArmature|HitReact");
    }
}