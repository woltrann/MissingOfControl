using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    public static EnemyMovement instance;
    public Transform target;
    public float speed = 2f;
    public float detectionRange = 5f;
    public int attackDamage = 10;
    public float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;
    public bool isAttacking = false;

    public Animator anim;
    private PlayerHealth playerHealth; // Player’ın can scriptini tutacağız

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return; // saldırı halindeyken hareket etme

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > detectionRange) return;

        // Player’a doğru yürü
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
                anim.SetBool("Attack", true);
                isAttacking = true;
                playerHealth = other.GetComponent<PlayerHealth>();
            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Attack", false);
            isAttacking = false;
        }
    }

    // 🔥 Bu fonksiyonu animasyonun tam vurma anına event olarak ekle!
    public void DealDamage()
    {
        if (playerHealth != null && Time.time >= nextAttackTime)
        {
            playerHealth.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
