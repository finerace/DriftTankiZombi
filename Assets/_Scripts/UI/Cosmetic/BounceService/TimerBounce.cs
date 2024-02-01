using System.Collections;
using UnityEngine;

public class TimerBounce : BaseBounceService
{

    [Space] 
    
    [SerializeField] private float bounceCooldown;

    protected new void Start()
    {
        StartCoroutine(BounceTImer());
        
       base.Start(); 
    }
    
    private IEnumerator BounceTImer()
    {
        var cooldownWait = new WaitForSeconds(bounceCooldown);

        while (true)
        {
            yield return cooldownWait;
            
            Bounce();
        }
    }

}
