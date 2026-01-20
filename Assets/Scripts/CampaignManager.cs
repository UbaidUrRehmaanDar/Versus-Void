using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager :  MonoBehaviour
{
    public static CampaignManager instance;
    
    [Header("Campaign Progress")]
    public int currentStage = 1; // Which AI opponent (1, 2, 3, or 4)
    public int playerSelectedCharacter = -1; // Player's character choice
    
    [Header("Scene Names")]
    public string stage1Scene = "Stage1_AI";  // Fight Character 2
    public string stage2Scene = "Stage2_AI";  // Fight Character 3
    public string stage3Scene = "Stage3_AI";  // Fight Character 4
    public string victoryScene = "VictoryScreen"; // Final victory
    public string mainMenuScene = "MainMenu";
    
    void Awake()
    {
        // Singleton pattern - persist across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ CampaignManager created and persisting across scenes");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void StartCampaign(int playerCharacterID)
    {
        currentStage = 1;
        playerSelectedCharacter = playerCharacterID;
        
        Debug.Log("🎮 Campaign Started! Player chose character: " + playerCharacterID);
        
        LoadNextStage();
    }
    
    public void PlayerWon()
    {
        Debug.Log("🏆 Player WON Stage " + currentStage + "!");
        
        currentStage++;
        
        if (currentStage == 2)
        {
            // Beat Character 2 → Fight Character 3
            Debug.Log("⏭️ Loading Stage 2 (Character 3)");
            SceneManager.LoadScene(stage2Scene);
        }
        else if (currentStage == 3)
        {
            // Beat Character 3 → Fight Character 4
            Debug.Log("⏭️ Loading Stage 3 (Character 4)");
            SceneManager.LoadScene(stage3Scene);
        }
        else if (currentStage == 4)
        {
            // Beat Character 4 → VICTORY! 
            Debug.Log("🎉 CAMPAIGN COMPLETE!");
            SceneManager.LoadScene(victoryScene);
            ResetCampaign();
        }
    }
    
    public void PlayerLost()
    {
        Debug. Log("💀 Player LOST at Stage " + currentStage);
        Debug.Log("↩️ Returning to Main Menu");
        
        ResetCampaign();
        SceneManager.LoadScene(mainMenuScene);
    }
    
    void LoadNextStage()
    {
        if (currentStage == 1)
        {
            SceneManager.LoadScene(stage1Scene);
        }
        else if (currentStage == 2)
        {
            SceneManager.LoadScene(stage2Scene);
        }
        else if (currentStage == 3)
        {
            SceneManager.LoadScene(stage3Scene);
        }
    }
    
    public void ResetCampaign()
    {
        currentStage = 1;
        playerSelectedCharacter = -1;
        Debug.Log("🔄 Campaign Reset");
    }
    
    public int GetCurrentOpponentCharacterID()
    {
        // Stage 1 = Character 2 (ID 1)
        // Stage 2 = Character 3 (ID 2)
        // Stage 3 = Character 4 (ID 3)
        return currentStage; // Assuming character IDs match stage numbers
    }
}