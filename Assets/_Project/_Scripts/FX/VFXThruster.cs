using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem ))]
public class VFXThruster : MonoBehaviour
{
    private IThruster thruster;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emissionModule;
    [SerializeField] private float rateOverTime = 15;

    void Awake()
    {
        thruster = GetComponentInParent<IThruster>();
        ps = GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
    }

    void Update()
    {
        if (thruster == null) return;

        emissionModule.rateOverTime = thruster.IsThrusting? rateOverTime: 0;
    }
}
