using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IDestructable
{
    public AsteroidParameters asteroidParameters;
    [SerializeField] private AsteroidSize size = AsteroidSize.Large;
    [SerializeField] private Asteroid smallerVersionPrefab;



    [Space]

    private Rigidbody2D rb;

    [SerializeField]
    public Vector2 direction { get; set; }

    public Vector2 Velocity
    {
        get { return rb.velocity; }
    }

    public Action OnDestroied { get; set ; }


    public float startForce = 500;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward);

        if (direction == null || direction == Vector2.zero)
            direction = UnityEngine.Random.insideUnitCircle.normalized;

        rb.AddForce(direction * startForce);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy();
        }
    }


    private void CreateChild()
    {
        if (!smallerVersionPrefab) return;

        if(asteroidParameters.splitAmount <= 0)
        {
            return;
        }else if(asteroidParameters.splitAmount == 1)
        {
            InstantiateChild(0);
        }
        else
        {
            float dividedAngle = asteroidParameters.SplitAngle / asteroidParameters.splitAmount;

            for (int i = 0; i < asteroidParameters.splitAmount; i++)
            {
                float angleVariance = dividedAngle*i - asteroidParameters.SplitAngle/2;

                InstantiateChild(angleVariance);
            }
        }
    }

    private void InstantiateChild(float angleVariance)
    {
        Asteroid asteroid = Instantiate(smallerVersionPrefab, transform.position, Quaternion.identity);

        asteroid.direction = RotateVector2(direction, angleVariance);

        asteroid.asteroidParameters = asteroidParameters;
    }

    [ContextMenu("Destroy")]
    public void Destroy()
    {
        OnDestroied?.Invoke();
        CreateChild();
        Destroy(gameObject);
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
