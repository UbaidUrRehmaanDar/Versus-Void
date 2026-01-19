using UnityEngine;

public class HitboxController : MonoBehaviour
{
    [Header("Hitbox Reference")]
    public GameObject hitbox; // Drag P1_HitBox or P2_HitBox here
    
    [Header("Debug")]
    public bool showDebugLogs = true;
    
    // Called from animation events to ENABLE hitbox
    public void EnableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true);
            if (showDebugLogs)
                Debug.Log("⚔️ [" + gameObject. name + "] Hitbox ENABLED");
        }
        else
        {
            Debug.LogError("❌ [" + gameObject. name + "] Hitbox reference is NULL!");
        }
    }
    
    // Called from animation events to DISABLE hitbox
    public void DisableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
            if (showDebugLogs)
                Debug.Log("🛡️ [" + gameObject. name + "] Hitbox DISABLED");
        }
    }
    
    // Safety:  Disable hitbox when object is disabled
    void OnDisable()
    {
        if (hitbox != null)
            hitbox.SetActive(false);
    }
}