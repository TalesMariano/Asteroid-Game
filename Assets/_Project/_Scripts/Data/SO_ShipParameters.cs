using UnityEngine;

[CreateAssetMenu]
public class SO_ShipParameters : ScriptableObject
{
    public ShipParameters shipParameters;
}


[System.Serializable]
public class ShipParameters
{
    public float thrustSpeed = 1f;
    public float rotationSpeed = 1.5f;
    public float intangibilityDuration = 2f;
}