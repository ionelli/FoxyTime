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

    public bool StarsActive => stars.isPlaying;
    
    public void PlayStars()
    {
        StartSystem(stars);
    }

    private Coroutine _smileyRoutine;
    public void PlaySmileys()
    {
        ParticleSystem.EmissionModule e = smileys.emission;
        StartSystem(smileys);
    }

    public void PlayHearts()
    {
        StartSystem(hearts);
    }

    public void StopStars()
    {
        
    }

    public void StopHearts()
    {
        
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
