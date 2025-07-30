using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem ))]
public class VFXThruster : MonoBehaviour
{
    private IThruster _thruster;
    private ParticleSystem _ps;
    private ParticleSystem.EmissionModule _emissionModule;
    private AudioSource _audioSource;
    [SerializeField] private float _rateOverTime = 15;


    void Awake()
    {
        _thruster = GetComponentInParent<IThruster>();
        _ps = GetComponent<ParticleSystem>();

        _audioSource = GetComponent<AudioSource>();
        _emissionModule = _ps.emission;
    }

    void Update()
    {
        if (_thruster == null) return;

        _emissionModule.rateOverTime = _thruster.IsThrusting? _rateOverTime: 0;

        if(_audioSource) _audioSource.volume = _thruster.IsThrusting ? 1 : 0;
    }
}
