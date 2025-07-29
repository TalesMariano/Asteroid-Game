using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShooter : MonoBehaviour, IShooter
{
    [SerializeField] Transform spawnPoint;

    public int maxBullets = 1;

    public Bullet bulletPrefab;

    private HashSet<Bullet> activeBullets = new HashSet<Bullet>();

    public GameObject GetGameObject => gameObject;

    void Update()
    {
        if (activeBullets.Count < maxBullets &&  Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();            
        }
    }

    public void BulletDestroid( Bullet bullet)
    {
        activeBullets.Remove(bullet);
    }

    public void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        bullet.Owner = this;

        activeBullets.Add(bullet);
    }

}
