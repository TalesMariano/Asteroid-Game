using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShooter : MonoBehaviour, IShooter
{
    [SerializeField] private SO_ShooterParameters soShooterParameters;
    [SerializeField, Tooltip("Used when SO is null")] private ShooterParameters _debugShooterParameters;
    private ShooterParameters Parameters
    {
        get { return soShooterParameters ? soShooterParameters.shooterParameters : _debugShooterParameters; }
    }

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _bulletPrefab;

    private HashSet<Bullet> _activeBullets = new HashSet<Bullet>();

    public GameObject GetGameObject => gameObject;

    void Update()
    {
        if (_activeBullets.Count < Parameters.maxAmountBullets &&  Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(_spawnPoint.position, _spawnPoint.rotation);            
        }
    }

    public void BulletDestroid( Bullet bullet)
    {
        _activeBullets.Remove(bullet);
    }

    public void Shoot(Vector3 spawnPosition, Quaternion rotation)
    {
        Bullet bullet = Instantiate(_bulletPrefab, spawnPosition, rotation);

        bullet.SetInitialValues(this, Parameters.bulletSpeed, Parameters.bulletLifeTime);

        _activeBullets.Add(bullet);
    }
}
