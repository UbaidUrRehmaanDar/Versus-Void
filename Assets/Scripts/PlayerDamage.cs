using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float punchDamage = 10f;
    public float kickDamage = 15f;
    public float knockbackForce = 5f;
    
    private Animator anim;
    private FighterHealth health;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<FighterHealth>();
    }

    // This detects when a "Trigger" collider (the fist) enters this player
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing that hit us is tagged "Attack"
        if (other.CompareTag("Attack"))
        {
            // Determine damage based on attack type
            float damage = punchDamage;
            if (other.name.ToLower().Contains("kick") || other.name.ToLower().Contains("foot"))
            {
                damage = kickDamage;
            }
            
            // Apply damage to health
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            
            // Trigger the animation!
            if (anim != null)
            {
                anim.SetTrigger("TakeDamage");
            }
            
            Debug.Log(gameObject.name + " was hit by: " + other.name + " for " + damage + " damage!");
            
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