using UnityEngine;

[CreateAssetMenu]
public class SO_AsteroidSpawnerParamentes : ScriptableObject
{
    public AsteroidSpawnerParamentes asteroidSpawnerParamentes;
}


[System.Serializable]
public class AsteroidSpawnerParamentes
{
    public Asteroid asteroidPrefab;
    public float spawnDistance = 12f;
    public int amountPerSpawn = 10;
}