using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingShooter : MonoBehaviour, IShooter
{
    public ShipController tempShip;
    public ITracker<Asteroid> asteroidTracker;


    public Bullet bulletPrefab;


    [SerializeField] float bulletPower = 30f;

    [SerializeField] float bulletCooldown = 1f;
    private float currentCooldown;

    [SerializeField] Transform startPosition;


    public GameObject GetGameObject => gameObject;

    private void Start()
    {
        asteroidTracker = GetComponent<ITracker<Asteroid>>();
        tempShip = GetComponent<ShipController>();
    }

    private void Update()
    {
        //Bullet cooldown
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else
        {
            //If ready to shoot
            if (asteroidTracker.listItems.Count > 0 && asteroidTracker.listItems[0] != null)
            {
                ShootTarget(asteroidTracker.listItems[0].transform);
                currentCooldown = bulletCooldown;
            }
        }
    }


    void ShootTarget(Transform target )
    {
        TargetData data = new TargetData(target.position, target);

        FireBullet(Calculate(data));
        
    }

    public LaunchData Calculate(TargetData data)
    {
        bulletPower = bulletPrefab.speed;

        LaunchData newData = CalculatePath2D(data);

        //DebugPause(newData);
        return newData;
    }

    LaunchData CalculatePath2D(TargetData target)
    {
        LaunchData bullet = new LaunchData();
        Rigidbody2D targetRigidbody = target.targetObject.GetComponent<Rigidbody2D>();

        Vector3 velocity = GetBulletVelocity2D(targetRigidbody, target);
        if (targetRigidbody != null)
            bullet.timeToTarget = TimeToImpact(targetRigidbody);

        return FillBulletData(bullet, velocity, target);
    }

    Vector3 GetBulletVelocity2D(Rigidbody2D targetRigidbody, TargetData target)
    {
        Vector3 v = targetRigidbody != null ? AnticipateVelocity(targetRigidbody).normalized : startPosition.position.DirectionTo(target.targetPosition).normalized;
        return v;
    }

    Vector2 AnticipateVelocity(Rigidbody2D target)
    {
        float timeToHit = TimeToImpact(target);
        Vector3 expectedPosition = target.position + target.velocity * timeToHit;
        Vector3 dir = expectedPosition - startPosition.position;
        return dir.normalized;
    }

    float TimeToImpact(Rigidbody2D target)
    {
        Vector2 estimatedRigidbodyVelocity = (target.position - (Vector2)startPosition.position).normalized * bulletPower;
        float distance = Vector2.Distance(startPosition.position, target.position);
        Vector2 relativeVelocity = estimatedRigidbodyVelocity - target.velocity;
        return distance / relativeVelocity.magnitude;
    }

    LaunchData FillBulletData(LaunchData arrowData, Vector3 velocity, TargetData target)
    {
        arrowData.initialVelocity = velocity * bulletPower;
        arrowData.initialPosition = startPosition.position;
        arrowData.targetPosition = target.targetPosition;
        arrowData.horizontalDistance = Vector3.Distance(arrowData.initialPosition, target.targetPosition.With(y: arrowData.initialPosition.y));
        return arrowData;
    }

    void FireBullet(LaunchData lData)
    {
        Vector2 velocity = lData.initialVelocity.normalized;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg; //(velocity.y, velocity.x)

        Bullet bullet = Instantiate(bulletPrefab, startPosition.position, Quaternion.Euler(0, 0, angle - 90)).GetComponent<Bullet>();
        bullet.Owner = tempShip.GetComponent<IShooter>();
    }

    void DebugPause(LaunchData d)
    {
        Debug.DrawLine(startPosition.position, d.targetPosition, Color.red);
        Debug.DrawLine(startPosition.position, d.initialVelocity, Color.white);
        Debug.Break();
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public void BulletDestroid(Bullet bullet)
    {
        throw new System.NotImplementedException();
    }
}
