using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public AsteroidSpawnerParamentes asteroidSpawnerParamentes;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        for (int i = 0; i < asteroidSpawnerParamentes.amountPerSpawn; i++)
        {
            // Choose a random direction from the center of the spawner and
            // spawn the asteroid a distance away
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * asteroidSpawnerParamentes.spawnDistance);

            // Create the new asteroid by cloning the prefab and set a random
            // size within the range
            Asteroid asteroid = Instantiate(asteroidSpawnerParamentes.asteroidPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
