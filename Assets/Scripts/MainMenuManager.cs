using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        // Destroy old CampaignManager if exists
        if (CampaignManager. instance != null)
        {
            Debug.Log(" Destroying old CampaignManager");
            Destroy(CampaignManager.instance.gameObject);
        }
    }

    public void StartCampaign()
    {
        Debug.Log(" StartCampaign() called from Main Menu");
        
        // Create CampaignManager
        GameObject campaignObj = new GameObject("CampaignManager");
        CampaignManager cm = campaignObj.AddComponent<CampaignManager>();
        
        Debug.Log(" CampaignManager created");
        
        // Load character select
        SceneManager.LoadScene("Stage1_AI");
    }

    public void Start1v1()
    {
        Debug.Log(" Starting 1v1 Mode");
        SceneManager.LoadScene("1v1CharacterSelect");
    }

    public void OpenOptions()
    {
        Debug. Log(" Options - Coming soon!");
    }

    public void ExitGame()
    {
        Debug.Log(" Exiting game");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication. isPlaying = false;
        #endif
    }
}