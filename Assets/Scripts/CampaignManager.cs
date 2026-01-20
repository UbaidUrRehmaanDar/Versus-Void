using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager :  MonoBehaviour
{
    public static CampaignManager instance;
    
    [Header("Campaign Progress")]
    public int currentStage = 1;
    public int playerSelectedCharacter = 0;
    
    [Header("Scene Names")]
    public string stage1Scene = "Stage1_AI";
    public string stage2Scene = "Stage2_AI";
    public string stage3Scene = "Stage3_AI";
    public string victoryScene = "MainMenu";
    public string mainMenuScene = "MainMenu";
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ CampaignManager created and persistent");
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
        
        Debug. Log("==========================================");
        Debug.Log(" CAMPAIGN STARTED!");
        Debug.Log("Player Character: " + playerCharacterID);
        Debug.Log("Starting Stage:  " + currentStage);
        Debug.Log("==========================================");
        
        LoadNextStage();
    }
    
    public void PlayerWon()
    {
        Debug.Log("==========================================");
        Debug.Log(" PlayerWon() CALLED!");
        Debug.Log("Current Stage BEFORE:  " + currentStage);
        
        currentStage++;
        
        Debug.Log("Current Stage AFTER: " + currentStage);
        Debug.Log("==========================================");
        
        if (currentStage == 2)
        {
            Debug.Log(" Loading Stage 2: " + stage2Scene);
            SceneManager.LoadScene(stage2Scene);
        }
        else if (currentStage == 3)
        {
            Debug.Log(" Loading Stage 3: " + stage3Scene);
            SceneManager.LoadScene(stage3Scene);
        }
        else if (currentStage >= 4)
        {
            Debug.Log(" CAMPAIGN COMPLETE!");
            SceneManager.LoadScene(victoryScene);
            ResetCampaign();
        }
    }
    
    public void PlayerLost()
    {
        Debug. Log("==========================================");
        Debug.Log(" PlayerLost() CALLED!");
        Debug.Log("Lost at Stage:  " + currentStage);
        Debug.Log("Returning to Main Menu");
        Debug.Log("==========================================");
        
        ResetCampaign();
        SceneManager.LoadScene(mainMenuScene);
    }
    
    void LoadNextStage()
    {
        Debug.Log(" LoadNextStage() - Stage " + currentStage);
        
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
        Debug.Log(" Campaign Reset");
    }
    
    public int GetCurrentOpponentCharacterID()
    {
        int opponentID = currentStage;
        Debug.Log(" Opponent for Stage " + currentStage + " is Character " + opponentID);
        return opponentID;
    }
}