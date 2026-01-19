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

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<FighterHealth>();
        playerController = GetComponent<PlayerController>();
        
        Debug.Log("[" + gameObject.name + "] PlayerDamage initialized.  PlayerID: " + (playerController != null ? playerController.playerID. ToString() : "NULL"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing that hit us is tagged "Attack"
        if (other.CompareTag("Attack"))
        {
            Debug.Log("[" + gameObject. name + "] Detected attack from: " + other.gameObject.name);
            
            // ⭐ CRITICAL FIX: Check if this attack belongs to THIS player
            // PunchHitbox is a CHILD of the player, so we check the parent
            Transform attackParent = other.transform.parent;
            
            if (attackParent == this.transform)
            {
                Debug.Log("[" + gameObject.name + "] ❌ IGNORED - This is MY OWN attack!  (Parent match)");
                return; // Don't hit yourself!
            }
            
            // EXTRA CHECK: Compare PlayerID if both have PlayerController
            if (playerController != null && attackParent != null)
            {
                PlayerController attackerController = attackParent.GetComponent<PlayerController>();
                if (attackerController != null && attackerController.playerID == playerController. playerID)
                {
                    Debug.Log("[" + gameObject.name + "] ❌ IGNORED - Same PlayerID detected!");
                    return;
                }
            }
            
            Debug.Log("[" + gameObject. name + "] ✅ VALID HIT from enemy!");
            
            // Determine damage based on attack type
            float damage = punchDamage;
            if (other.name.ToLower().Contains("kick") || other.name.ToLower().Contains("foot"))
            {
                damage = kickDamage;
            }
            
            // Apply damage to health
            if (health != null)
            {
                health. TakeDamage(damage);
                Debug.Log("[" + gameObject.name + "] Health reduced by " + damage);
            }
            else
            {
                Debug.LogError("[" + gameObject.name + "] No FighterHealth component!");
            }
            
            // Trigger the animation!
            if (anim != null)
            {
                anim.SetTrigger("TakeDamage");
                Debug.Log("[" + gameObject.name + "] TakeDamage animation triggered");
            }
            else
            {
                Debug.LogError("[" + gameObject.name + "] No Animator component!");
            }
            
            // Knockback effect
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
                rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}