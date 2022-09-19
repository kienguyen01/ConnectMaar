using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AFAS : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public AFAS()
    {
        solarRequired = true;
        heatRequired = false;
    }

    public override void GetSpecialBonus()
    {
        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector2());
        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector2());
    }
}
