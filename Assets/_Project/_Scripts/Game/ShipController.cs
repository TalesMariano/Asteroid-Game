using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour, IDestructable, IIntangible, IThruster
{
    [SerializeField] 
    private SO_ShipParameters shipParametersSO;
    [SerializeField, Tooltip("Used when SO is null")] 
    private ShipParameters _debugShipParameters;

    private ShipParameters Parameters
    {
        get { return shipParametersSO ? shipParametersSO.shipParameters : _debugShipParameters; }
    }

    private Rigidbody2D _rb;
    private float _turnDirection;

    public bool IsThrusting { get; private set; }
    public Action OnDestroyed { get; set;}
    public Action<bool> OnChangeIntangible { get; set; }



    private bool isIntangible = false;
    public bool IsIntangible
    {
        get { return isIntangible; }
        private set
        {
            isIntangible = value;
            OnChangeIntangible?.Invoke(isIntangible);
        }
    }


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator Start()
    {
        IsIntangible = true;
        yield return new WaitForSeconds(Parameters.intangibilityDuration);
        IsIntangible = false;
    }

    private void Update()
    {
        IsThrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1f;
        }
        else
        {
            _turnDirection = 0f;
        }

    }

    private void FixedUpdate()
    {
        if (IsThrusting)
        {
            _rb.AddForce(transform.up * Parameters.thrustSpeed * Time.deltaTime * 100);
        }

        if (_turnDirection != 0f)
        {
            transform.Rotate(Vector3.forward * Parameters.rotationSpeed * _turnDirection * Time.deltaTime * 100);
        }
    }


    [ContextMenu("Destroy")]
    public void Destroy()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsIntangible && collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy();
        }
    }
}
