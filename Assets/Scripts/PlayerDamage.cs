using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float punchDamage = 10f;
    public float kickDamage = 15f;
    public float knockbackForce = 5f;
    
    private Animator anim;
    private FighterHealth health;
    private PlayerController playerController;
    private string enemyAttackTag;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<FighterHealth>();
        playerController = GetComponent<PlayerController>();
        
        // Determine which attack tag to respond to
        if (playerController != null)
        {
            if (playerController.playerID == 1)
            {
                enemyAttackTag = "P2_Attack"; // Player 1 gets hit by P2's attacks
                Debug.Log("✅ Player 1 initialized - Will respond to:  P2_Attack");
            }
            else if (playerController. playerID == 2)
            {
                enemyAttackTag = "P1_Attack"; // Player 2 gets hit by P1's attacks
                Debug.Log("✅ Player 2 initialized - Will respond to: P1_Attack");
            }
        }
        else
        {
            Debug.LogError("❌ PlayerController not found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug. Log("🔔 [" + gameObject.name + "] Detected:  " + other.name + " (Tag: " + other.tag + ")");
        
        // Only respond to ENEMY attacks
        if (other.CompareTag(enemyAttackTag))
        {
            Debug.Log("✅ [" + gameObject.name + "] VALID HIT!");
            
            // Determine damage
            float damage = punchDamage;
            if (other.name.Contains("Kick") || other.name.ToLower().Contains("kick"))
            {
                damage = kickDamage;
            }
            
            // Apply damage
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug. Log("💥 [" + gameObject.name + "] HP: " + health.currentHealth);
            }
            
            // Play animation
            if (anim != null)
            {
                anim.SetTrigger("TakeDamage");
            }
            
            // Knockback
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
                rb.AddForce(knockbackDir * knockbackForce, ForceMode2D. Impulse);
            }
        }
        else
        {
            Debug.Log("❌ [" + gameObject.name + "] Wrong tag!  Ignoring.");
        }
    }
}