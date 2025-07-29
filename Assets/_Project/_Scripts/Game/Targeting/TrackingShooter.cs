using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingShooter : MonoBehaviour, IShooter
{
    private ITracker<Asteroid> _asteroidTracker;

    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private SO_ShooterParameters soShooterParameters;
    [SerializeField, Tooltip("Used when SO is null")] private ShooterParameters _debugShooterParameters;
    private ShooterParameters Parameters
    {
        get { return soShooterParameters ? soShooterParameters.shooterParameters : _debugShooterParameters; }
    }

    private float _currentCooldown;

    [SerializeField] Transform spawnPoint;

    public GameObject GetGameObject => gameObject ? gameObject : null;


    private HashSet<Bullet> _activeBullets = new HashSet<Bullet>();


    private void Awake()
    {
        _asteroidTracker = GetComponent<ITracker<Asteroid>>();
    }

    private void Start()
    {
        _currentCooldown = Parameters.bulletCooldown;
    }

    private void Update()
    {
        //Bullet cooldown
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (_activeBullets.Count < Parameters.maxAmountBullets)
        {
            //If ready to shoot
            if (_asteroidTracker.ObjectDetected)
            {
                Transform target = _asteroidTracker.ObjectList[0].transform;

                ShootTarget(target);

                _currentCooldown = Parameters.bulletCooldown;
            }
        }
    }


    void ShootTarget(Transform target )
    {
        TargetData data = new TargetData(target.position, target);
        Vector2 velocity = Calculate(data).initialVelocity.normalized;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        Shoot(spawnPoint.position, Quaternion.Euler(0, 0, angle - 90));
    }

    public void Shoot(Vector3 spawnPosition, Quaternion rotation)
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPosition, rotation);
        bullet.SetInitialValues(this, Parameters.bulletSpeed, Parameters.bulletLifeTime);
        _activeBullets.Add(bullet);
    }

    public LaunchData Calculate(TargetData data)
    {
        LaunchData newData = CalculatePath2D(data);

        //DebugPause(newData);
        return newData;
    }

    LaunchData CalculatePath2D(TargetData target)
    {
        LaunchData bulletData = new LaunchData();
        Rigidbody2D targetRigidbody = target.targetObject.GetComponent<Rigidbody2D>();

        Vector3 velocity = GetBulletVelocity2D(targetRigidbody, target);
        if (targetRigidbody != null)
            bulletData.timeToTarget = TimeToImpact(targetRigidbody);

        return FillBulletData(bulletData, velocity, target);
    }

    Vector3 GetBulletVelocity2D(Rigidbody2D targetRigidbody, TargetData target)
    {
        Vector3 v = targetRigidbody != null ? AnticipateVelocity(targetRigidbody).normalized : spawnPoint.position.DirectionTo(target.targetPosition).normalized;
        return v;
    }

    Vector2 AnticipateVelocity(Rigidbody2D target)
    {
        float timeToHit = TimeToImpact(target);
        Vector3 expectedPosition = target.position + target.velocity * timeToHit;
        Vector3 dir = expectedPosition - spawnPoint.position;
        return dir.normalized;
    }

    float TimeToImpact(Rigidbody2D target)
    {
        Vector2 estimatedRigidbodyVelocity = (target.position - (Vector2)spawnPoint.position).normalized * Parameters.bulletSpeed;
        float distance = Vector2.Distance(spawnPoint.position, target.position);
        Vector2 relativeVelocity = estimatedRigidbodyVelocity - target.velocity;
        return distance / relativeVelocity.magnitude;
    }

    LaunchData FillBulletData(LaunchData bulletData, Vector3 velocity, TargetData target)
    {
        bulletData.initialVelocity = velocity * Parameters.bulletSpeed;
        bulletData.initialPosition = spawnPoint.position;
        bulletData.targetPosition = target.targetPosition;
        bulletData.horizontalDistance = Vector3.Distance(bulletData.initialPosition, target.targetPosition.With(y: bulletData.initialPosition.y));
        return bulletData;
    }

    void DebugPause(LaunchData d)
    {
        Debug.DrawLine(spawnPoint.position, d.targetPosition, Color.red);
        Debug.DrawLine(spawnPoint.position, d.initialVelocity, Color.white);
        Debug.Break();
    }

    public void BulletDestroid(Bullet bullet)
    {
        _activeBullets.Remove(bullet);
    }
}
