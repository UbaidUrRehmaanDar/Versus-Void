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
    private PlayerCombat combat; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        combat = GetComponent<PlayerCombat>(); 

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Force P2 to face West immediately
        if (playerID == 2) sprite.flipX = true; 
    }

    void Update()
    {
        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Punch") || 
                           anim.GetCurrentAnimatorStateInfo(0).IsName("Kick");

        float moveInput = 0;
        if (playerID == 1) {
            if (Input.GetKey(KeyCode.A)) moveInput = -1;
            else if (Input.GetKey(KeyCode.D)) moveInput = 1;
        } else {
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        }

        if (!isAttacking) {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            
            // 1. Handle Sprite Flipping during movement (Turning away to walk)
            if (moveInput > 0) sprite.flipX = false;
            else if (moveInput < 0) sprite.flipX = true;

            // 2. SNAP BACK: When no button is held, face the opponent again
            if (moveInput == 0)
            {
                if (playerID == 1) sprite.flipX = false; // P1 faces East
                else if (playerID == 2) sprite.flipX = true; // P2 faces West
            }
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("Speed", 0);
        }

        // --- ATTACK & JUMP CONTROL ---
        if (playerID == 1) 
        {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isAttacking) Jump();
            
            if (Input.GetKeyDown(KeyCode.T)) combat.PerformAttack(10, "Punch");
            if (Input.GetKeyDown(KeyCode.Y)) combat.PerformAttack(20, "Kick");
        } 
        else if (playerID == 2) 
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isAttacking) Jump();
            
            if (Input.GetKeyDown(KeyCode.J)) combat.PerformAttack(10, "Punch");
            if (Input.GetKeyDown(KeyCode.K)) combat.PerformAttack(20, "Kick");
        }
    }

    void Jump() {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        anim.SetBool("isGrounded", false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            anim.SetBool("isGrounded", true);
        }
    }
}