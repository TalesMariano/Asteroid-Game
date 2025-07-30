using UnityEngine;

[CreateAssetMenu]
public class SO_AsteroidDetectionParameters : ScriptableObject
{
    public AsteroidDetectionParameters asteroidDetectionParameters;
}

[System.Serializable]
public class AsteroidDetectionParameters
{
    public int rayAmount = 8;
    public float rayLength = 5f;
    public float detectionAngle = 45;
}

