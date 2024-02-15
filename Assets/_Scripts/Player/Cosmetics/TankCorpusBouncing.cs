using System;
using UnityEngine;

public class TankCorpusBouncing : MonoBehaviour
{
    [SerializeField] private Transform tankCorpusT;
    [SerializeField] private Rigidbody tankRb;

    [Space] 
    
    [SerializeField] private float bouncePower = 10f;
    [SerializeField] private float bounceAddCof = 0.5f;
    [SerializeField] private float maxBouncing = 4;
    [SerializeField] private float maxBounceTimeCof = 0.85f;
    [SerializeField] private float bounceStartCof = 0.25f;

    private Vector3 corpusBouncing;

    private void Update()
    {
        corpusBouncing += tankRb.velocity * Time.deltaTime * bounceAddCof;
        corpusBouncing = corpusBouncing.ClampMagnitude(maxBouncing);

        corpusBouncing *= maxBounceTimeCof;

        var localEulerAngles = tankCorpusT.localEulerAngles;
        localEulerAngles = 
            new Vector3(corpusBouncing.magnitude,localEulerAngles.y,localEulerAngles.z) * 
            ((float)Math.Cos(Time.time * bouncePower) * 
             (1 - Mathf.Clamp(tankRb.velocity.magnitude * bounceStartCof,0,1)));
        
        tankCorpusT.localEulerAngles = localEulerAngles;
    }

    public void TakeBounce(float bounce)
    {
        corpusBouncing += Vector3.one * bounce;
    }
}