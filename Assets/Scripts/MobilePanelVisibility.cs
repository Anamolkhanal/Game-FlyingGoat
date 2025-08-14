using UnityEngine;
using UnityEngine.SceneManagement;

public class MobilePanelVisibility : MonoBehaviour
{
    public string gameplaySceneName = "Gameplay"; // Set this to your gameplay scene's name

    void Start()
    {
        // Only show this panel in the gameplay scene
        if (SceneManager.GetActiveScene().name == gameplaySceneName)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
} 