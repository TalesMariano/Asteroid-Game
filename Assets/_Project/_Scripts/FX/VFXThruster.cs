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


    private AudioSource audioSource;
    void Awake()
    {
        thruster = GetComponentInParent<IThruster>();
        ps = GetComponent<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();
        emissionModule = ps.emission;
    }

    void Update()
    {
        if (thruster == null) return;

        emissionModule.rateOverTime = thruster.IsThrusting? rateOverTime: 0;

        if(audioSource) audioSource.volume = thruster.IsThrusting ? 1 : 0;
    }
}
