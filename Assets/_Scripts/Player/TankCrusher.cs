using System;
using UnityEngine;

public class TankCrusher : MonoBehaviour
{
    [SerializeField] private Transform tankT;
    [SerializeField] private Rigidbody tankRb;

    [SerializeField] private float toCrushNeededPower = 10;
    [SerializeField] private float crushDamage;
    [SerializeField] private float crushPower;
    [SerializeField] private bool toCrusherRotation = true;
    
    private void OnCollisionEnter(Collision collision)
    {
        Crush(collision.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Crush(other.transform);
    }

    void Crush(Transform objectT)
    {
        if(tankRb.velocity.magnitude < toCrushNeededPower)
            return;

        if (objectT.gameObject.TryGetComponent(out Rigidbody rb))
        {
            var punchDirection = -(tankT.position - rb.transform.position).normalized;
            rb.AddForce((punchDirection * crushPower) + tankRb.velocity,ForceMode.Impulse);

            if(toCrusherRotation)
                RotateTargetToTank();
            void RotateTargetToTank()
            {
                var toTankLookRotation = Quaternion.LookRotation(-punchDirection).eulerAngles;

                toTankLookRotation.x = 0;
                toTankLookRotation.z = 0;

                rb.transform.rotation = Quaternion.Euler(toTankLookRotation);
            }
        }
        
        if(objectT.gameObject.TryGetComponent(out IHealth health))
            health.TakeDamage(crushDamage);
    }
    
}
