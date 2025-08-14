using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isGamePaused = false;
    PlayerControl control;

    void Awake()
    {
        control = new PlayerControl();
        control.Enable();
        control.Goat.Pause.performed += ctx => TogglePauseMenu(); // Use the correct map/action name
    }

    void Start()
    {
        // Hide pause menu initially
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void OnDestroy()
    {
        control.Disable();
    }

    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(true);
            Debug.Log("Pause menu shown");
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            Debug.Log("Pause menu hidden");
        }
    }

    public void OnResumeButton()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        Debug.Log("Pause menu hidden (Resume)");
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
} 