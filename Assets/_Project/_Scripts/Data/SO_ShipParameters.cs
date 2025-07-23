using System.Collections;
using System.Collections.Generic;
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
    public float shipMaxSpeed = 5f;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
}