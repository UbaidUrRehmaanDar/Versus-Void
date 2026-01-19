using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimerUI : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    private bool timerIsRunning = false;
    private TextMeshProUGUI timerText;

    [Header("End Game UI")]
    public GameObject endGamePanel; 
    
    [Header("Result Images - Assign the 3 Image GameObjects")]
    public GameObject player1WinsImage; // Drag the P1 Wins Image here
    public GameObject player2WinsImage; // Drag the P2 Wins Image here
    public GameObject drawImage;        // Drag the Draw Image here

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerIsRunning = true;
        Time.timeScale = 1f; 
        
        if(endGamePanel != null) endGamePanel.SetActive(false);
        
        // Hide all result images at start
        if(player1WinsImage != null) player1WinsImage.SetActive(false);
        if(player2WinsImage != null) player2WinsImage.SetActive(false);
        if(drawImage != null) drawImage.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = Mathf. CeilToInt(timeRemaining).ToString();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                timerText.text = "0";
                ShowEndGame();
            }
        }
    }

    void ShowEndGame()
    {
        if (endGamePanel == null) return;

        endGamePanel. SetActive(true);

        // Determine who won and show the correct image
        if (CharacterManager. instance != null)
        {
            string result = CharacterManager. instance.GetWinnerMessage();
            
            Debug.Log("Winner Result: '" + result + "'");

            if (result == "PLAYER 1 WINS!")
            {
                if(player1WinsImage != null) player1WinsImage.SetActive(true);
            }
            else if (result == "PLAYER 2 WINS!")
            {
                if(player2WinsImage != null) player2WinsImage.SetActive(true);
            }
            else // Draw
            {
                if(drawImage != null) drawImage.SetActive(true);
            }
        }

        StartCoroutine(WaitAndReturnToMenu());
    }

    IEnumerator WaitAndReturnToMenu()
    {
        yield return new WaitForSecondsRealtime(5f);
        
        // Hide all images before leaving
        if(player1WinsImage != null) player1WinsImage.SetActive(false);
        if(player2WinsImage != null) player2WinsImage.SetActive(false);
        if(drawImage != null) drawImage.SetActive(false);
        
        if (CharacterManager.instance != null) CharacterManager.instance.ResetData();
        SceneManager. LoadScene(mainMenuSceneName);
    }
}