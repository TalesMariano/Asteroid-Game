using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 500f;
    private float _maxLifetime = 10f;

    public IShooter Owner { get; set; } 

    public Action OnDestroy;

    public void SetInitialValues(IShooter owner, float speed, float maxLifetime)
    {
        Owner = owner;
        _speed = speed;
        _maxLifetime = maxLifetime;
    }


    IEnumerator Start()
    {
        yield return new WaitForSeconds(_maxLifetime);
        DestroyBullet();
    }

    void Update()
    {
        transform.position += transform.up * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Owner.GetGameObject) return;
            
            
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Owner?.BulletDestroid(this);
        OnDestroy?.Invoke();
        Destroy(gameObject);
    }
}
