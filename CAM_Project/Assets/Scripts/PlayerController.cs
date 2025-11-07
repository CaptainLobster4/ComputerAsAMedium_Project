using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int lives;
    private float playerSpeed;

    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        playerSpeed = 6.5f;
        gameManager.ChangeLivesText(lives);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    public void LoseALife()
    {
        //lives = lives - 1;
        //lives -= 1;
        lives--;
        gameManager.ChangeLivesText(lives);
        if (lives == 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

    void Movement()
    {
        // Read player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Move player
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * playerSpeed);

        // Get bounds from GameManager (synced with camera)
        float horizontalScreenSize = gameManager.horizontalScreenSize;
        float verticalScreenSize = gameManager.verticalScreenSize;

        // --- Horizontal wrap-around ---
        if (transform.position.x <= -horizontalScreenSize || transform.position.x > horizontalScreenSize)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        // --- Vertical clamp (relative to camera position) ---
        Camera cam = Camera.main;
        float camY = cam.transform.position.y;

        // bottom = bottom of camera view, top = halfway up the camera
        float bottomLimit = camY - verticalScreenSize +4.5f;
        float topLimit = camY; // halfway up camera view

        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomLimit, topLimit);
        transform.position = clampedPosition;


    }
}

