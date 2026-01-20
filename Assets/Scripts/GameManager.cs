using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("End Game UI")]
    public GameObject endGamePanel;
    public GameObject player1WinsImage;
    public GameObject player2WinsImage;
    public GameObject drawImage;
    
    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public float endGameDelay = 3f;
    
    [Header("Campaign Mode")]
    public bool isCampaignMode = false;
    
    private bool gameEnded = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (endGamePanel != null)
            endGamePanel.SetActive(false);
        
        if (player1WinsImage != null) player1WinsImage.SetActive(false);
        if (player2WinsImage != null) player2WinsImage.SetActive(false);
        if (drawImage != null) drawImage.SetActive(false);
        
        Debug.Log("🎮 GameManager started.  Campaign Mode: " + isCampaignMode);
    }

    public void EndMatch()
    {
        if (gameEnded)
        {
            Debug.Log("⚠️ Match already ended, ignoring duplicate call");
            return;
        }
        
        gameEnded = true;
        
        Debug.Log("🏁 MATCH ENDED!");
        
        ShowEndGame();
    }

    void ShowEndGame()
    {
        if (endGamePanel != null)
        {
            endGamePanel. SetActive(true);
            Debug.Log("✅ End game panel activated");
        }
        else
        {
            Debug.LogError("❌ endGamePanel is NULL!");
        }

        string result = GetWinner();
        
        Debug.Log("🏆 Winner: " + result);

        if (result == "PLAYER 1 WINS!")
        {
            if (player1WinsImage != null) player1WinsImage.SetActive(true);
            
            if (isCampaignMode && CampaignManager.instance != null)
            {
                StartCoroutine(CampaignWin());
                return;
            }
        }
        else if (result == "PLAYER 2 WINS!")
        {
            if (player2WinsImage != null) player2WinsImage.SetActive(true);
            
            if (isCampaignMode && CampaignManager.instance != null)
            {
                StartCoroutine(CampaignLoss());
                return;
            }
        }
        else
        {
            if (drawImage != null) drawImage.SetActive(true);
            
            if (isCampaignMode && CampaignManager.instance != null)
            {
                StartCoroutine(CampaignLoss());
                return;
            }
        }

        StartCoroutine(ReturnToMenu());
    }

    string GetWinner()
    {
        float p1HP = -1;
        float p2HP = -1;

        FighterHealth[] allHealths = FindObjectsByType<FighterHealth>(FindObjectsSortMode.None);
        
        Debug.Log("🔍 Found " + allHealths.Length + " FighterHealth components");
        
        foreach (var h in allHealths)
        {
            string objName = h.gameObject.name;
            Debug.Log("   Checking: " + objName + " HP: " + h.currentHealth);
            
            // Check by PlayerController
            PlayerController pc = h.GetComponent<PlayerController>();
            if (pc != null)
            {
                if (pc.playerID == 1)
                {
                    p1HP = h.currentHealth;
                    Debug.Log("   ✅ This is Player 1");
                }
                else if (pc.playerID == 2)
                {
                    p2HP = h.currentHealth;
                    Debug.Log("   ✅ This is Player 2");
                }
            }
            // Check by AIController
            else
            {
                AIController ai = h.GetComponent<AIController>();
                if (ai != null)
                {
                    p2HP = h.currentHealth;
                    Debug.Log("   ✅ This is AI (Player 2)");
                }
            }
        }
        
        // Fallback:  check by name
        if (p1HP < 0 || p2HP < 0)
        {
            Debug.LogWarning("⚠️ Couldn't identify players by script, trying by name.. .");
            
            foreach (var h in allHealths)
            {
                if (h.gameObject.name. Contains("Player1") || h.gameObject.name == "Player1")
                {
                    p1HP = h. currentHealth;
                    Debug. Log("   Found Player1 by name:  HP = " + p1HP);
                }
                if (h.gameObject.name. Contains("Player2") || h.gameObject.name == "Player2")
                {
                    p2HP = h.currentHealth;
                    Debug.Log("   Found Player2 by name: HP = " + p2HP);
                }
            }
        }

        Debug.Log("💚 FINAL HP - P1: " + p1HP + " | P2: " + p2HP);

        if (p1HP > p2HP) return "PLAYER 1 WINS!";
        if (p2HP > p1HP) return "PLAYER 2 WINS!";
        return "IT'S A DRAW!";
    }

    IEnumerator CampaignWin()
    {
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        if (player1WinsImage != null) player1WinsImage.SetActive(false);
        
        CampaignManager.instance.PlayerWon();
    }

    IEnumerator CampaignLoss()
    {
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        if (player2WinsImage != null) player2WinsImage.SetActive(false);
        if (drawImage != null) drawImage.SetActive(false);
        
        CampaignManager. instance.PlayerLost();
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        if (player1WinsImage != null) player1WinsImage.SetActive(false);
        if (player2WinsImage != null) player2WinsImage.SetActive(false);
        if (drawImage != null) drawImage.SetActive(false);
        
        if (CharacterManager.instance != null)
            CharacterManager.instance.ResetData();
        
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
}