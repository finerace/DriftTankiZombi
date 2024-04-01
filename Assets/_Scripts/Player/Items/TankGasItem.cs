using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGasItem : PlayerItem
{
    protected override void ItemWork(PlayerTank playerTank)
    {
        playerTank.Fuel = playerTank.MaxFuel;
    }
}
