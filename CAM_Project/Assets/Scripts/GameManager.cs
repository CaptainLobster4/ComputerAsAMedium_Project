using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float horizontalScreenSize = 5f;
    public float verticalScreenSize = 6f;

    public int score;
    public GameObject enemyOnePrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateEnemyOne", 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void CreateEnemyOne()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-9f, 9f), 6.5f, 0), Quaternion.identity);
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        Debug.Log("Score: " + score);
    }
}
