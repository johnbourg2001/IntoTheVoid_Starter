using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    public int score = 0;
    public float difficultyLevelTime = 30.0f; // Amount of seconds between each level of difficulty

    public int difficulty = 0;
    public AsteroidSpawner asteroidSpawner;
    
    public TextMeshProUGUI  scoreText;
    
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
            if (difficulty != 0)
            {
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
        if (asteroid.size >= asteroid.largeCutoff)
        {
           score += 25;
        }
        else if (asteroid.size >= asteroid.mediumCutoff)
        {
            score += 50;
        }
        else if (asteroid.size >= asteroid.smallCutoff)
        {
            score += 100;
        }
    }
}
