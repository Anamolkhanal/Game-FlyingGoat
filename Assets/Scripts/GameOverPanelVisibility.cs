using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelVisibility : MonoBehaviour
{
    void Start()
    {
        // Only enable this panel if the active scene is GameOver
        if (SceneManager.GetActiveScene().name != "GameOver")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
} 