using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider; // UI Slider

    Animator anim;
    public bool isDead = false;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log($"Player hasar aldý: {amount}, kalan can: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        Debug.Log("Player öldü!");
        EnemyMovement.instance.anim.SetBool("Attack", false);
        EnemyMovement.instance.isAttacking = false;
        GameManager.instance.GameOver(); // GameManag
        // Karakter kontrolünü kapat
        PlayerMovements pm = GetComponent<PlayerMovements>();
        if (pm != null) pm.enabled = false;

        // Ýstersen birkaç saniye sonra sahneyi resetle
        // UnityEngine.SceneManagement.SceneManager.LoadScene(
        //     UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
