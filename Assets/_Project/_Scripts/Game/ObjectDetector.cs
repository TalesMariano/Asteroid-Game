using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour, ITracker<Asteroid>
{
    [SerializeField] private Transform startPoint;

    public float distance = 5f;

    public int rayAmmount = 8;
    public float detectionAngle = 45;

    [SerializeField] private List<Asteroid> asteroids;

    public List<Asteroid> listItems
    {
        get => asteroids; 
    }

    void Update()
    {

        bool hit = false;

        float dividedAngle = detectionAngle / rayAmmount;

        for (int i = 0; i < rayAmmount; i++)
        {
            float angleVariance = dividedAngle * i - detectionAngle / 2;

            Vector2 direction = RotateVector2(transform.up, angleVariance);

            RaycastHit2D ray = Physics2D.Raycast(startPoint.position, direction, distance, LayerMask.GetMask("Asteroid"));

            float hitDistance = distance;

            

            if (ray)
            {
                hitDistance = ray.distance;

                Asteroid hitAsteroid = ray.transform.gameObject.GetComponent<Asteroid>();

                if (!asteroids.Contains(hitAsteroid))
                {
                    asteroids.Add(hitAsteroid);
                }

            }

            hit = hit || ray;

            Debug.DrawRay(startPoint.position, direction * hitDistance, Color.blue);


        }


        if (hit)
        {
            // Debug.LogError("haycast hit");
        }
        else if(asteroids.Count != 0)
        {
            asteroids.Clear();
        }
    }

    public static Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad; // Convert to radians
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float x = v.x * cos - v.y * sin;
        float y = v.x * sin + v.y * cos;

        return new Vector2(x, y);
    }
}
