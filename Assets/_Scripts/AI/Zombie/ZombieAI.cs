using System;
using System.Collections;
using UnityEngine;

public class ZombieAI : HealthBase
{
    [SerializeField] private Transform zombieT;
    [SerializeField] private Rigidbody zombieRb;

    [Space] 
    
    [SerializeField] private float damage;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackCooldown;
    
    private IHealth targetHealth;
    private float attackCooldownTimer;
    
    [Space]
    
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private bool isAnnoyed = false;
    
    [Space]
    
    [SerializeField] private Transform targetT;
    [SerializeField] private float lookTargetDistance;
    [SerializeField] private float lookTargetDotFov;

    public event Action onAnnoyed;
    
    private void Start()
    {
        StartCoroutine(LookTargetUpdater());
        IEnumerator LookTargetUpdater()
        {
            var wait = 0.4f;
            
            while (true)
            {
                yield return wait;
                
                if(isAnnoyed)
                    continue;
                
                if(Vector3.Distance(zombieT.position,targetT.position) > lookTargetDistance)
                    continue;
                
                var toTargetDirection = (targetT.position - zombieT.position).normalized;
                
                if (Vector3.Dot(zombieT.forward, toTargetDirection) >= lookTargetDotFov)
                {
                    isAnnoyed = true;
                    
                    onAnnoyed?.Invoke();
                }
            }
        }

        targetT.TryGetComponent(out targetHealth);
    }

    private void Update()
    {
        if (!isAnnoyed || isDie) 
            return;
        
        var toTargetDirection = (targetT.position - zombieT.position).normalized;
        
        void MoveToTarget()
        {
            zombieRb.AddForce(toTargetDirection * Time.deltaTime * speed * 100,ForceMode.Acceleration);
        }
        void RotateToTarget()
        {
            var targetRotationEuler = 
                Quaternion.LookRotation(toTargetDirection).eulerAngles;

            targetRotationEuler.x = 0;
            targetRotationEuler.z = 0;

            zombieT.rotation = 
                Quaternion.RotateTowards(zombieT.rotation,  Quaternion.Euler(targetRotationEuler),
                    Time.deltaTime * rotationSpeed  * 100);
        }

        MoveToTarget();
        RotateToTarget();
        
        if (Vector3.Distance(zombieT.position, targetT.position) > attackDistance)
            return;
        
        void AttackTarget()
        {
            if (targetHealth == null) 
                return;

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
                return;    
            }
            
            targetHealth.TakeDamage(damage);
            attackCooldownTimer = attackCooldown;
        }
        
        AttackTarget();
    }
    
    public override void Died()
    {
        gameObject.layer = 8;
        Destroy(gameObject,30f);
    }
}