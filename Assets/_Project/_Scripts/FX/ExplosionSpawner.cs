using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDestructable))]
public class ExplosionSpawner : MonoBehaviour
{
    private IDestructable destructable;
    [SerializeField] private VFXExplosion explosionVFX;

    private void Awake()
    {
        destructable = GetComponent<IDestructable>();
    }

    private void OnEnable()
    {
        destructable.OnDestroyed += Spawn;
    }

    private void OnDisable()
    {
        destructable.OnDestroyed -= Spawn;
    }
    
    void Spawn()
    {
        Instantiate(explosionVFX).transform.position = transform.position;
    }
}
