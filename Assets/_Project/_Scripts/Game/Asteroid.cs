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

    public Action OnDestroyed { get; set ; }
    public Action<bool> OnChangeIntangible { get; set; }

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


        GameManager.Instance?.AsteroidCreated(this);
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

        asteroid.direction = Vector2Extensions.RotateVector2(direction, angleVariance);

        asteroid.asteroidParameters = asteroidParameters;
    }

    [ContextMenu("Destroy")]
    public void Destroy()
    {
        OnDestroyed?.Invoke();
        CreateChild();

        GameManager.Instance?.AsteroidDestroied(this);

        Destroy(gameObject);
    }

    public int GetScore()
    {
        int score = 0;
        if (size == AsteroidSize.Large)
        {
            score = asteroidParameters.largeScore;
        }
        else if (size == AsteroidSize.Medium)
        {
            score = asteroidParameters.mediumScore;
        }
        else if (size == AsteroidSize.Small)
        {
            score = asteroidParameters.smallScore;
        }
        return score;
    }

    private void AwardPoints()
    {
        GameManager.Instance?.AddScore(GetScore());
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy();
        }
        else if (collision.CompareTag("Bullet"))
        {
            AwardPoints();
            Destroy();
        }
    }
}
