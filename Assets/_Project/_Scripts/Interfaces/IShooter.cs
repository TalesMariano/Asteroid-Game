using UnityEngine;

public interface IShooter 
{
    GameObject GetGameObject { get;}

    void Shoot(Vector3 spawnPosition, Quaternion rotation);

    void BulletDestroid(Bullet bullet);
}
