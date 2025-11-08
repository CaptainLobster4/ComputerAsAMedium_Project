using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject enemyTwoPrefab;
    public GameObject cloudPrefab;

    public TextMeshProUGUI livesText;

    public float horizontalScreenSize;
    public float verticalScreenSize;

    public int score;

    // Adjustable spawn timing for each enemy type
    public float enemyOneSpawnRate = 3f;
    public float enemyTwoSpawnRate = 5f;
    public float enemyTwoStartDelay = 2f;

    void Start()
    {
        Camera cam = Camera.main;

        // Automatically set world bounds based on camera’s view
        verticalScreenSize = cam.orthographicSize;
        horizontalScreenSize = cam.orthographicSize * cam.aspect;

        score = 0;
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();

        // Spawn EnemyOne starting immediately, every few seconds
        InvokeRepeating(nameof(SpawnEnemyOne), 1f, enemyOneSpawnRate);

        // Spawn EnemyTwo starting after a delay, at its own rate
        InvokeRepeating(nameof(SpawnEnemyTwo), enemyTwoStartDelay, enemyTwoSpawnRate);
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
        Instantiate(
            enemyTwoPrefab,
            new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.7f, verticalScreenSize, 0),
            Quaternion.Euler(180, 0, 0)
        );
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

    public void AddScore(int earnedScore)
    {
        score += earnedScore;
    }

    public void ChangeLivesText(int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }
}
