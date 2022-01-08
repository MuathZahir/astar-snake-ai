using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Body : MonoBehaviour
{
    [SerializeField] FruitSpawner fruitSpawner;
    [SerializeField] bool head = false;

    [SerializeField] MenuManager menuManager;

    float width, height;

    private void Start()
    {
        transform.localScale = new Vector3(Snake.bodySize, Snake.bodySize, Snake.bodySize);

        height = Camera.main.orthographicSize * 2;
        width = height * Camera.main.aspect;

        fruitSpawner = GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(head)
        {
            Snake.add = true;
            fruitSpawner.spawned = false;

            Snake.score += 10;

            if(Snake.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", Snake.score);
            }

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(head)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                menuManager.RestartGame();
            }
        }
    }

    private void Update()
    {
        if (transform.position.y > height / 2 || transform.position.y < -height / 2)
        {
            menuManager.RestartGame();
        }
        if (transform.position.x > width / 2 || transform.position.x < -width / 2)
        {
            menuManager.RestartGame();
        }

    }


}
