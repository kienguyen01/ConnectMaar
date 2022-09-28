using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DaltonCollege : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public DaltonCollege()
    {
        solarRequired = false;
        heatRequired = true;
    }

    public override void GetSpecialBonus()
    {
        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector3());
        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector3());

        base.GetSpecialBonus();
    }
}
