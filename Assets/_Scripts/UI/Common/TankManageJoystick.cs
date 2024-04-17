using System;
using UnityEngine;

public class TankManageJoystick : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private PlayerTankObserveReference playerTank;
    [SerializeField] private bool tankHeadMode;
    [SerializeField] private AxisOptions targetAxis;
    
    private void Update()
    {
        if (!tankHeadMode)
        {
            switch (targetAxis)
            {
                case AxisOptions.Both:
                {
                    playerTank.SetMobileMovementAxis("Vertical", joystick.Vertical);
                    playerTank.SetMobileMovementAxis("Horizontal", joystick.Horizontal);
                }
                    break;
                case AxisOptions.Horizontal:
                {
                    playerTank.SetMobileMovementAxis("Horizontal", joystick.Horizontal);
                }
                    break;
                case AxisOptions.Vertical:
                {
                    playerTank.SetMobileMovementAxis("Vertical", joystick.Vertical);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
            
            //print($"Y={joystick.Vertical} X={joystick.Horizontal}");
            
            return;
        }

        var targetDirection = new Vector3(joystick.Horizontal, joystick.Vertical);
        
        playerTank.SetMobileTankHeadManageData(targetDirection,targetDirection.magnitude);

        //print($"Y={joystick.Vertical} X={joystick.Horizontal}");
    }
}
