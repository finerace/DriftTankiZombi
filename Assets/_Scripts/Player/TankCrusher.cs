using UnityEngine;

public class TankCrusher : MonoBehaviour
{
    [SerializeField] private Transform tankT;
    [SerializeField] private Rigidbody tankRb;

    [SerializeField] private float toCrushNeededPower = 10;
    [SerializeField] private float crushDamage;
    [SerializeField] private float crushPower;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(tankRb.velocity.magnitude < toCrushNeededPower)
            return;

        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            var punchDirection = -(tankT.position - rb.transform.position).normalized;
            rb.AddForce((punchDirection * crushPower) + tankRb.velocity,ForceMode.Impulse);

            RotateTargetToTank();
            void RotateTargetToTank()
            {
                var toTankLookRotation = Quaternion.LookRotation(-punchDirection).eulerAngles;

                toTankLookRotation.x = 0;
                toTankLookRotation.z = 0;

                rb.transform.rotation = Quaternion.Euler(toTankLookRotation);
            }
        }
        
        if(collision.gameObject.TryGetComponent(out IHealth health))
            health.TakeDamage(crushDamage);
    }
    
}
