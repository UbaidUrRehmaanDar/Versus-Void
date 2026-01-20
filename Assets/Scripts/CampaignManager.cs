using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager instance;
    
    [Header("Campaign Progress")]
    public int currentStage = 1;
    public int playerSelectedCharacter = 0; // 0, 1, 2, or 3
    
    [Header("Scene Names")]
    public string stage1Scene = "Stage1_AI";
    public string stage2Scene = "Stage2_AI";
    public string stage3Scene = "Stage3_AI";
    public string victoryScene = "VictoryScreen";
    public string mainMenuScene = "MainMenu";
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ CampaignManager created");
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
        
        Debug.Log("🎮 CAMPAIGN STARTED! Player chose character:  " + playerCharacterID);
        
        LoadNextStage();
    }
    
    public void PlayerWon()
    {
        Debug.Log("🏆 Player WON Stage " + currentStage + "!");
        
        currentStage++;
        
        if (currentStage == 2)
        {
            Debug. Log("⏭️ Loading Stage 2");
            SceneManager.LoadScene(stage2Scene);
        }
        else if (currentStage == 3)
        {
            Debug. Log("⏭️ Loading Stage 3");
            SceneManager.LoadScene(stage3Scene);
        }
        else if (currentStage == 4)
        {
            Debug.Log("🎉 CAMPAIGN COMPLETE!");
            SceneManager.LoadScene(victoryScene);
            ResetCampaign();
        }
    }
    
    public void PlayerLost()
    {
        Debug.Log("💀 Player LOST at Stage " + currentStage);
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
        playerSelectedCharacter = 0;
        Debug.Log("🔄 Campaign Reset");
    }
    
    public int GetCurrentOpponentCharacterID()
    {
        // Stage 1 → Fight Character 2 (array index 1)
        // Stage 2 → Fight Character 3 (array index 2)
        // Stage 3 → Fight Character 4 (array index 3)
        
        int opponentID = currentStage; // Stage 1 = opponent 1, Stage 2 = opponent 2, etc.
        
        Debug.Log("🤖 Stage " + currentStage + " opponent character ID: " + opponentID);
        
        return opponentID;
    }
}