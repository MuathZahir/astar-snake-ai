using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Text score;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        score.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        Screen.SetResolution(800, 800, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            RestartGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
