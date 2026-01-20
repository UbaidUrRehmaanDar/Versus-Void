using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    
    public HealthBarUI healthBar; // Drag your HealthBar object here

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // DEBUG: Watch the console to see the numbers drop!
        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            anim.SetTrigger("Killed");
            Debug.Log(gameObject.name + " has been KO'd!");
        }
        else
        {
            // SYNCED: Using your exact trigger name
            anim.SetTrigger("TakeDamage2"); 
        }
    }
}