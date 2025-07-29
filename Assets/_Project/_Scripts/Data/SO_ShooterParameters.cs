using UnityEngine;

[CreateAssetMenu]
public class SO_ShooterParameters : ScriptableObject
{
    public ShooterParameters shooterParameters;
}

[System.Serializable]
public class ShooterParameters
{
    public float bulletSpeed = 20;
    public float bulletLifeTime = 1;
    public float bulletCooldown = 1;
    public float maxAmountBullets = 3;
}