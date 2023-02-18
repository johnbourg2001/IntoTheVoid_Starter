using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    public Player player;

    public int lives = 3;

    public float RespawnTime = 3.0f;

    public int score = 0;
    public float difficultyLevelTime = 30.0f; // Amount of seconds between each level of difficulty

    public int difficulty = 0;
    public AsteroidSpawner asteroidSpawner;
    
    public TextMeshProUGUI  scoreText;

    public static GameManager Instance; // A static reference to the GameManager instance

    void Awake()
    {
        if(Instance == null) { // If there is no instance already
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        } else if(Instance != this) { // If there is already an instance and it's not `this` instance
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DifficultyScaler());
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
        string timeText = string.Format("{0:D2}:{1:D2}<br>", timeSpan.Minutes, timeSpan.Seconds);
        scoreText.text = timeText + "Score:<br>" + score;
    }

    

    IEnumerator DifficultyScaler()
    {
        // Could look into Time.time for total time game has been running for
        
        // 3 difficulty levels
        // 0 - Start (fake level for first pass of routine)
        // 1 - Spawns 2x asteroids every 4 seconds
        // 2 - Spawns 4x asteroids every 3 seconds
        // 3 - Spawns 6x asteroids every 2 seconds
        // Difficulty transitions every 30 seconds
        while(difficulty != 3)
        {
            if (difficulty != 0) {
                asteroidSpawner.spawnAmount += 2;
                asteroidSpawner.spawnRate--; 
            }
            difficulty++;
            // Wait difficulty level time until this coroutien gets called again
            yield return new WaitForSeconds(this.difficultyLevelTime);
        }
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        if (asteroid.size >= asteroid.largeCutoff) {
           score += 25;
        } else if (asteroid.size >= asteroid.mediumCutoff) {
            score += 50;
        } else if (asteroid.size >= asteroid.smallCutoff) {
            score += 100;
        }
    }

    public void PlayerDied()
    {
        this.lives--;

        if (lives <= 0) {
            GameOver();   
        } else {
            Invoke(nameof(Respawn), this.RespawnTime);
        }

    }

    public float RespawnImmunity = 3.0f;
    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        
        // Do any respawn cleanup of the Player class
        this.player.Respawn();

        // Turn on collision after respawn immunity
        this.player.Invoke(nameof(player.TurnOnCollition), RespawnImmunity);
    }

    public void GameOver()
    {
        //TODO
        Time.timeScale = 0.0f;
    }
}
