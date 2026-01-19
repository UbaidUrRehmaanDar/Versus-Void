using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for the Image component
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
    public Image resultImageDisplay; // Drag the 'ResultImage' object here

    [Header("Result Sprites")]
    public Sprite p1WonSprite; // Drag your "1p won" sprite here
    public Sprite p2WonSprite; // Drag your "2p won" sprite here
    public Sprite drawSprite;  // Drag your "draw" sprite here

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

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
        if (endGamePanel == null || resultImageDisplay == null) return;

        endGamePanel.SetActive(true);

        // Determine who won and show the correct sprite
        if (CharacterManager.instance != null)
        {
            string result = CharacterManager.instance.GetWinnerMessage();

            if (result == "PLAYER 1 WINS!") {
                resultImageDisplay.sprite = p1WonSprite;
            } else if (result == "PLAYER 2 WINS!") {
                resultImageDisplay.sprite = p2WonSprite;
            } else {
                resultImageDisplay.sprite = drawSprite;
            }
            
            // This makes the image fit its original sprite proportions
            resultImageDisplay.SetNativeSize(); 
        }

        StartCoroutine(WaitAndReturnToMenu());
    }

    IEnumerator WaitAndReturnToMenu()
    {
        yield return new WaitForSecondsRealtime(5f);
        if (CharacterManager.instance != null) CharacterManager.instance.ResetData();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}