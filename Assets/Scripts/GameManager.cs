using UnityEngine;
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
        
        Debug.Log("==========================================");
        Debug.Log(" GameManager Started");
        Debug.Log("Campaign Mode: " + isCampaignMode);
        Debug.Log("CampaignManager exists: " + (CampaignManager. instance != null));
        if (CampaignManager.instance != null)
        {
            Debug. Log("Current Campaign Stage: " + CampaignManager.instance.currentStage);
        }
        Debug.Log("==========================================");
    }

    public void EndMatch()
    {
        if (gameEnded)
        {
            Debug.Log(" EndMatch already called, ignoring");
            return;
        }
        
        gameEnded = true;
        
        Debug.Log("==========================================");
        Debug.Log(" MATCH ENDED!");
        Debug.Log("==========================================");
        
        ShowEndGame();
    }

    void ShowEndGame()
    {
        Debug.Log(" ShowEndGame() called");
        Debug.Log("   isCampaignMode = " + isCampaignMode);
        Debug.Log("   CampaignManager exists = " + (CampaignManager.instance != null));
        
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            Debug.Log(" End game panel shown");
        }
        else
        {
            Debug.LogError(" endGamePanel is NULL!");
        }

        string result = GetWinner();
        
        Debug.Log(" Winner:  " + result);

        if (result == "PLAYER 1 WINS!")
        {
            if (player1WinsImage != null)
            {
                player1WinsImage. SetActive(true);
            }
            
            if (isCampaignMode && CampaignManager.instance != null)
            {
                Debug.Log(" Campaign mode & player won - calling CampaignWin()");
                StartCoroutine(CampaignWin());
                return;
            }
            else
            {
                Debug.Log(" NOT campaign mode or no CampaignManager");
            }
        }
        else if (result == "PLAYER 2 WINS!")
        {
            if (player2WinsImage != null)
            {
                player2WinsImage.SetActive(true);
            }
            
            if (isCampaignMode && CampaignManager. instance != null)
            {
                Debug.Log(" Campaign mode & player lost - calling CampaignLoss()");
                StartCoroutine(CampaignLoss());
                return;
            }
        }
        else
        {
            if (drawImage != null)
            {
                drawImage. SetActive(true);
            }
            
            if (isCampaignMode && CampaignManager.instance != null)
            {
                Debug.Log(" Campaign mode & draw - calling CampaignLoss()");
                StartCoroutine(CampaignLoss());
                return;
            }
        }

        Debug.Log(" Going to ReturnToMenu()");
        StartCoroutine(ReturnToMenu());
    }

    string GetWinner()
    {
        float p1HP = -1;
        float p2HP = -1;

        FighterHealth[] allHealths = FindObjectsByType<FighterHealth>(FindObjectsSortMode.None);
        
        Debug.Log(" Found " + allHealths.Length + " FighterHealth components");
        
        foreach (var h in allHealths)
        {
            string objName = h.gameObject.name;
            float hp = h. currentHealth;
            
            PlayerController pc = h.GetComponent<PlayerController>();
            AIController ai = h.GetComponent<AIController>();
            
            if (pc != null)
            {
                if (pc.playerID == 1)
                {
                    p1HP = hp;
                    Debug.Log("   Player 1 (PlayerController): HP = " + p1HP);
                }
                else if (pc.playerID == 2)
                {
                    p2HP = hp;
                    Debug. Log("   Player 2 (PlayerController): HP = " + p2HP);
                }
            }
            else if (ai != null)
            {
                p2HP = hp;
                Debug. Log("   Player 2 (AIController): HP = " + p2HP);
            }
            else if (objName.Contains("Player1"))
            {
                p1HP = hp;
                Debug. Log("   Player 1 (by name): HP = " + p1HP);
            }
            else if (objName.Contains("Player2"))
            {
                p2HP = hp;
                Debug.Log("   Player 2 (by name): HP = " + p2HP);
            }
        }

        Debug.Log(" FINAL - P1 HP:  " + p1HP + " | P2 HP: " + p2HP);

        if (p1HP > p2HP) return "PLAYER 1 WINS!";
        if (p2HP > p1HP) return "PLAYER 2 WINS!";
        return "IT'S A DRAW!";
    }

    IEnumerator CampaignWin()
    {
        Debug.Log(" Waiting " + endGameDelay + " seconds before next stage.. .");
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        if (player1WinsImage != null) player1WinsImage.SetActive(false);
        if (endGamePanel != null) endGamePanel.SetActive(false);
        
        Debug.Log(" Calling CampaignManager.PlayerWon()");
        CampaignManager.instance. PlayerWon();
    }

    IEnumerator CampaignLoss()
    {
        Debug.Log(" Waiting " + endGameDelay + " seconds before main menu...");
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        if (player2WinsImage != null) player2WinsImage.SetActive(false);
        if (drawImage != null) drawImage.SetActive(false);
        if (endGamePanel != null) endGamePanel.SetActive(false);
        
        Debug.Log(" Calling CampaignManager.PlayerLost()");
        CampaignManager. instance.PlayerLost();
    }

    IEnumerator ReturnToMenu()
    {
        Debug.Log(" Waiting " + endGameDelay + " seconds before menu...");
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        if (player1WinsImage != null) player1WinsImage.SetActive(false);
        if (player2WinsImage != null) player2WinsImage.SetActive(false);
        if (drawImage != null) drawImage.SetActive(false);
        if (endGamePanel != null) endGamePanel.SetActive(false);
        
        if (CharacterManager.instance != null)
            CharacterManager.instance.ResetData();
        
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
}