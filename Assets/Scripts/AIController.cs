using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Drag Player 1 here in Inspector
    
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float jumpForce = 10f;
    public bool isGrounded;
    
    [Header("AI Behavior Settings")]
    public float attackRange = 1.5f;      // Distance to start attacking
    public float optimalRange = 2f;       // Preferred fighting distance
    public float attackCooldown = 0.8f;   // Time between attacks
    public float reactionTime = 0.3f;     // Delay before AI reacts
    public float aggressiveness = 0.7f;   // 0-1, higher = more attacks
    
    [Header("AI State")]
    public AIState currentState = AIState.Idle;
    
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    
    private float lastAttackTime;
    private float nextDecisionTime;
    private float distanceToPlayer;
    private bool isAttacking;
    
    public enum AIState
    {
        Idle,
        Approaching,
        Attacking,
        Retreating,
        Jumping
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Auto-find player if not assigned
        if (player == null)
        {
            // Find ALL objects with "Player" tag and pick the one that isn't this AI
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            
            Debug.Log("AI: Found " + allPlayers.Length + " objects with 'Player' tag. This AI is: " + this.gameObject.name);
            
            foreach (GameObject p in allPlayers)
            {
                Debug.Log("AI: Checking object: " + p.name + " (is this AI? " + (p == this.gameObject) + ")");
                
                // Skip if it's this AI object OR a child of this AI
                if (p != this.gameObject && !p.transform.IsChildOf(this.transform))
                {
                    player = p.transform;
                    Debug.Log("AI: Target set to: " + p.name);
                    break;
                }
            }
            
            // If still not found, try "Players" tag
            if (player == null)
            {
                allPlayers = GameObject.FindGameObjectsWithTag("Players");
                foreach (GameObject p in allPlayers)
                {
                    if (p != this.gameObject && !p.transform.IsChildOf(this.transform))
                    {
                        player = p.transform;
                        Debug.Log("AI: Found player with 'Players' tag: " + p.name);
                        break;
                    }
                }
            }
            
            if (player == null)
            {
                Debug.LogWarning("AI: Could not find another Player! Drag Player 1 into the 'Player' field in Inspector.");
            }
        }
        
        // Debug checks
        if (rb == null) Debug.LogError("AI: Missing Rigidbody2D!");
        if (anim == null) Debug.LogError("AI: Missing Animator!");
        if (sprite == null) Debug.LogError("AI: Missing SpriteRenderer!");
        
        // Check for animator parameters
        if (anim != null)
        {
            if (!HasAnimatorParameter("Speed"))
                Debug.LogWarning("AI: Animator is missing 'Speed' parameter! Walking animation won't play.");
            if (!HasAnimatorParameter("Punch"))
                Debug.LogWarning("AI: Animator is missing 'Punch' trigger!");
            if (!HasAnimatorParameter("Kick"))
                Debug.LogWarning("AI: Animator is missing 'Kick' trigger!");
        }
        
        // Start grounded (so AI can move immediately)
        isGrounded = true;
    }

    void Update()
    {
        if (player == null) 
        {
            Debug.LogWarning("AI: No player reference!");
            return;
        }
        
        // Check if currently attacking (with null check)
        if (anim != null)
        {
            isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Punch") || 
                          anim.GetCurrentAnimatorStateInfo(0).IsName("Kick");
        }
        else
        {
            isAttacking = false;
        }
        
        // Calculate distance to player
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Face the player
        FacePlayer();
        
        // Make decisions at intervals (simulates reaction time)
        if (Time.time >= nextDecisionTime)
        {
            MakeDecision();
            nextDecisionTime = Time.time + reactionTime;
        }
        
        // Execute current behavior
        ExecuteState();
        
        // Always update walking animation based on actual horizontal movement
        UpdateWalkingAnimation();
    }
    
    void UpdateWalkingAnimation()
    {
        // Set walking animation true if moving horizontally AND on ground
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        
        if (anim != null && HasAnimatorParameter("isWalking"))
        {
            // Only walk when grounded, not in air
            anim.SetBool("isWalking", isMoving && isGrounded);
        }
        
        // Update jumping/grounded animation
        if (anim != null && HasAnimatorParameter("isGrounded"))
        {
            anim.SetBool("isGrounded", isGrounded);
            Debug.Log("AI: Setting isGrounded animator to: " + isGrounded);
        }
        else if (anim != null)
        {
            Debug.LogWarning("AI: 'isGrounded' parameter NOT FOUND in Animator!");
        }
    }
    
    void FacePlayer()
    {
        if (player.position.x < transform.position.x)
            sprite.flipX = false;  // Face left
        else
            sprite.flipX = true; // Face right
    }
    
    void MakeDecision()
    {
        // Don't change state while attacking
        if (isAttacking)
        {
            currentState = AIState.Attacking;
            return;
        }
        
        // In attack range - decide to attack or retreat
        if (distanceToPlayer <= attackRange)
        {
            if (CanAttack() && Random.value < aggressiveness)
            {
                currentState = AIState.Attacking;
            }
            else if (Random.value < 0.3f) // Sometimes retreat
            {
                currentState = AIState.Retreating;
            }
            else
            {
                currentState = AIState.Idle;
            }
        }
        // Too far - approach
        else if (distanceToPlayer > optimalRange)
        {
            currentState = AIState.Approaching;
            
            // Random chance to jump while approaching
            if (isGrounded && Random.value < 0.1f)
            {
                currentState = AIState.Jumping;
            }
        }
        // At optimal range - mix of behaviors
        else
        {
            float roll = Random.value;
            if (roll < 0.5f)
                currentState = AIState.Approaching;
            else if (roll < 0.7f)
                currentState = AIState.Idle;
            else
                currentState = AIState.Retreating;
        }
    }
    
    void ExecuteState()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Stop moving
                if (!isAttacking)
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    SetAnimSpeed(0);
                }
                break;
                
            case AIState.Approaching:
                MoveTowardsPlayer();
                break;
                
            case AIState.Attacking:
                PerformAttack();
                break;
                
            case AIState.Retreating:
                MoveAwayFromPlayer();
                break;
                
            case AIState.Jumping:
                if (isGrounded)
                {
                    Jump();
                }
                MoveTowardsPlayer();
                break;
        }
    }
    
    void MoveTowardsPlayer()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimSpeed(0);
            return;
        }
        
        float direction = player.position.x < transform.position.x ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        SetAnimSpeed(1);
    }
    
    void MoveAwayFromPlayer()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimSpeed(0);
            return;
        }
        
        float direction = player.position.x < transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed * 0.7f, rb.linearVelocity.y);
        SetAnimSpeed(1);
    }
    
    // Helper to set animation speed (kept for compatibility but animation is now handled in Update)
    void SetAnimSpeed(float speed)
    {
        // Animation is now handled by UpdateWalkingAnimation() in Update()
    }
    
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        
        // Set animator immediately
        if (anim != null && HasAnimatorParameter("isGrounded"))
            anim.SetBool("isGrounded", false);
        
        Debug.Log("AI: Jumped! isGrounded = " + isGrounded);
    }
    
    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown && !isAttacking;
    }
    
    void PerformAttack()
    {
        // Stop moving during attack
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetFloat("Speed", 0);
        
        if (CanAttack() && distanceToPlayer <= attackRange)
        {
            // Randomly choose punch or kick
            if (Random.value < 0.5f)
            {
                anim.SetTrigger("Punch");
            }
            else
            {
                anim.SetTrigger("Kick");
            }
            
            lastAttackTime = Time.time;
        }
    }
    
    // Ground detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // Only set animator parameter if it exists
            if (HasAnimatorParameter("isGrounded"))
                anim.SetBool("isGrounded", true);
        }
        
        // Ignore collision with player
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Players"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // Only set grounded if we're falling or stationary (not jumping up)
        if (other.CompareTag("Ground") && rb.linearVelocity.y <= 0.1f)
        {
            isGrounded = true;
            // Only set animator parameter if it exists
            if (HasAnimatorParameter("isGrounded"))
                anim.SetBool("isGrounded", true);
        }
    }
    
    // Helper method to check if animator parameter exists
    private bool HasAnimatorParameter(string paramName)
    {
        if (anim == null) return false;
        
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    
    // 
    
    // Visual debug in editor
    void OnDrawGizmosSelected()
    {
        // Attack range (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Optimal range (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, optimalRange);
    }
}
