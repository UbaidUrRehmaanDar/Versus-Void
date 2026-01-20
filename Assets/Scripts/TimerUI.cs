using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    private bool timerIsRunning = false;
    private TextMeshProUGUI timerText;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerIsRunning = true;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Stop timer if game already ended
        if (GameManager. instance != null && GameManager.instance.IsGameEnded())
        {
            timerIsRunning = false;
            return;
        }
        
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
                timerText. text = "0";
                
                // Time's up! End match
                if (GameManager.instance != null)
                {
                    Debug.Log("⏰ TIME'S UP!");
                    GameManager.instance. EndMatch();
                }
            }
        }
    }
}