using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PlayerTankMovement : MonoBehaviour
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
                    if (groundCheck.Raycast(groundCheck.forward, groundCheckDistance, groundMask) != null)
                    {
                        isFly = false;
                        break;
                    }
            }
        }
    }

    private void Update()
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

                bool IsTankMoveBackwards()
                {
                    return vertical < 0;
                }
                
                if (IsTankMoveBackwards())
                    return -resultVector;
                
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

            ChangePhysMaterial(horizontal != 0 && tankRb.velocity.magnitude >= driftModeOnSpeed ? driftMaterial : normalMaterial);
        }
    }
    
}
