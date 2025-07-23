using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{

    [Header("References")]
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    private Rigidbody2D rb;

    public SO_ShipParameters shipParametersSO;
    public ShipParameters shipParameters;
    
    public bool thrusting { get; private set; }
    private float turnDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (shipParametersSO) shipParameters = shipParametersSO.shipParameters;
    }



    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1f;
        }
        else
        {
            turnDirection = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(transform.up * shipParameters.thrustSpeed);
        }

        if (turnDirection != 0f)
        {
            transform.Rotate(Vector3.forward * shipParameters.rotationSpeed * turnDirection * Time.deltaTime * 100);
        }
    }

}
