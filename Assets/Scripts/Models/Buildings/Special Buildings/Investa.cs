using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Investa : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public Investa()
    {
        solarRequired = true;
        heatRequired = true;
    }

    public override void GetSpecialBonus()
    {
        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector3());



        GameState.instance.player1.gameData.Inventory.Add(new StandardConnector2());


        base.GetSpecialBonus();
    }
}
