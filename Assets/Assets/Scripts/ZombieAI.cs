using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>(); // Player'ýn health component'ini alýyoruz
    }

    void Update()
    {
        //if (playerHealth.isDead) // Eðer oyuncu ölmüþse hiçbir þey yapma
        //{
        //    animator.SetBool("isAttacking", false);
        //    agent.isStopped = true;
        //    return;
        //}

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange) // Oyuncuya saldýrmak için yakýnsa
        {
            agent.isStopped = true; // Zombi hareket etmesin
            animator.SetBool("isWalking", false); // Yürümeyi durdur
            animator.SetBool("isAttacking", true); // Saldýrýyý baþlat

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown; // Attack cooldown

                // Oyuncuya hasar ver
                playerHealth.TakeDamage(attackDamage);
            }
        }
        else if (distance <= detectionRange) // Oyuncu yakýnsa zombi onu takip etsin
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false); // Saldýrý durdurulsun
        }
        else // Oyuncu çok uzaksa zombi duracak
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false); // Saldýrýyý durdur
        }
    }

    public void Die()
    {
        agent.isStopped = true; // Zombi duracak
        animator.SetBool("isDead", true); // Ölüm animasyonu
        Destroy(gameObject, 5f); // Ölüm animasyonu sonrasý zombi yok edilsin
    }
}