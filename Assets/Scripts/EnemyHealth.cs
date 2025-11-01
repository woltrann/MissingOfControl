using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider; // UI Slider
    Animator anim;
    bool isDead = false;

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

        Debug.Log($"Düşman hasar aldı: {amount}, kalan can: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;

        // Ölüm animasyonu bitince objeyi sil
        Destroy(gameObject, 3f);
    }
}
