using System.Collections;
using UnityEngine;

public class PlayerTank : HealthBase
{
    
    [SerializeField] private Transform tankT;
    [SerializeField] private Rigidbody tankRb;

    [Space]
    
    [SerializeField] private float enginePower;
    [SerializeField] private float rotationPower;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float maxRotation = 15;
    [SerializeField] private float driftModeOnSpeed = 5;
    [SerializeField] private float onFlyMovementSpeedModifier = 0.05f;

    [Space] 
    
    [SerializeField] private Collider[] tankWheelColliders;
    
    [SerializeField] private PhysicMaterial normalMaterial;
    [SerializeField] private PhysicMaterial driftMaterial;

    [Space] 
    
    [SerializeField] private Transform[] groundCheckT;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isFly;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private bool isTankDrifting;

    public Transform TankT => tankT;
    public Rigidbody TankRb => tankRb;

    public bool IsFly => isFly;
    
    private void Start()
    {
        StartCoroutine(FlyChecker());
        IEnumerator FlyChecker()
        {
            var wait = new WaitForSeconds(0.15f);
            while (true)
            {
                yield return wait;
                
                isFly = true;
                
                foreach (var groundCheck in groundCheckT)
                    if (groundCheck.Raycast(groundCheck.forward, groundCheckDistance, groundMask).distance != 0)
                    {
                        isFly = false;
                        break;
                    }
            }
        }
    }

    private void FixedUpdate()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");

        if (isFly)
        {
            vertical *= onFlyMovementSpeedModifier;
            horizontal *= onFlyMovementSpeedModifier;
        }
        
        Movement();
        void Movement()
        {
            Vector3 GetMoveVector()
            {
                var resultMoveVector = Vector3.zero;

                resultMoveVector = tankT.forward * vertical * enginePower * Time.deltaTime * 100;
                
                return resultMoveVector; 
            }

            Vector3 GetRotationVector()
            {
                var resultVector = Vector3.zero;

                resultVector.y = horizontal * rotationPower * Time.deltaTime * 100;

                // bool IsTankMoveBackwards()
                // {
                //     return vertical < 0;
                // }
                //
                // if (IsTankMoveBackwards())
                //     return -resultVector;
                
                return resultVector;
            }
            
            tankRb.AddForce(GetMoveVector(),ForceMode.Acceleration);
            tankRb.AddTorque(GetRotationVector(),ForceMode.Acceleration);
            
            tankRb.velocity = tankRb.velocity.ClampMagnitude(maxSpeed);
            tankRb.angularVelocity = tankRb.angularVelocity.ClampMagnitude(maxRotation);
        }

        DriftModeChange();
        void DriftModeChange()
        {
            void ChangePhysMaterial(PhysicMaterial physicMaterial)
            {
                foreach (var collider in tankWheelColliders)
                    collider.material = physicMaterial;
            }
            
            bool IsDriftModeOn()
            {
                return horizontal != 0 && tankRb.velocity.magnitude >= driftModeOnSpeed;
            }

            isTankDrifting = IsDriftModeOn();
            
            ChangePhysMaterial(isTankDrifting ? driftMaterial : normalMaterial);
        }
    }

    public override void Died()
    {
        
    }
}
