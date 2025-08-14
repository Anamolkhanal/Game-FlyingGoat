using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    PlayerControl control;
    float direction;
    float lastDirection = 0f;
    float externalDirection = 0f; // set by mobile input
    bool isGameOver = false;
    Vector3 startingPosition;
    public float moveSpeed = 5f;
    public float jumpStrength = 5f;
    public float gameOverHeight = -5f;
    public Rigidbody2D playerRB;

    private void Awake(){
        control = new PlayerControl();
        control.Enable();
        // Listen for Move action (1D Axis, float)
        control.Goat.Move.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
            // Play move SFX only when starting to move
            if (lastDirection == 0f && direction != 0f)
            {
                if (SFXManager.Instance != null) SFXManager.Instance.PlayMove();
            }
            lastDirection = direction;
        };
        control.Goat.Move.canceled += ctx =>
        {
            direction = 0f;
            lastDirection = 0f;
        };
        // Listen for Jump action (assume it exists)
        control.Goat.Jump.performed += ctx =>
        {
            if (!isGameOver && playerRB != null)
            {
                playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, jumpStrength);
                if (SFXManager.Instance != null) SFXManager.Instance.PlayJump();
            }
        };
    }

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (playerRB != null)
                playerRB.linearVelocity = Vector2.zero;
            return;
        }

        // Game over if player falls below threshold
        if (transform.position.y < gameOverHeight)
        {
            GameOver();
            return;
        }

        // Choose external direction when present, else input action direction
        float effectiveDirection = externalDirection != 0f ? externalDirection : direction;
        playerRB.linearVelocity = new Vector2(effectiveDirection * moveSpeed, playerRB.linearVelocity.y);
    }

    void GameOver()
    {
        isGameOver = true;
        if (SFXManager.Instance != null) SFXManager.Instance.PlayGameOver();
        Debug.Log("GAME OVER - Player fell down!");
        // Get the score from ObstacleSpawner
        ObstacleSpawner spawner = FindFirstObjectByType<ObstacleSpawner>();
        int finalScore = spawner != null ? spawner.GetScore() : 0;
        Debug.Log("Final Score: " + finalScore);
        Debug.Log("----------------------------------------");
        // Save the score for the GameOver scene
        PlayerPrefs.SetInt("FinalScore", finalScore);
        PlayerPrefs.Save();
        // Load the GameOver scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOver");
    }

    public void ResetPlayer()
    {
        transform.position = startingPosition;
        if (playerRB != null)
        {
            playerRB.linearVelocity = Vector2.zero;
            playerRB.angularVelocity = 0f;
        }
        // Reset other state as needed
    }

    // Mobile input hooks
    public void SetExternalDirection(float value)
    {
        externalDirection = Mathf.Clamp(value, -1f, 1f);
    }

    public void TriggerExternalJump()
    {
        if (!isGameOver && playerRB != null)
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, jumpStrength);
            if (SFXManager.Instance != null) SFXManager.Instance.PlayJump();
        }
    }
}
