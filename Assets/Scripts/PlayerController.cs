using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID = 1; 
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;

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
                           anim. GetCurrentAnimatorStateInfo(0).IsName("Kick");

        float moveInput = 0;
        if (playerID == 1) {
            if (Input.GetKey(KeyCode.A)) moveInput = -1;
            else if (Input.GetKey(KeyCode.D)) moveInput = 1;
        } else {
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        }

        if (! isAttacking) {
            if (rb != null)
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            anim.SetFloat("Speed", Mathf. Abs(moveInput));
            
            // Handle Sprite Flipping
            if (moveInput > 0) sprite.flipX = false;
            else if (moveInput < 0) sprite.flipX = true;

            // SNAP BACK when idle
            if (moveInput == 0)
            {
                if (playerID == 1) sprite.flipX = false;
                else if (playerID == 2) sprite.flipX = true;
            }
        } else {
            if (rb != null)
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("Speed", 0);
        }

        // --- ATTACK & JUMP ---
        if (playerID == 1) 
        {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded && ! isAttacking) Jump();
            
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
        } 
        else if (playerID == 2) 
        {
            if (Input.GetKeyDown(KeyCode. UpArrow) && isGrounded && !isAttacking) Jump();
            
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
        }
    }

    void Jump() {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            if (anim != null)
                anim.SetBool("isGrounded", true);
        }
    }
}