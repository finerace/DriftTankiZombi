using UnityEngine;

public class TankManageJoystick : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private PlayerTankObserveReference playerTank;
    [SerializeField] private bool tankHeadMode;
    
    private void Update()
    {
        if (!tankHeadMode)
        {
            playerTank.SetMobileMovementAxis("Vertical", joystick.Vertical);
            playerTank.SetMobileMovementAxis("Horizontal", joystick.Horizontal);
            
            //print($"Y={joystick.Vertical} X={joystick.Horizontal}");
            
            return;
        }

        var targetDirection = new Vector3(joystick.Horizontal, joystick.Vertical);
        
        playerTank.SetMobileTankHeadManageData(targetDirection,targetDirection.magnitude);

        //print($"Y={joystick.Vertical} X={joystick.Horizontal}");
    }
}
