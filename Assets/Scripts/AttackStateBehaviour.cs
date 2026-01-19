using UnityEngine;

public class AttackStateBehaviour : StateMachineBehaviour
{
    private GameObject hitbox;
    private bool hitboxFound = false;
    
    // When animation state STARTS
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the hitbox if we haven't yet
        if (!hitboxFound)
        {
            // Look for child named "PunchHitbox" or "HitBox"
            Transform hitboxTransform = animator.transform. Find("PunchHitbox");
            if (hitboxTransform == null)
                hitboxTransform = animator.transform.Find("HitBox");
            
            if (hitboxTransform != null)
            {
                hitbox = hitboxTransform.gameObject;
                hitboxFound = true;
                Debug.Log("✅ AttackState found hitbox:  " + hitbox.name);
            }
            else
            {
                Debug.LogError("❌ AttackState: Could not find hitbox!");
            }
        }
        
        // Enable hitbox
        if (hitbox != null)
        {
            hitbox.SetActive(true);
            Debug.Log("⚔️ HITBOX ENABLED by State Machine");
        }
    }
    
    // When animation state ENDS
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Disable hitbox
        if (hitbox != null)
        {
            hitbox.SetActive(false);
            Debug.Log("🛡️ HITBOX DISABLED by State Machine");
        }
    }
}