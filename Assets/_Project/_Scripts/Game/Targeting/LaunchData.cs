using UnityEngine;

public struct LaunchData
{
    public Vector3 initialPosition;
    public Vector3 targetPosition;
    public Vector3 initialVelocity;

    public float horizontalDistance;
    public float timeToTarget;
    public float gravity;
}

public struct TargetData
{
    public Transform targetObject;
    public Vector3 targetPosition;

    public TargetData(Vector3 mouseClickPosition, Transform target)
    {
        targetObject = target;
        targetPosition = mouseClickPosition;
    }
}