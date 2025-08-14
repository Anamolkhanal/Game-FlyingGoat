using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    void Start()
    {
        // Show main menu at start
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        // Hide credits panel initially
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
        // Ensure game is paused in menu
        Time.timeScale = 0f;
    }

    public void OnPlayButton()
    {
        // Start the game (load gameplay scene)
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }

    public void OnCreditsButton()
    {
        // Hide main menu
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        // Show credits panel
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
        Debug.Log("Credits shown!");
    }

    public void OnHideCreditsButton()
    {
        // Hide credits panel
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
        // Show main menu again
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        Debug.Log("Credits hidden!");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
} 