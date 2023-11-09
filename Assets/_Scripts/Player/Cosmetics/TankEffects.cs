using System;
using UnityEngine;

public class TankEffects : MonoBehaviour
{
    [SerializeField] private PlayerTankCombat playerTankCombat;
    [SerializeField] private TankCorpusBouncing tankCorpusBouncing;
    
    [Space]
    
    [SerializeField] private GameObject boomEffect;

    [SerializeField] private ParticleSystem gunShotEffect;
    [SerializeField] private ParticleSystem gunShotEffect2;

    private void Start()
    {
        playerTankCombat.onGunShot += explosionPos => 
        {
            Instantiate(boomEffect, explosionPos, Quaternion.identity);
            gunShotEffect.Play();
            gunShotEffect2.Play();
            tankCorpusBouncing.TakeBounce(5);
        };
    }
}
