using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IDestructable, IScorable
{
    [SerializeField] 
    private SO_AsteroidParameters soAsteroidParameters;
    [SerializeField, Tooltip("Used when SO is null")] 
    private AsteroidParameters _debugAsteroidParameters;

    private AsteroidParameters Parameters
    {
        get { return soAsteroidParameters ? soAsteroidParameters.asteroidParameters : _debugAsteroidParameters; }
    }


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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameManager.Instance?.AsteroidCreated(this);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += ImediateDestroy;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= ImediateDestroy;
    }

    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward);

        if (direction == null || direction == Vector2.zero)
            direction = UnityEngine.Random.insideUnitCircle.normalized;

        rb.AddForce(direction * Parameters.startForce);


    }


    private void CreateChild()
    {
        if (!smallerVersionPrefab) return;

        if(Parameters.splitAmount <= 0)
        {
            return;
        }else if(Parameters.splitAmount == 1)
        {
            InstantiateChild(0);
        }
        else
        {
            float dividedAngle = Parameters.splitAngle / Parameters.splitAmount;

            for (int i = 0; i < Parameters.splitAmount; i++)
            {
                float angleVariance = dividedAngle*i - Parameters.splitAngle/2;

                InstantiateChild(angleVariance);
            }
        }
    }

    private void InstantiateChild(float angleVariance)
    {
        Asteroid asteroid = Instantiate(smallerVersionPrefab, transform.position, Quaternion.identity);

        asteroid.direction = Vector2Extensions.RotateVector2(direction, angleVariance);
    }

    [ContextMenu("Destroy")]
    public void Destroy()
    {
        OnDestroyed?.Invoke();
        CreateChild();

        GameManager.Instance?.AsteroidDestroied(this);

        Destroy(gameObject);
    }

    void ImediateDestroy()
    {
        Destroy(gameObject);
    }

    public int GetScore()
    {
        int score = 0;
        if (size == AsteroidSize.Large)
        {
            score = Parameters.largeScore;
        }
        else if (size == AsteroidSize.Medium)
        {
            score = Parameters.mediumScore;
        }
        else if (size == AsteroidSize.Small)
        {
            score = Parameters.smallScore;
        }
        return score;
    }

    public void ScorePoints()
    {
        GameManager.Instance?.AddScore(GetScore());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy();
        }
    }
}
