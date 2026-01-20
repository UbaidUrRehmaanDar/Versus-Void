using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        // Make sure no old CampaignManager exists
        if (CampaignManager.instance != null)
        {
            Destroy(CampaignManager.instance.gameObject);
        }
    }

    // Called by "PLAY" button - Starts Campaign Mode (vs AI)
    public void StartCampaign()
    {
        // Create CampaignManager before loading character select
        GameObject campaignObj = new GameObject("CampaignManager");
        campaignObj. AddComponent<CampaignManager>();
        
        Debug.Log(" Starting Campaign Mode");
        SceneManager.LoadScene("AI_CharacterSelect");
    }

    // Called by "1v1" button - Starts Versus Mode (2 players)
    public void Start1v1()
    {
        Debug.Log(" Starting 1v1 Mode");
        SceneManager.LoadScene("1v1CharacterSelect");
    }

    // Called by "OPTIONS" button (not implemented yet)
    public void OpenOptions()
    {
        Debug.Log(" Options menu - Coming soon!");
        // TODO: Load options scene or open options panel
    }

    // Called by "EXIT" button
    public void ExitGame()
    {
        Debug.Log(" Game is exiting...");
        Application.Quit();
        
        // For testing in Unity Editor: 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication. isPlaying = false;
        #endif
    }
}