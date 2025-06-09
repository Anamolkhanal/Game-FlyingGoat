using UnityEngine;

public class GoatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Speed for left/right movement
    public float jumpStrength = 5f;     // Strength of the jump
    public float gameOverHeight = -5f;  // Height at which game over occurs
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            // Stop all movement when game is over
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        // Check if goat has fallen
        if (transform.position.y < gameOverHeight)
        {
            GameOver();
            return;
        }

        // Get input from left/right arrow keys
        horizontalInput = Input.GetAxis("Horizontal");  // Left/Right arrows

        // Jump with space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength);
            }
        }
    }

    void FixedUpdate()
    {
        if (isGameOver || rb == null) return;

        // Apply only horizontal movement
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("----------------------------------------");
        Debug.Log("GAME OVER - Goat fell down!");
        
        // Get the score from ObstacleSpawner
        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        int finalScore = spawner != null ? spawner.GetScore() : 0;
        Debug.Log("Final Score: " + finalScore);
        
        Debug.Log("----------------------------------------");
        
        // Stop time
        Time.timeScale = 0f;
    }
}
