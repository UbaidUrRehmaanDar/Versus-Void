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
    private AIController aiController;
    private string enemyAttackTag;
    private int myPlayerID;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<FighterHealth>();
        playerController = GetComponent<PlayerController>();
        aiController = GetComponent<AIController>();
        
        // Determine player ID
        if (playerController != null)
        {
            myPlayerID = playerController.playerID;
        }
        else if (aiController != null)
        {
            myPlayerID = 2; // AI is always Player 2
        }
        else
        {
            Debug.LogError("❌ " + gameObject.name + ": No PlayerController or AIController found!");
            myPlayerID = 0;
        }
        
        // Set enemy attack tag
        if (myPlayerID == 1)
        {
            enemyAttackTag = "P2_Attack";
            Debug.Log("✅ " + gameObject.name + " is Player 1 - Will respond to:  P2_Attack");
        }
        else if (myPlayerID == 2)
        {
            enemyAttackTag = "P1_Attack";
            Debug.Log("✅ " + gameObject.name + " is Player 2 - Will respond to: P1_Attack");
        }
        
        // Verify health component
        if (health == null)
        {
            Debug.LogError("❌ " + gameObject.name + ": Missing FighterHealth component!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔔 [" + gameObject.name + "] Trigger detected:  " + other.name + " (Tag: " + other.tag + ")");
        
        // Only respond to ENEMY attacks
        if (other.CompareTag(enemyAttackTag))
        {
            Debug. Log("✅ [" + gameObject.name + "] VALID HIT from " + other.name + "!");
            
            // Determine damage
            float damage = punchDamage;
            if (other.name. Contains("Kick") || other.name.ToLower().Contains("kick"))
            {
                damage = kickDamage;
                Debug.Log("🦵 Kick detected! Damage: " + damage);
            }
            else
            {
                Debug.Log("🥊 Punch detected! Damage: " + damage);
            }
            
            // Apply damage
            if (health != null)
            {
                float healthBefore = health.currentHealth;
                health.TakeDamage(damage);
                Debug.Log("💥 [" + gameObject.name + "] HP:  " + healthBefore + " → " + health.currentHealth);
            }
            else
            {
                Debug.LogError("❌ [" + gameObject.name + "] Cannot apply damage - FighterHealth is null!");
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
                rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Debug. Log("❌ [" + gameObject.name + "] Wrong tag!  Expected: " + enemyAttackTag + ", Got: " + other.tag);
        }
    }
}