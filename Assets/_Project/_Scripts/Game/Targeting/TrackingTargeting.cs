using UnityEngine;

public class TrackingTargeting : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform startPosition;
    [SerializeField] float power = 30f;
    [SerializeField] bool debugPause = true;


    //test
    public GameObject target;
    public ShipController tempShip;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TestCalculate();
        }
    }

    [ContextMenu("Calculate")]
    public void  TestCalculate()
    {
        TargetData data = new TargetData(target.transform.position, target.transform);
        Calculate(data);
    }


    public LaunchData Calculate(TargetData data)
    {
        //RotateArcher(data.targetPosition);
        LaunchData newData = CalculateFlight2D(data);


        DebugPause(newData);

        LaunchBullet(newData);
        return newData;
    }

    void DebugPause(LaunchData d)
    {
        if (!debugPause)
            return;

        Debug.DrawLine(startPosition.position, d.targetPosition, Color.red);

        Debug.DrawLine(startPosition.position, d.initialVelocity, Color.white);
        Debug.Break();
    }

    public void Launch(LaunchData data)
    {
        Debug.Log("Left click to fire a predicting arrow at a moving target, targets move too fast, there barely time to first aim and then fire");
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target.With(y: transform.position.y));
        transform.rotation = Quaternion.LookRotation(dir);
    }

    LaunchData CalculateFlight(TargetData target)
    {
        LaunchData arrowData = new LaunchData();
        Rigidbody targetRigidbody = target.targetObject.GetComponent<Rigidbody>();

        Vector3 velocity = GetArrowVelocity(targetRigidbody, target);
        if (targetRigidbody != null)
            arrowData.timeToTarget = TimeToImpact(targetRigidbody);

        return FillArrowData(arrowData, velocity, target);
    }

    Vector3 GetArrowVelocity(Rigidbody targetRigidbody, TargetData target)
    {
        Vector3 v = targetRigidbody != null ? AnticipateVelocity(targetRigidbody).normalized : startPosition.position.DirectionTo(target.targetPosition).normalized;
        return v;
    }

    LaunchData FillArrowData(LaunchData arrowData, Vector3 velocity, TargetData target)
    {
        arrowData.initialVelocity = velocity * power;
        arrowData.initialPosition = startPosition.position;
        arrowData.targetPosition = target.targetPosition;
        arrowData.horizontalDistance = Vector3.Distance(arrowData.initialPosition, target.targetPosition.With(y: arrowData.initialPosition.y));
        return arrowData;
    }

    void LaunchArrow(LaunchData lData)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = lData.initialVelocity;
    }

    float TimeToImpact(Rigidbody target)
    {
        Vector3 estimatedRigidbodyVelocity = (target.position - startPosition.position).normalized * power;

        float distance = Vector3.Distance(startPosition.position, target.position);

        Vector3 relativeVelocity = estimatedRigidbodyVelocity - target.velocity;

        return distance / relativeVelocity.magnitude;
    }

    Vector3 AnticipateVelocity(Rigidbody target)
    {
        float timeToHit = TimeToImpact(target);
        Vector3 expectedPosition = target.position + target.velocity * timeToHit;
        Vector3 dir = expectedPosition - startPosition.position;
        return dir.normalized;
    }


    //Tales Stuff

    float TimeToImpact(Rigidbody2D target)
    {
        Vector2 estimatedRigidbodyVelocity = (target.position - (Vector2)startPosition.position).normalized * power;

        float distance = Vector2.Distance(startPosition.position, target.position);

        Vector2 relativeVelocity = estimatedRigidbodyVelocity - target.velocity;

        return distance / relativeVelocity.magnitude;
    }

    Vector2 AnticipateVelocity(Rigidbody2D target)
    {
        float timeToHit = TimeToImpact(target);
        Vector3 expectedPosition = target.position + target.velocity * timeToHit;
        Vector3 dir = expectedPosition - startPosition.position;
        return dir.normalized;
    }

    void LaunchArrow2D(LaunchData lData)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = lData.initialVelocity;
    }

    void LaunchBullet(LaunchData lData)
    {
        Vector2 velocity = lData.initialVelocity.normalized;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg; //(velocity.y, velocity.x)

        Bullet bullet = Instantiate(arrow, startPosition.position, Quaternion.Euler(0, 0, angle-90)).GetComponent<Bullet>();
        bullet.Owner = tempShip.GetComponent<IShooter>();
        //newArrow.GetComponent<Rigidbody2D>().velocity = lData.initialVelocity;
    }
    LaunchData CalculateFlight2D(TargetData target)
    {
        LaunchData arrowData = new LaunchData();
        Rigidbody2D targetRigidbody = target.targetObject.GetComponent<Rigidbody2D>();

        Vector3 velocity = GetArrowVelocity2D(targetRigidbody, target);
        if (targetRigidbody != null)
            arrowData.timeToTarget = TimeToImpact(targetRigidbody);

        return FillArrowData(arrowData, velocity, target);
    }

    Vector3 GetArrowVelocity2D(Rigidbody2D targetRigidbody, TargetData target)
    {
        Vector3 v = targetRigidbody != null ? AnticipateVelocity(targetRigidbody).normalized : startPosition.position.DirectionTo(target.targetPosition).normalized;
        return v;
    }

}