using UnityEngine;

public class AIController :  MonoBehaviour
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
    
    [Header("Special Move")]
    public GameObject voidPrefab;         // Drag Void prefab here
    public Transform voidSpawnPoint;      // Auto-created if missing
    public float specialCooldown = 3f;    // Time between special moves
    public float specialMoveChance = 0.2f; // 20% chance to use special when in range
    private float lastSpecialTime = -999f;
    
    [Header("AI State")]
    public AIState currentState = AIState. Idle;
    
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
        Jumping,
        SpecialMove
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
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            
            Debug.Log("AI: Found " + allPlayers.Length + " objects with 'Player' tag.  This AI is:  " + this.gameObject.name);
            
            foreach (GameObject p in allPlayers)
            {
                Debug.Log("AI:  Checking object: " + p.name + " (is this AI?  " + (p == this.gameObject) + ")");
                
                if (p != this.gameObject && ! p.transform.IsChildOf(this.transform))
                {
                    player = p.transform;
                    Debug. Log("AI: Target set to: " + p.name);
                    break;
                }
            }
            
            if (player == null)
            {
                allPlayers = GameObject.FindGameObjectsWithTag("Players");
                foreach (GameObject p in allPlayers)
                {
                    if (p != this.gameObject && !p.transform.IsChildOf(this.transform))
                    {
                        player = p. transform;
                        Debug.Log("AI: Found player with 'Players' tag: " + p.name);
                        break;
                    }
                }
            }
            
            if (player == null)
            {
                Debug.LogWarning("AI: Could not find another Player!  Drag Player 1 into the 'Player' field in Inspector.");
            }
        }
        
        // Create void spawn point if missing
        if (voidSpawnPoint == null)
        {
            GameObject spawnObj = new GameObject("VoidSpawnPoint");
            spawnObj.transform.SetParent(transform);
            spawnObj.transform.localPosition = new Vector3(0.3f, 0.1f, 0f);
            voidSpawnPoint = spawnObj.transform;
            Debug.Log("AI: Auto-created VoidSpawnPoint");
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
                Debug. LogWarning("AI: Animator is missing 'Kick' trigger!");
            if (!HasAnimatorParameter("Special"))
                Debug. LogWarning("AI: Animator is missing 'Special' trigger!  Special moves won't play animation.");
        }
        
        isGrounded = true;
        
        // Force AI to face player initially
        if (player != null && sprite != null)
        {
            sprite. flipX = (player.position. x < transform.position.x);
        }
    }

    void Update()
    {
        if (player == null) 
        {
            Debug.LogWarning("AI: No player reference!");
            return;
        }
        
        // Check if currently attacking (including special move)
        if (anim != null)
        {
            isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Punch") || 
                          anim.GetCurrentAnimatorStateInfo(0).IsName("Kick") ||
                          anim.GetCurrentAnimatorStateInfo(0).IsName("Special");
        }
        else
        {
            isAttacking = false;
        }
        
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        FacePlayer();
        
        if (Time.time >= nextDecisionTime)
        {
            MakeDecision();
            nextDecisionTime = Time.time + reactionTime;
        }
        
        ExecuteState();
        UpdateWalkingAnimation();
    }
    
    void UpdateWalkingAnimation()
    {
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        
        if (anim != null && HasAnimatorParameter("isWalking"))
        {
            anim.SetBool("isWalking", isMoving && isGrounded);
        }
        
        if (anim != null && HasAnimatorParameter("isGrounded"))
        {
            anim.SetBool("isGrounded", isGrounded);
        }
    }
    
    void FacePlayer()
    {
        // FIXED: Correct facing logic
        if (player.position. x < transform.position.x)
            sprite.flipX = true;  // Player is on LEFT → Face LEFT (flipX = true)
        else
            sprite.flipX = false; // Player is on RIGHT → Face RIGHT (flipX = false)
    }
    
    void MakeDecision()
    {
        if (isAttacking)
        {
            currentState = AIState.Attacking;
            return;
        }
        
        // In attack range - decide to attack, special, or retreat
        if (distanceToPlayer <= attackRange)
        {
            // Try special move first if available
            if (CanUseSpecial() && Random.value < specialMoveChance)
            {
                currentState = AIState.SpecialMove;
            }
            else if (CanAttack() && Random.value < aggressiveness)
            {
                currentState = AIState.Attacking;
            }
            else if (Random.value < 0.3f)
            {
                currentState = AIState.Retreating;
            }
            else
            {
                currentState = AIState.Idle;
            }
        }
        // Medium range - might use special move
        else if (distanceToPlayer <= optimalRange)
        {
            if (CanUseSpecial() && Random.value < specialMoveChance * 0.5f)
            {
                currentState = AIState.SpecialMove;
            }
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
        // Too far - approach
        else if (distanceToPlayer > optimalRange)
        {
            currentState = AIState.Approaching;
            
            if (isGrounded && Random.value < 0.1f)
            {
                currentState = AIState.Jumping;
            }
        }
    }
    
    void ExecuteState()
    {
        switch (currentState)
        {
            case AIState. Idle:
                if (! isAttacking)
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    SetAnimSpeed(0);
                }
                break;
                
            case AIState.Approaching:
                MoveTowardsPlayer();
                break;
                
            case AIState. Attacking:
                PerformAttack();
                break;
                
            case AIState.Retreating:
                MoveAwayFromPlayer();
                break;
                
            case AIState. Jumping:
                if (isGrounded)
                {
                    Jump();
                }
                MoveTowardsPlayer();
                break;
                
            case AIState. SpecialMove:
                PerformSpecialMove();
                break;
        }
    }
    
    void MoveTowardsPlayer()
    {
        if (isAttacking)
        {
            rb. linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimSpeed(0);
            return;
        }
        
        float direction = player.position.x < transform.position.x ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity. y);
        SetAnimSpeed(1);
    }
    
    void MoveAwayFromPlayer()
    {
        if (isAttacking)
        {
            rb. linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimSpeed(0);
            return;
        }
        
        float direction = player.position. x < transform.position.x ?  1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed * 0.7f, rb.linearVelocity.y);
        SetAnimSpeed(1);
    }
    
    void SetAnimSpeed(float speed)
    {
        // Animation is now handled by UpdateWalkingAnimation()
    }
    
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        
        if (anim != null && HasAnimatorParameter("isGrounded"))
            anim.SetBool("isGrounded", false);
        
        Debug.Log("AI: Jumped!  isGrounded = " + isGrounded);
    }
    
    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown && !isAttacking;
    }
    
    bool CanUseSpecial()
    {
        return Time.time >= lastSpecialTime + specialCooldown && 
               voidPrefab != null && 
               voidSpawnPoint != null &&
               ! isAttacking;
    }
    
    void PerformAttack()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetFloat("Speed", 0);
        
        if (CanAttack() && distanceToPlayer <= attackRange)
        {
            if (Random.value < 0.5f)
            {
                anim.SetTrigger("Punch");
                Debug.Log("🤖 AI Punch");
            }
            else
            {
                anim.SetTrigger("Kick");
                Debug. Log("🤖 AI Kick");
            }
            
            lastAttackTime = Time. time;
        }
    }
    
    void PerformSpecialMove()
    {
        // Stop moving
        rb. linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (! CanUseSpecial())
        {
            currentState = AIState.Idle;
            return;
        }
        
        lastSpecialTime = Time.time;
        
        // Play animation if exists
        if (anim != null && HasAnimatorParameter("Special"))
        {
            anim.SetTrigger("Special");
        }
        
        Debug.Log("🤖 AI SPECIAL MOVE!");
        
        // Spawn projectile
        GameObject voidObj = Instantiate(voidPrefab, voidSpawnPoint.position, Quaternion.identity);
        
        // Determine shoot direction based on facing
        Vector2 shootDirection = sprite.flipX ? Vector2.left : Vector2.right;
        
        // Initialize projectile
        VoidProjectile projectile = voidObj.GetComponent<VoidProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(shootDirection, 2); // AI is Player 2
        }
        
        currentState = AIState. Idle;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject. CompareTag("Ground"))
        {
            isGrounded = true;
            if (HasAnimatorParameter("isGrounded"))
                anim.SetBool("isGrounded", true);
        }
        
        if (collision.gameObject. CompareTag("Player") || collision.gameObject.CompareTag("Players"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && rb.linearVelocity.y <= 0.1f)
        {
            isGrounded = true;
            if (HasAnimatorParameter("isGrounded"))
                anim. SetBool("isGrounded", true);
        }
    }
    
    private bool HasAnimatorParameter(string paramName)
    {
        if (anim == null) return false;
        
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param. name == paramName)
                return true;
        }
        return false;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color. red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.yellow;
        Gizmos. DrawWireSphere(transform. position, optimalRange);
    }
}