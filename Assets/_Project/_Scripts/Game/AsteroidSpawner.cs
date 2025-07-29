using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid _asteroidPrefab;

    [SerializeField] private SO_AsteroidSpawnerParameters soAsteroidSpawnerParameters;
    [SerializeField, Tooltip("Used when SO is null")] private AsteroidSpawnerParameters _debugAsteroidSpawnerParameters;

    private AsteroidSpawnerParameters Parameters
    {
        get { return soAsteroidSpawnerParameters ? soAsteroidSpawnerParameters.asteroidSpawnerParameters : _debugAsteroidSpawnerParameters; }
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        for (int i = 0; i < Parameters.amountPerSpawn; i++)
        {
            // Choose a random direction from the center of the spawner and
            // spawn the asteroid a distance away
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * Parameters.spawnDistance);

            // Create the new asteroid by cloning the prefab and set a random
            // size within the range
            Asteroid asteroid = Instantiate(_asteroidPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
