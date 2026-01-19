using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Transform attackPoint; 

    [Header("Combat Settings")]
    public float attackRange = 0.5f;
    public LayerMask enemyLayers; 

    // REMOVED Update() function to stop both players attacking at once

    public void PerformAttack(int damage, string triggerName)
    {
        // 1. Play the animation
        animator.SetTrigger(triggerName);

        // 2. Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // 3. Damage the enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            FighterHealth enemyHealth = enemy.GetComponent<FighterHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}