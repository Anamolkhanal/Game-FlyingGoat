using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreTxt;
    public GameObject gameOverPanel;
    public GameObject TryAgainButton;


    void Start()
    {
        Debug.Log("GameOverUI script started");
        // Time.timeScale = 1f; // Ensure UI is responsive in GameOver scene
        // Display the final score if available
        if (finalScoreTxt != null)
        {
            int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
            finalScoreTxt.text = finalScore.ToString("0");
        }
    }

    public void OnTryAgainButton()
    {
        Debug.Log("Try Again button pressed");
          Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void Update()
{
    Debug.Log("TryAgainButton active: " + TryAgainButton.activeInHierarchy);
}
} 