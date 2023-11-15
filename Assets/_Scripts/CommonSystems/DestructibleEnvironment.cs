using System;
using UnityEngine;

public class DestructibleEnvironment : HealthBase
{
    [SerializeField] private GameObject graphic;
    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private float destroyTime = 5;
    [SerializeField] private Rigidbody joint;
    private bool isDestroy;
    
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (!isDestroy && collision.gameObject.TryGetComponent(out PlayerTank player))
    //         StartDestroy();
    // }
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (!isDestroy && other.gameObject.TryGetComponent(out PlayerTank player))
    //         StartDestroy();
    // }

    private void Start()
    {
        if(joint != null)
            joint.transform.parent = null;
    }

    private void StartDestroy()
    {
        if(isDestroy)
            return;
            
        isDestroy = true;

        if (destroyEffect != null)
        {
            destroyEffect.Play();
            destroyEffect.transform.parent = transform;
        }

        gameObject.layer = 8;
        
        if(graphic != null)
            Destroy(graphic);
        
        if(joint != null)
            Destroy(joint.gameObject);
        
        Destroy(gameObject,destroyTime);
    }

    public override void Died()
    {
        StartDestroy();
    }
}
