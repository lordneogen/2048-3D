using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem:MonoBehaviour
{
    public List<ParticleSystem> HighScoreSystem; 
    private void Start()
    {
        EventManager.Instance.EffectSystem = this;
    }

    public void Play(List<ParticleSystem> particleSystems)
    {
        foreach (var system in particleSystems)
        {
            system.Play();
        }
    }
}