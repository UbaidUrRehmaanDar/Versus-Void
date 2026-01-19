using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    public float width;  // Set this to the full width of your bar (e.g., 200)
    public float height; // Set this to the height of your bar (e.g., 30)

    public void SetMaxHealth(float maxHealth) 
    {
        // Optional: logic if you want to resize the bar based on max health
    }

    // This now accepts 2 arguments to match the FighterHealth script
    public void SetHealth(float health, float maxHealth) 
    {
        // Prevents division by zero
        if (maxHealth <= 0) return;

        float proportion = health / maxHealth;
        
        // This actually changes the size of the red/green bar
        healthBar.sizeDelta = new Vector2(proportion * width, height);
    }
}