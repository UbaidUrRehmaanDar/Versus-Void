using UnityEngine;

public class FighterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    
    public HealthBarUI healthBar;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar. SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Don't take damage if already dead
        
        currentHealth -= damage;
        currentHealth = Mathf. Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("Killed");
            Debug.Log("💀 " + gameObject.name + " has been KO'd!");
            
            // END THE MATCH! 
            if (GameManager.instance != null)
            {
                GameManager. instance.EndMatch();
            }
        }
        else
        {
            anim.SetTrigger("TakeDamage2");
        }
    }
}