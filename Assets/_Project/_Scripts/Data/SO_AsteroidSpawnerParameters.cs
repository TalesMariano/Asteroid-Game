using UnityEngine;

[CreateAssetMenu]
public class SO_AsteroidSpawnerParameters : ScriptableObject
{
    public AsteroidSpawnerParameters asteroidSpawnerParameters;
}

[System.Serializable]
public class AsteroidSpawnerParameters
{
    public float spawnDistance = 12f;
    public int amountPerSpawn = 5;
}