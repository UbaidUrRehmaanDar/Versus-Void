using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;

    [Header("Special Move")]
    public GameObject voidPrefab;
    public Transform voidSpawnPoint;
    public float specialCooldown = 2f;
    private float lastSpecialTime = -999f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Force P2 to face West immediately
        if (playerID == 2) sprite.flipX = true;

        Debug.Log("✅ [" + gameObject.name + "] PlayerController initialized.  PlayerID: " + playerID);
    }

    void Update()
    {
        if (anim == null) return;

        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Punch") ||
                           anim.GetCurrentAnimatorStateInfo(0).IsName("Kick") ||
                           anim.GetCurrentAnimatorStateInfo(0).IsName("Special");
        float moveInput = 0;
        if (playerID == 1)
        {
            if (Input.GetKey(KeyCode.A)) moveInput = -1;
            else if (Input.GetKey(KeyCode.D)) moveInput = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        }

        if (!isAttacking)
        {
            if (rb != null)
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            anim.SetFloat("Speed", Mathf.Abs(moveInput));

            // Handle Sprite Flipping
            if (moveInput > 0) sprite.flipX = false;
            else if (moveInput < 0) sprite.flipX = true;

            // SNAP BACK when idle
            if (moveInput == 0)
            {
                if (playerID == 1) sprite.flipX = false;
                else if (playerID == 2) sprite.flipX = true;
            }
        }
        else
        {
            if (rb != null)
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("Speed", 0);
        }

        // --- ATTACK & JUMP & SPECIAL ---
        if (playerID == 1)
        {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isAttacking) Jump();

            if (Input.GetKeyDown(KeyCode.T))
            {
                anim.SetTrigger("Punch");
                Debug.Log("🥊 Player 1 Punch");
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                anim.SetTrigger("Kick");
                Debug.Log("🦵 Player 1 Kick");
            }
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastSpecialTime + specialCooldown)
            {
                PerformSpecialMove();
            }
        }
        else if (playerID == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isAttacking) Jump();

            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetTrigger("Punch");
                Debug.Log("🥊 Player 2 Punch");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("Kick");
                Debug.Log("🦵 Player 2 Kick");
            }
            if (Input.GetKeyDown(KeyCode.I) && Time.time >= lastSpecialTime + specialCooldown)
            {
                PerformSpecialMove();
            }
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
        }
    }

    void PerformSpecialMove()
    {
        if (voidPrefab == null)
        {
            Debug.LogError("❌ Void prefab not assigned!");
            return;
        }

        if (voidSpawnPoint == null)
        {
            Debug.LogError("❌ Void spawn point not assigned!");
            return;
        }

        lastSpecialTime = Time.time;

        // ADD THIS LINE - Play the hadoken animation
        if (anim != null)
            anim.SetTrigger("Special");

        Debug.Log("🔥 Player " + playerID + " used SPECIAL MOVE!");

        // Spawn the projectile
        GameObject voidObj = Instantiate(voidPrefab, voidSpawnPoint.position, Quaternion.identity);

        Vector2 shootDirection = sprite.flipX ? Vector2.left : Vector2.right;

        Debug.Log("🎯 Shoot direction: " + shootDirection);

        VoidProjectile projectile = voidObj.GetComponent<VoidProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(shootDirection, playerID);
        }
        else
        {
            Debug.LogError("❌ VoidProjectile component not found!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (anim != null)
                anim.SetBool("isGrounded", true);
        }
    }
}