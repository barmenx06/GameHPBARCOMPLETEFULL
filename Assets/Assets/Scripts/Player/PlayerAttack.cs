using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public float attackCooldown = 1f;
    private float nextAttackTime;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            animator.SetTrigger("isAttack");  // ← Animator'daki Trigger parametresi
            
        }
    }

    // Yeni eklenen fonksiyon: HitReact animasyonunu tetikler
    public void TriggerHitReact()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit"); // Animator'da "Hit" trigger'ı olmalı
        }
    }
}
