using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Needed to load scenes
using System.Collections; // Needed for Coroutines

public class TimerUI : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    private bool timerIsRunning = false;
    private TextMeshProUGUI timerText;

    [Header("End Game UI")]
    public GameObject endGamePanel; 
    public TextMeshProUGUI winnerText; 

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; // Make sure this matches your scene name!

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerIsRunning = true;
        Time.timeScale = 1f; 
        if(endGamePanel != null) endGamePanel.SetActive(false); 
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
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

        endGamePanel.SetActive(true);

        if (CharacterManager.instance != null)
        {
            // This gets "PLAYER 1 WINS!", "PLAYER 2 WINS!", or "IT'S A DRAW!"
            string result = CharacterManager.instance.GetWinnerMessage();
            winnerText.text = "TIME UP!\n" + result;
        }

        // Start the 5-second countdown to exit
        StartCoroutine(WaitAndReturnToMenu());
    }

    IEnumerator WaitAndReturnToMenu()
    {
        // We use 'WaitForSecondsRealtime' because Time.timeScale is usually 0 at game end
        yield return new WaitForSecondsRealtime(5f);
        
        // Reset the CharacterManager data so the next game is fresh
        if (CharacterManager.instance != null) CharacterManager.instance.ResetData();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}