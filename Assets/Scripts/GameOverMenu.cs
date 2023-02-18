using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    
    public GameObject gameOverMenu;
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayGameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }


    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
