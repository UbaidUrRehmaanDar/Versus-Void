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

    public void SelectStage(string stageName) 
    {
        selectedStage = stageName;
        Debug.Log("🏟️ Stage selected: " + stageName);
    }

    public void LaunchFight()
    {
        if (p1SelectedCharacter == -1 || p2SelectedCharacter == -1)
        {
            Debug.LogError("❌ Cannot launch:  Characters not selected!");
            return;
        }
        
        if (string.IsNullOrEmpty(selectedStage)) 
        {
            Debug.LogError("❌ Cannot launch: No stage selected!");
            return;
        }
        
        Debug.Log("🚀 Launching fight!  P1=" + p1SelectedCharacter + " P2=" + p2SelectedCharacter + " Stage=" + selectedStage);
        Time.timeScale = 1f;
        SceneManager.LoadScene(selectedStage);
    }

    public string GetWinnerMessage()
    {
        float p1HP = 0;
        float p2HP = 0;

        FighterHealth[] allHealths = FindObjectsByType<FighterHealth>(FindObjectsSortMode. None);
        
        foreach(var h in allHealths) 
        {
            PlayerController pc = h.GetComponent<PlayerController>();
            if (pc != null)
            {
                if(pc.playerID == 1) p1HP = h.currentHealth;
                else if(pc. playerID == 2) p2HP = h.currentHealth;
            }
        }

        if (p1HP > p2HP) return "PLAYER 1 WINS!";
        if (p2HP > p1HP) return "PLAYER 2 WINS!";
        return "IT'S A DRAW!";
    }

    public void ResetData()
    {
        p1SelectedCharacter = -1;
        p2SelectedCharacter = -1;
        selectedStage = "";
        Time. timeScale = 1f;
    }
}