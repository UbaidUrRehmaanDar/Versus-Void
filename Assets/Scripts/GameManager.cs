using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System. Collections;

public class GameManager :  MonoBehaviour
{
    public static GameManager instance;
    
    [Header("End Game UI")]
    public GameObject endGamePanel;
    public GameObject player1WinsImage;
    public GameObject player2WinsImage;
    public GameObject drawImage;
    
    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public float endGameDelay = 5f;
    
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
    }

    public void EndMatch()
    {
        if (gameEnded) return; // Prevent multiple calls
        
        gameEnded = true;
        
        Debug.Log("🏁 MATCH ENDED!");
        
        ShowEndGame();
    }

    void ShowEndGame()
    {
        if (endGamePanel == null) return;

        endGamePanel.SetActive(true);

        // Determine who won
        string result = GetWinner();
        
        Debug.Log("🏆 Winner Result: '" + result + "'");

        if (result == "PLAYER 1 WINS!")
        {
            if (player1WinsImage != null) player1WinsImage.SetActive(true);
        }
        else if (result == "PLAYER 2 WINS!")
        {
            if (player2WinsImage != null) player2WinsImage.SetActive(true);
        }
        else // Draw
        {
            if (drawImage != null) drawImage.SetActive(true);
        }

        StartCoroutine(ReturnToMenu());
    }

    string GetWinner()
    {
        float p1HP = 0;
        float p2HP = 0;

        // Find all FighterHealth components
        FighterHealth[] allHealths = FindObjectsByType<FighterHealth>(FindObjectsSortMode.None);
        
        foreach (var h in allHealths)
        {
            PlayerController pc = h.GetComponent<PlayerController>();
            if (pc != null)
            {
                if (pc.playerID == 1) p1HP = h.currentHealth;
                else if (pc.playerID == 2) p2HP = h.currentHealth;
            }
        }

        Debug.Log("💚 P1 Health: " + p1HP + " | P2 Health: " + p2HP);

        if (p1HP > p2HP) return "PLAYER 1 WINS!";
        if (p2HP > p1HP) return "PLAYER 2 WINS!";
        return "IT'S A DRAW!";
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSecondsRealtime(endGameDelay);
        
        // Hide all images
        if (player1WinsImage != null) player1WinsImage.SetActive(false);
        if (player2WinsImage != null) player2WinsImage.SetActive(false);
        if (drawImage != null) drawImage.SetActive(false);
        
        if (CharacterManager. instance != null)
            CharacterManager.instance.ResetData();
        
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
}