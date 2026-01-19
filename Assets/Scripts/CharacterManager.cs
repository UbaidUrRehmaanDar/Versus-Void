using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    [Header("Selection Data")]
    public int p1SelectedCharacter = -1;
    public int p2SelectedCharacter = -1;
    public string selectedStage;

    void Awake() 
    {
        // Singleton pattern: ensures this object persists between scenes
        if (instance == null) 
        { 
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }

    // Called by the Stage buttons in your Menu
    public void SelectStage(string stageName) 
    {
        selectedStage = stageName;
        Debug.Log("Stage selected: " + stageName);
    }

    // Called by the Start Fight button
    public void LaunchFight()
    {
        if (!string.IsNullOrEmpty(selectedStage)) 
        {
            Time.timeScale = 1f; // Ensure game isn't paused from a previous round
            SceneManager.LoadScene(selectedStage);
        }
        else 
        {
            Debug.LogError("Cannot launch: No stage selected!");
        }
    }

    // This is used by the TimerUI to get the winner's name for the popup
    public string GetWinnerMessage()
    {
        float p1HP = 0;
        float p2HP = 0;

        // Find all objects with FighterHealth in the current fight scene
        FighterHealth[] allHealths = FindObjectsByType<FighterHealth>(FindObjectsSortMode.None);
        
        foreach(var h in allHealths) 
        {
            PlayerController pc = h.GetComponent<PlayerController>();
            if (pc != null)
            {
                if(pc.playerID == 1) p1HP = h.currentHealth;
                else if(pc.playerID == 2) p2HP = h.currentHealth;
            }
        }

        if (p1HP > p2HP) return "PLAYER 1 WINS!";
        if (p2HP > p1HP) return "PLAYER 2 WINS!";
        return "IT'S A DRAW!";
    }

    // Useful for a "Return to Menu" button
    public void ResetData()
    {
        p1SelectedCharacter = -1;
        p2SelectedCharacter = -1;
        selectedStage = "";
        Time.timeScale = 1f;
    }
}