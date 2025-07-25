using UnityEngine;

[CreateAssetMenu]
public class SO_AsteroidParameters : ScriptableObject
{
    public AsteroidParameters asteroidParameters;
}

[System.Serializable]
public class AsteroidParameters
{
    public int largeScore = 200;
    public int mediumScore = 100;
    public int smallScore = 50;
    [Space]
    public float SplitAngle = 30f;
    public int splitAmount = 2;
}

enum AsteroidSize
{
    Large,
    Medium,
    Small
}