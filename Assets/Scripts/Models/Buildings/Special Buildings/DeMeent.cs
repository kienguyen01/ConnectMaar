using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeMeent : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public DeMeent()
    {
        solarRequired = true;
        heatRequired = false;
    }

    public override void GetSpecialBonus()
    {
        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector3());
    }
}
