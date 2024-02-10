using System;
using System.Collections;
using UnityEngine;

public class PlayerTank : HealthBase, IObserveNum
{
    
    [SerializeField] private Transform tankT;
    [SerializeField] private Rigidbody tankRb;
    [SerializeField] private PlayerTankCombat playerTankCombat;
    [SerializeField] private TankEffects tankEffects;

    [Space]
    
    [SerializeField] private float enginePower;
    [SerializeField] private float rotationPower;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float maxRotation = 15;
    [SerializeField] private float driftModeOnSpeed = 5;
    [SerializeField] private float onFlyMovementSpeedModifier = 0.05f;
    [SerializeField] private bool isMovementBlocked;

    public bool IsMovementBlocked => isMovementBlocked;
    
    [Space] 
    
    [SerializeField] private float fuel;
    [SerializeField] private float maxFuel;
    [SerializeField] private float fuelEatSpeed;
    [SerializeField] private float onStopFuelEatMultiplier;

    [Space] 
    
    [SerializeField] private Transform tankHead;
    [SerializeField] private float tankHeadRotationSpeed;
    
    [Space] 
    
    [SerializeField] private Collider[] tankWheelColliders;
    
    [SerializeField] private PhysicMaterial normalMaterial;
    [SerializeField] private PhysicMaterial driftMaterial;

    [Space] 
    
    [SerializeField] private Transform[] groundCheckT;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isFly;
    [SerializeField] private float groundCheckDistance = 0.2f;
    
    private bool isDriftModOn;
    
    private Coroutine flyChecker;
    
    public Transform TankT => tankT;
    public Rigidbody TankRb => tankRb;

    public bool IsFly => isFly;
    
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
            
            if(isMovementBlocked)
                return;
            
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

            isDriftModOn = IsDriftModeOn();
            
            ChangePhysMaterial(isDriftModOn ? driftMaterial : normalMaterial);
        }

        TankHeadRotation();
        void TankHeadRotation()
        {
            var direction = 0;

            if (Input.GetKey(KeyCode.Q))
                direction = -1;
            else if (Input.GetKey(KeyCode.E))
                direction = 1;
            else return;

            var euler = tankHead.localEulerAngles;
            var smooth = 0.01f;
            
            tankHead.localEulerAngles = 
                new Vector3(euler.x,euler.y + tankHeadRotationSpeed * smooth * direction,euler.z);
        }
        
        EatFuel();
        void EatFuel()
        {
            if (fuel <= 0)
            {
                if(!isDie)
                    TakeDamage(Health+1);
                
                return;
            }
            
            var isTankEngineWork = (vertical != 0 || horizontal != 0) && !isFly;
            var smooth = 0.01f;

            var fuelEat = fuelEatSpeed * smooth;

            if (!isTankEngineWork)
                fuelEat *= onStopFuelEatMultiplier;
                
            fuel -= fuelEat;
        }
    }

    public override void Died()
    {
        isMovementBlocked = true;
    }

    private void OnEnable()
    {
        if(flyChecker != null)
            StopCoroutine(flyChecker);
        
        flyChecker = StartCoroutine(FlyChecker());
        
        Reset();
    }

    private void OnDisable()
    {
        StopCoroutine(flyChecker);
    }

    IEnumerator FlyChecker()
    {
        var wait = new WaitForSeconds(0.15f);
        while (true)
        {
            yield return wait;
                
            isFly = true;

            if (tankRb.velocity.magnitude < 0.15f)
            {
                isFly = false;
                continue;
            }
            
            foreach (var groundCheck in groundCheckT)
                if (groundCheck.Raycast(groundCheck.forward, groundCheckDistance, groundMask).distance != 0)
                {
                    isFly = false;
                    break;
                }
        }
    }
    
    public void Reset()
    {
        isDie = false;
        health = maxHealth;
        tankRb.velocity = Vector3.zero;
        tankRb.angularVelocity = Vector3.zero;

        fuel = maxFuel;
        isMovementBlocked = false;
        
        tankHead.localEulerAngles = 
            new Vector3(tankHead.localEulerAngles.x,0,tankHead.localEulerAngles.z);

        playerTankCombat.Reset();
        tankEffects.Reset();
        
        OnBarParamChange?.Invoke();
    }

    public float GetNum(int id)
    {
        switch (id)
        {
            case 1:
                return health;
                break;
            
            case 2:
                return fuel;
                break;
            
            case 3:
                return playerTankCombat.GunCooldownTimer; 
                break;
            
            default:
                throw new Exception("This num id is not exist!");
        }
    }

    public (float min, float max) GetBarParam(int id)
    {
        switch (id)
        {
            case 1:
                return (0,maxHealth);
                break;
            
            case 2:
                return (0,maxFuel);
                break;
            
            case 3:
                return (0,playerTankCombat.GunCooldown); 
                break;
            
            default:
                throw new Exception("This num id is not exist!");
        }
    }

    public event Action OnBarParamChange;
    public event Action<int,int> OnObserveNumChange;
}
