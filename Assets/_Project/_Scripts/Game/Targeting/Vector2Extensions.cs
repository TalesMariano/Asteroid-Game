using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 RotateVector2(this Vector3 source, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad; // Convert to radians
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float x = source.x * cos - source.y * sin;
        float y = source.x * sin + source.y * cos;

        return new Vector2(x, y);
    }
}
