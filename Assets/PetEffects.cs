using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem stars;
    [SerializeField] private ParticleSystem smileys;
    [SerializeField] private float minSmileyEmission = 0.75f;
    [SerializeField] private float maxSmileyEmission = 3f;
    [SerializeField] private ParticleSystem hearts;
    [SerializeField] private float minFlexAngle = 10f;

    public bool StarsActive => stars.isPlaying;

    private PhysicalPetState _physicalState;
    private PetArduinoHandler _arduino;

    private void Awake()
    {
        _arduino = GetComponent<PetArduinoHandler>();
    }

    private void FixedUpdate()
    {
        bool prevMotion = _physicalState.motionDetected;
        float prevAngleLeft = _physicalState.flexAngleLeft;
        float prevAngleRight = _physicalState.flexAngleRight;
        _physicalState = _arduino.State;
        
        if(hearts.isPlaying)
            return;
        
        if (!prevMotion && _physicalState.motionDetected ||
            _physicalState.flexAngleLeft - prevAngleLeft > minFlexAngle ||
            _physicalState.flexAngleRight - prevAngleRight > minFlexAngle) 
        {
            StartSystem(hearts);
        }
    }

    public void PlayStars()
    {
        StartSystem(stars);
    }

    private Coroutine _smileyRoutine;
    public void PlaySmileys()
    {
        StartSystem(smileys);
        _smileyRoutine = StartCoroutine(C_Smileys());
    }

    private IEnumerator C_Smileys()
    {
        ParticleSystem.EmissionModule e = smileys.emission;
        
        while (smileys.isPlaying)
        {
            float nd = _physicalState.normalizedDistance;
            float emission = Mathf.Lerp(minSmileyEmission, maxSmileyEmission, nd);
            e.rateOverTimeMultiplier = emission;
            yield return null;
        }
    }

    public void StopSmileys()
    {
        StopSystem(smileys);
    }

    private void StartSystem(ParticleSystem system)
    {
        if(system.isPlaying)
            return;

        system.Play();
    }

    private void StopSystem(ParticleSystem system)
    {
        if(system.isStopped)
            return;
        
        system.Stop();
    }
}
