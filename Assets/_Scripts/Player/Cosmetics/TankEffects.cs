using System;
using UnityEngine;

public class TankEffects : MonoBehaviour
{
    [SerializeField] private PlayerTank playerTank; 
    [SerializeField] private PlayerTankCombat playerTankCombat;
    [SerializeField] private TankCorpusBouncing tankCorpusBouncing;
    
    [Space]
    
    [SerializeField] private GameObject boomEffect;
    [SerializeField] private ParticleSystem gunShotEffect;
    [SerializeField] private ParticleSystem gunShotEffect2;
    
    [Space]
    
    [SerializeField] private ParticleSystem metalSparksEffect;
    [SerializeField] private float driftDotCof;
    [SerializeField] private bool isTankDrifting;

    public bool IsTankDrifting => isTankDrifting;
    
    [Space]
    
    [SerializeField] private ParticleSystem leftUpWheelSmoke;
    [SerializeField] private ParticleSystem rightUpWheelSmoke;
    
    [SerializeField] private ParticleSystem leftDownWheelSmoke;
    [SerializeField] private ParticleSystem rightDownWheelSmoke;
    
    [Space]
    
    [SerializeField] private ParticleSystem machineGunShotEffect1;
    [SerializeField] private ParticleSystem machineGunShotEffect2;

    [SerializeField] private Transform machineGunShotEffect3T;
    [SerializeField] private ParticleSystem machineGunShotEffect3;

    [Space] 
    
    [SerializeField] private MeshRenderer leftChassis;
    [SerializeField] private MeshRenderer rightChassis;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float moveSpeedRb = 1;

    [Space] 
    
    [SerializeField] private TrailRenderer leftChassisTrace;
    [SerializeField] private TrailRenderer rightChassisTrace;
    [SerializeField] private TrailRenderer allChassisTrace;
    [SerializeField] private float zTraceAxis = 1.27f;

    private Transform leftChassisTraceT;
    private Transform rightChassisTraceT;
    private Transform allChassisTraceT;
    
    private static readonly int LineMovement = Shader.PropertyToID("_LineMovement");

    private void Start()
    {
        playerTankCombat.onGunShot += explosionPos => 
        {
            Instantiate(boomEffect, explosionPos, Quaternion.identity);
            gunShotEffect.Play();
            gunShotEffect2.Play();
            tankCorpusBouncing.TakeBounce(5);
        };
        
        machineGunShotEffect3T.parent = null;
        playerTankCombat.onMachineGunShot += shotPos =>
        {
            if (shotPos != Vector3.zero)
            {
                machineGunShotEffect3T.position = shotPos;
                machineGunShotEffect3.PlayP();
            }
            
            machineGunShotEffect1.PlayP();
            machineGunShotEffect2.PlayP();
        };

        leftChassisTraceT = leftChassisTrace.transform;
        rightChassisTraceT = rightChassisTrace.transform;
        allChassisTraceT = allChassisTrace.transform;
    }

    private void Update()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");
        var driftDot = Vector2.Dot(playerTank.TankT.forward.ToVectorXZ()
            , playerTank.TankRb.velocity.normalized.ToVectorXZ());
        
        MetalSparksEffectWork();
        void MetalSparksEffectWork()
        {
            var driftDotYes =
                Mathf.Abs(driftDot) <= driftDotCof;
            
            isTankDrifting =
                !playerTank.IsFly && driftDotYes && playerTank.TankRb.velocity.magnitude > 1;
            
            if (isTankDrifting)
            {
                if (!metalSparksEffect.isPlaying)
                    metalSparksEffect.Play();
            }
            else
            {
                if (metalSparksEffect.isPlaying)
                    metalSparksEffect.Stop();
            }
        }

        WheelSmokeMoveEffectWork();
        void WheelSmokeMoveEffectWork()
        {
            if(playerTank.IsMovementBlocked || playerTank.IsFly)
                return;
            
            MoveChassis(leftChassis,0,true);
            MoveChassis(rightChassis,0,true);

            if (vertical > 0)
            {
                leftDownWheelSmoke.PlayP();
                rightDownWheelSmoke.PlayP();
                
                MoveChassis(leftChassis,moveSpeed);
                MoveChassis(rightChassis,moveSpeed);
            }
            else if (vertical < 0)
            {
                leftUpWheelSmoke.PlayP();
                rightUpWheelSmoke.PlayP();
                
                MoveChassis(leftChassis,-moveSpeed);
                MoveChassis(rightChassis,-moveSpeed);
            }
            else
            {
                if (horizontal > 0)
                {
                    leftDownWheelSmoke.PlayP();
                    MoveChassis(leftChassis,moveSpeed);
                }
                else if (horizontal < 0)
                {
                    rightDownWheelSmoke.PlayP();
                    MoveChassis(rightChassis,moveSpeed);
                }
            }

            void MoveChassis(MeshRenderer chassisMesh, float moveSpeed, bool addRbSpeed = false)
            {
                var chassisMat = chassisMesh.sharedMaterial;

                var currentMoveVector = chassisMat.GetVector(LineMovement);

                if (addRbSpeed)
                    currentMoveVector.y += playerTank.TankRb.velocity.LengthXZ() * moveSpeedRb * driftDot * Time.timeScale;
                else
                    currentMoveVector.y += moveSpeed * Time.deltaTime * Time.timeScale;
                
                chassisMesh.sharedMaterial.SetVector(LineMovement,currentMoveVector);
            }
            
        }

        TraceWork();
        void TraceWork()
        {
            var isFly = playerTank.IsFly;

            leftChassisTrace.emitting = !isFly;
            rightChassisTrace.emitting = !isFly;
            allChassisTrace.emitting = !isFly;

            if(isFly)
                return;

            SideTracePossSet();
            void SideTracePossSet()
            {
                var localL = leftChassisTraceT.localPosition;
                var localR = rightChassisTraceT.localPosition;

                if (vertical > 0)
                {
                    localL.z = -zTraceAxis;
                    localR.z = -zTraceAxis;
                }
                else
                {
                    localL.z = zTraceAxis;
                    localR.z = zTraceAxis;
                }

                leftChassisTraceT.localPosition = localL;
                rightChassisTraceT.localPosition = localR;
            }

            AllTraceWork();
            void AllTraceWork()
            {
                allChassisTrace.emitting = (horizontal != 0 && vertical == 0) || isTankDrifting;

                var localA = allChassisTraceT.localPosition;

                if (vertical == 0 && !isTankDrifting)
                    localA.z = -(zTraceAxis / 4);
                else
                    localA.z = 0;

                allChassisTraceT.localPosition = localA;
            }
        }

    }

    public void Reset()
    {
        leftChassisTrace.Clear();
        rightChassisTrace.Clear();
        allChassisTrace.Clear();
    }
}
