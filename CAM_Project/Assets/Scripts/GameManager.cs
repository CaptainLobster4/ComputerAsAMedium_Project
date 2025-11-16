using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject enemyTwoPrefab;
    public GameObject cloudPrefab;
    public GameObject gameOverText;
    public GameObject restartText;
    public GameObject powerupPrefab;
    public GameObject coinPrefab;
    public GameObject audioPlayer;

    public AudioClip powerupSound;
    public AudioClip powerdownSound;


    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerupText;

    public float horizontalScreenSize;
    public float verticalScreenSize;

    // Adjustable spawn timing for each enemy type
    public float enemyOneSpawnRate = 3f;
    public float enemyTwoSpawnRate = 5f;
    public float enemyTwoStartDelay = 2f;

    public int score;
    public int cloudMove;

    private bool gameOver;

    void Start()
    {
        Camera cam = Camera.main;

        // Automatically set world bounds based on camera’s view
        verticalScreenSize = cam.orthographicSize;
        horizontalScreenSize = cam.orthographicSize * cam.aspect;

        score = 0;
        cloudMove = 1;
        gameOver = false;
        AddScore(0);
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();
        StartCoroutine(SpawnPowerup());
        powerupText.text = "No powerups yet!";

        StartCoroutine(SpawnCoin());

        // Spawn EnemyOne starting immediately, every few seconds
        InvokeRepeating(nameof(SpawnEnemyOne), 1f, enemyOneSpawnRate);

        // Spawn EnemyTwo starting after a delay, at its own rate
        InvokeRepeating(nameof(SpawnEnemyTwo), enemyTwoStartDelay, enemyTwoSpawnRate);
    }
    void Update()
    {
        // Restart logic
        if (gameOver && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SpawnEnemyOne()
    {
        Instantiate(
            enemyOnePrefab,
            new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.7f, verticalScreenSize, 0),
            Quaternion.Euler(180, 0, 0)
        );
    }

    void SpawnEnemyTwo()
    {
        float halfHeight = coinPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        float minY = -verticalScreenSize + halfHeight; // bottom of screen
        float maxY = 0f - halfHeight;                   // halfway point minus coin size
        Instantiate(
            enemyTwoPrefab,
            new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.7f, verticalScreenSize, 0),
            Quaternion.Euler(180, 0, 0)
        );
    }
    void CreateCoin()
    {
        float halfHeight = coinPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        float minY = -verticalScreenSize + halfHeight; // bottom
        float maxY = 0f - halfHeight;                   // halfway

        float spawnY = Random.Range(minY, maxY);

        Instantiate(
            coinPrefab,
            new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), spawnY, 0),
            Quaternion.identity
        );

    }

    IEnumerator SpawnCoin()
    {
        while (!gameOver)
        {
            float spawnTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(spawnTime);
            CreateCoin();
        }
    }

    // --- powerup spawn ---
    void CreatePowerup()
    {
        float halfHeight = powerupPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        float minY = -verticalScreenSize + halfHeight;
        float maxY = 0f - halfHeight;
        float spawnY = Random.Range(minY, maxY);

        Instantiate(
            powerupPrefab,
            new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), spawnY, 0),
            Quaternion.identity
        );

    }

    IEnumerator SpawnPowerup()
    {
        float spawnTime = Random.Range(3, 5);
        yield return new WaitForSeconds(spawnTime);
        CreatePowerup();
        StartCoroutine(SpawnPowerup());
    }
    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(
                cloudPrefab,
                new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0),
                Quaternion.identity
            );
        }
    }

    // --- UI & Score ---
    public void AddScore(int earnedScore)
    {
        score += earnedScore;

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ChangeLivesText(int currentLives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }
    // --- Powerup UI text ---
    public void ManagePowerupText(int powerupType)
    {
        if (powerupText == null) return;

        switch (powerupType)
        {
            case 1: powerupText.text = "Speed!"; break;
            case 2: powerupText.text = "Double Weapon!"; break;
            case 3: powerupText.text = "Triple Weapon!"; break;
            case 4: powerupText.text = "Shield!"; break;
            default: powerupText.text = "No powerups yet!"; break;
        }
    }
    // --- Audio ---
    public void PlaySound(int whichSound)
    {
        if (audioPlayer == null) return;

        var source = audioPlayer.GetComponent<AudioSource>();

        switch (whichSound)
        {
            case 1: source.PlayOneShot(powerupSound); break;
            case 2: source.PlayOneShot(powerdownSound); break;
        }
    }
    // --- Partner’s Game Over logic ---
    public void GameOver()
    {
        if (gameOverText != null) gameOverText.SetActive(true);
        if (restartText != null) restartText.SetActive(true);

        gameOver = true;
        CancelInvoke();
        cloudMove = 0;
    }
}
