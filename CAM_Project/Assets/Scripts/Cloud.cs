using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;

    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // look at heirarchy, find GameManager object, get component GameManager script
        transform.localScale = transform.localScale * Random.Range(0.1f, .6f); // scale cloud randomly between .1 and .6
        transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Random.Range(0.1f, 0.7f)); // set alpha randomly between .1 and .7 (transparency)
        speed = Random.Range(3f, 7f); // set speed randomly between 3 and 7

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime); // move cloud down at speed
        if (transform.position.y < -gameManager.verticalScreenSize) // if cloud goes off screen
        {
            transform.position = new Vector3(Random.Range(-gameManager.horizontalScreenSize, gameManager.horizontalScreenSize), gameManager.verticalScreenSize, 0); // reset position to top of screen at random x position
        }
    }
}
