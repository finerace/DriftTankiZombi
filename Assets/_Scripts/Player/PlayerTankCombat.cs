using System;
using UnityEngine;

public class PlayerTankCombat : MonoBehaviour
{
    [SerializeField] private Transform tankT;
    [SerializeField] private Rigidbody tankRb;

    [Space] 
    
    [SerializeField] private Transform headT;
    [SerializeField] private float gunExplosionDamage = 20f;
    [SerializeField] private float gunCooldown = 2f;
    [SerializeField] private float gunExplosionPower = 15f;
    [SerializeField] private float gunExplosionRadius = 10f;
    [SerializeField] private float gunShotDistance = 15f;
    [SerializeField] private LayerMask gunShotLayerMask;
    [SerializeField] private Transform gunShotPoint;
    private float gunCooldownTimer;
    
    [Space]
    
    [SerializeField] private float machineGunDamage = 1;
    [SerializeField] private float machineGunCooldown = 0.25f;
    [SerializeField] private float machineGunShotDistance = 20f;
    [SerializeField] private LayerMask machineGunShotLayerMask;
    [SerializeField] private Transform machineGunShotPoint;
    private float machineGunCooldownTimer;

    public event Action<Vector3> onGunShot;
    public event Action<Vector3> onMachineGunShot; 

    private void Update()
    {
        CooldownTimersWork();
        void CooldownTimersWork()
        {
            if (gunCooldownTimer > 0)
                gunCooldownTimer -= Time.deltaTime;

            if (machineGunCooldownTimer > 0)
                machineGunCooldownTimer -= Time.deltaTime;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
            UseGun();
        
        void UseGun()
        {
            if(gunCooldownTimer > 0)
                return;
            gunCooldownTimer = gunCooldown;
            
            var raycastHit = gunShotPoint.Raycast(gunShotPoint.forward, gunShotDistance, gunShotLayerMask);

            var explosionPos = Vector3.zero;

            if (raycastHit.collider == null)
                explosionPos = gunShotPoint.position + gunShotPoint.forward * gunShotDistance;
            else
                explosionPos = raycastHit.point;
            
            Explosions.Explosion
            (explosionPos,
                gunExplosionPower,gunExplosionRadius,
                gunExplosionDamage,
                0,
                gunShotLayerMask,gunShotLayerMask);
            
            tankRb.AddForce(headT.forward * gunExplosionDamage * 10,ForceMode.Acceleration);
            onGunShot?.Invoke(explosionPos);
        }
        
        if(Input.GetKey(KeyCode.Space))
            UseMachineGun();
        
        void UseMachineGun()
        {
            if(machineGunCooldownTimer > 0)
                return;
            machineGunCooldownTimer = machineGunCooldown;
            
            var rayCastInfo = machineGunShotPoint.Raycast(machineGunShotPoint.forward, machineGunShotDistance,
                machineGunShotLayerMask);
            
            onMachineGunShot?.Invoke(rayCastInfo.point);
            
            if(rayCastInfo.distance == 0)
                return;

            if(rayCastInfo.collider.TryGetComponent(out IHealth health))
                health.TakeDamage(machineGunDamage);
        }
    }

    public void Reset()
    {
        gunCooldownTimer = 0;
        machineGunCooldownTimer = 0;
    }
}
