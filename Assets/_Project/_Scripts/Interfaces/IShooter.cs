using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter 
{
    GameObject GetGameObject { get;}

    void Shoot();

    void BulletDestroid(Bullet bullet);
}
