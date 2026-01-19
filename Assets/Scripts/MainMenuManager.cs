using UnityEngine;
using UnityEngine.SceneManagement; // Required to change scenes

public class MainMenuManager : MonoBehaviour
{
    // This function will run when "Demo Play" is clicked

public void StartFullGame()
    {
        // This opens the Character Selection Screen
        SceneManager.LoadScene("1v1CharacterSelect"); 
    }

    public void StartDemo()
    {
        // Replace "DemoFight" with the exact name of your combat scene
        SceneManager.LoadScene("AI_CharacterSelect");
    }

    // This function will run when "Exit" is clicked
    public void ExitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }
}