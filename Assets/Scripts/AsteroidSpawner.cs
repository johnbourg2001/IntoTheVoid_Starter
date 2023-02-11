using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float spawnRate = 2.0f;
    public int spawnAmount = 1;
    public float spawnDistance = 5.0f;
    public float trajectoryVariance = 15.0f;

    public Asteroid asteroidPrefab;
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnAsteroid), this.spawnRate, this.spawnRate);
    }

    private void SpawnAsteroid()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            
            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance); // Get a random angle to adjust the trajectory
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            
            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
