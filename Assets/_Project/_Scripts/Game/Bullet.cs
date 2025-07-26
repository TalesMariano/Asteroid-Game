using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 500f;
    public float maxLifetime = 10f;

    public IShooter Owner { get; set; }   // Should be Ishooter

    public Action OnDestroy;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(maxLifetime);
        DestroyBullet();
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
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
