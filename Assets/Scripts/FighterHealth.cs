using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("UI")]
    public HealthBarUI healthBar;
    
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
        
        Debug.Log("💚 [" + gameObject.name + "] Health initialized:  " + currentHealth + "/" + maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        float oldHealth = currentHealth;
        currentHealth -= damage;
        
        if (currentHealth < 0)
            currentHealth = 0;
        
        Debug.Log("💥 [" + gameObject.name + "] Took " + damage + " damage!  HP: " + oldHealth + " → " + currentHealth);
        
        // Update health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
        
        // Check if dead
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        Debug.Log("💀 [" + gameObject.name + "] DIED!");
        
        // End the match
        if (GameManager.instance != null)
        {
            Debug.Log("🏁 Calling GameManager.EndMatch()");
            GameManager.instance.EndMatch();
        }
        else
        {
            Debug.LogError("❌ GameManager not found!  Can't end match!");
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }
}