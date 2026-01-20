using UnityEngine;

public class VoidProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public int damage = 25;
    public float lifetime = 3f;
    
    [Header("Identification")]
    public int ownerPlayerID; // Who shot this?  1 or 2
    
    private Vector2 direction;

    void Start()
    {
        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
        
        Debug.Log("🔥 Void projectile spawned by Player " + ownerPlayerID);
    }

    public void Initialize(Vector2 shootDirection, int playerID)
    {
        direction = shootDirection. normalized;
        ownerPlayerID = playerID;
        
        Debug.Log("⚡ Void initialized:  Direction=" + direction + ", Speed=" + speed + ", Owner=" + playerID);
    }

    void Update()
    {
        // Move the projectile
        transform. Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔥 Void hit:  " + other.name + " (Tag: " + other.tag + ")");
        
        // Check if hit a player
        PlayerController hitPlayer = other.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            // Don't hit the player who shot it! 
            if (hitPlayer.playerID == ownerPlayerID)
            {
                Debug.Log("❌ Hit own player, ignoring");
                return;
            }
            
            // Hit the enemy! 
            Debug.Log("✅ Hit enemy Player " + hitPlayer.playerID + " for " + damage + " damage!");
            
            FighterHealth enemyHealth = other.GetComponent<FighterHealth>();
            if (enemyHealth != null)
            {
                enemyHealth. TakeDamage(damage);
            }
            
            // Destroy the projectile
            Destroy(gameObject);
        }
        
        // Destroy if hit wall/ground
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Debug.Log("💥 Void hit wall/ground");
            Destroy(gameObject);
        }
    }
}