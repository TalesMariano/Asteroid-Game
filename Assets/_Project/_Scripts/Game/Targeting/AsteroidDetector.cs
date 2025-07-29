using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidDetector : MonoBehaviour, ITracker<Asteroid>
{

    [SerializeField] private SO_AsteroidDetectionParameters soAsteroidDetectionParameters;
    [SerializeField, Tooltip("Used when SO is null")] private AsteroidDetectionParameters _debugAsteroidDetectionParameters;

    private AsteroidDetectionParameters Parameters
    {
        get { return soAsteroidDetectionParameters ? soAsteroidDetectionParameters.asteroidDetectionParameters : _debugAsteroidDetectionParameters; }
    }

    [Space]

    private LayerMask _layerMask;

    [SerializeField] private Transform _castPoint;
    [SerializeField] private List<Asteroid> _asteroids;

    public List<Asteroid> ObjectList => GetAsteroidsSorted();
    public bool ObjectDetected { get; private set; }

    void Start()
    {
        _asteroids = new List<Asteroid>();
        _layerMask = LayerMask.GetMask("Asteroid");
    }

    void Update()
    {
        ObjectDetected = CheckHit();
    }

    bool CheckHit()
    {
        bool hit = false;

        for (int i = 0; i < Parameters.rayAmount; i++)
        {
            RaycastHit2D ray = CastRayIndex(i);

            hit = hit || ray;
        }

        return hit;
    }


    public List<Asteroid> GetAsteroidsSorted()
    {
        _asteroids.Clear();

        // Cast Ray to detect asteroids
        for (int i = 0; i < Parameters.rayAmount; i++)
        {
            RaycastHit2D ray = CastRayIndex(i);
            if (ray)
            {
                Asteroid hitAsteroid = ray.transform.gameObject.GetComponent<Asteroid>();

                if (!_asteroids.Contains(hitAsteroid))
                {
                    _asteroids.Add(hitAsteroid);
                }
            }
        }

        _asteroids = SortAsteroids(_asteroids, _castPoint.position);
        return _asteroids;
    }

    bool IsHeadingTowardsPlayer(Asteroid asteroid, Vector2 playerPosition)
    {
        Vector2 toPlayer = (playerPosition - (Vector2)asteroid.transform.position).normalized;
        float dot = Vector2.Dot(toPlayer, asteroid.direction);
        return dot > 0; // > 0 means it's moving in the general direction of the player
    }

    List<Asteroid> SortAsteroids(List<Asteroid> asteroids, Vector2 playerPosition)
    {
        return asteroids
            .OrderByDescending(a => IsHeadingTowardsPlayer(a, playerPosition)) // true > false
            .ThenBy(a => Vector2.Distance(a.transform.position, playerPosition)) // closer first
            .ThenByDescending(a => a.GetScore()) // higher points first
            .ToList();
    }

    RaycastHit2D CastRayIndex(int index)
    {
        float dividedAngle = Parameters.detectionAngle / Parameters.rayAmount;
        float angleVariance = dividedAngle * index - Parameters.detectionAngle / 2;
        float hitLength = Parameters.rayLength;

        Vector2 direction = Vector2Extensions.RotateVector2(transform.up, angleVariance);

        RaycastHit2D ray = Physics2D.Raycast(_castPoint.position, direction, hitLength, _layerMask);

        if (ray)
        {
            hitLength = ray.distance;
        }

        Debug.DrawRay(_castPoint.position, direction * hitLength, Color.blue);

        return ray;
    }
}
