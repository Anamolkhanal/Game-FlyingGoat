using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int numberOfObstacles = 8;
    public float minSpawnInterval = 1f;    // Minimum time between obstacles
    public float maxSpawnInterval = 10f;   // Maximum time between obstacles
    public float minY = -3f;               // Lower minimum height
    public float maxY = 3f;                // Higher maximum height
    public float spawnX = 10f;
    public float resetX = -10f;
    public float initialSpawnDelay = 0.5f;

    private List<GameObject> obstacles;
    private float nextSpawnTime;
    private SpriteRenderer prefabRenderer;
    private int activeObstacleCount = 0;
    private float lastY = 0f;              // Track last obstacle's Y position
    private int score = 0;

    public TextMeshProUGUI scoreTxt;            // Track the score
    private List<bool> obstaclePassed;     // Track which obstacles have been passed
    private GameObject goat;               // Reference to the goat object

    void Start()
    {
        // Check if obstacle prefab is assigned
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle Prefab is not assigned! Please assign an obstacle prefab in the Inspector!");
            return;
        }
        
        obstacles = new List<GameObject>();
        obstaclePassed = new List<bool>();
        prefabRenderer = obstaclePrefab.GetComponent<SpriteRenderer>();
        
        // Debug UI text reference
        if (scoreTxt == null)
        {
            Debug.LogError("Score Text is not assigned! Please assign the TextMeshPro UI element in the Inspector!");
        }
        else
        {
            Debug.Log("Score Text is assigned: " + scoreTxt.name);
            // Set initial score text
            scoreTxt.text = "0";
        }
        
        // Find the goat object automatically
        goat = GameObject.FindGameObjectWithTag("Player");
        if (goat == null)
        {
            Debug.LogError("Could not find goat object! Make sure your goat has the 'Player' tag!");
        }
        else
        {
            Debug.Log("Found goat object: " + goat.name);
        }
        
        // Create initial pool of obstacles
        for (int i = 0; i < numberOfObstacles; i++)
        {
            CreateNewObstacle();
            obstaclePassed.Add(false);
        }

        // Stagger the initial spawns
        for (int i = 0; i < numberOfObstacles; i++)
        {
            float delay = i * initialSpawnDelay;
            Invoke("SpawnInitialObstacle", delay);
        }

        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnInitialObstacle()
    {
        if (activeObstacleCount < numberOfObstacles)
        {
            float y = GetRandomY();
            Vector3 spawnPosition = new Vector3(spawnX, y, 0);
            GameObject obstacle = obstacles[activeObstacleCount];
            obstacle.transform.position = spawnPosition;
            activeObstacleCount++;
        }
    }

    float GetRandomY()
    {
        float y;
        do
        {
            y = Random.Range(minY, maxY);
        } while (Mathf.Abs(y - lastY) < 1f); // Ensure minimum distance between obstacles

        lastY = y;
        return y;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }

        // Check each obstacle's position and score
        for (int i = 0; i < activeObstacleCount; i++)
        {
            GameObject obstacle = obstacles[i];
            if (obstacle == null) continue; // Skip destroyed obstacles

            // Check if goat has passed the obstacle
            if (!obstaclePassed[i] && goat != null && goat.transform.position.x > obstacle.transform.position.x)
            {
                score++;
                obstaclePassed[i] = true;
                scoreTxt.text = score.ToString("0");
                Debug.Log("----------------------------------------");
                Debug.Log("GOAT PASSED OBSTACLE!");
                Debug.Log("SCORE: " + score);
                Debug.Log("----------------------------------------");
            }

            if (obstacle.transform.position.x < resetX)
            {
                ResetObstacle(obstacle);
                obstaclePassed[i] = false;  // Reset the passed status when obstacle is recycled
            }
        }
    }

    void CreateNewObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Cannot create obstacle - obstaclePrefab is null!");
            return;
        }
        
        GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(spawnX, 0, 0), Quaternion.identity);
        
        if (obstacle == null)
        {
            Debug.LogError("Failed to instantiate obstacle!");
            return;
        }
        
        // Set the sorting order
        SpriteRenderer renderer = obstacle.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 1;
        }
        
        // Ensure correct scale
        obstacle.transform.localScale = obstaclePrefab.transform.localScale;
        obstacles.Add(obstacle);
    }

    void ResetObstacle(GameObject obstacle)
    {
        float y = GetRandomY();
        obstacle.transform.position = new Vector3(spawnX, y, 0);
        obstacle.transform.localScale = obstaclePrefab.transform.localScale;
        
        // Ensure sorting order is maintained
        SpriteRenderer renderer = obstacle.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 1;
        }
    }

    void SpawnObstacle()
    {
        // Find the first obstacle that's off screen
        for (int i = 0; i < activeObstacleCount; i++)
        {
            GameObject obstacle = obstacles[i];
            if (obstacle.transform.position.x < resetX)
            {
                ResetObstacle(obstacle);
                break;
            }
        }
    }

    // Add this new method to get the current score
    public int GetScore()
    {
        return score;
    }

    // Add this method to reset the score
    public void ResetScore()
    {
        score = 0;
        if (scoreTxt != null)
        {
            scoreTxt.text = "0";
        }
        Debug.Log("ObstacleSpawner score reset to 0");
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
                Destroy(obstacle);
        }
        obstacles.Clear();
        obstaclePassed.Clear();
        activeObstacleCount = 0;
    }

    public void InitializeObstacles()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            CreateNewObstacle();
            obstaclePassed.Add(false);
        }

        // Stagger the initial spawns
        for (int i = 0; i < numberOfObstacles; i++)
        {
            float delay = i * initialSpawnDelay;
            Invoke("SpawnInitialObstacle", delay);
        }

        activeObstacleCount = 0;
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }
} 