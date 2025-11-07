using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glider : MonoBehaviour
{

    public bool goingUp;
    public float speed;

    // Horizontal movement settings (optional for certain enemies)
    public bool horizontalSlide;   // enable this for EnemyTwo
    public float slideAmplitude = 4f;  // how far left/right it moves
    public float slideFrequency = 2f;    // how fast it oscillates

    private float startX;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startX = transform.position.x;  // store original horizontal position
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        } else if (goingUp == false)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        // --- Horizontal Slide (for EnemyTwo only) ---
        if (horizontalSlide)
        {
            float xOffset = Mathf.Sin(Time.time * slideFrequency) * slideAmplitude;
            transform.position = new Vector3(startX + xOffset, transform.position.y, transform.position.z);
        }

        if (transform.position.y >= gameManager.verticalScreenSize * 1.25f || transform.position.y <= -gameManager.verticalScreenSize * 1.25f)
        {
            Destroy(this.gameObject);
        }
    }
}
