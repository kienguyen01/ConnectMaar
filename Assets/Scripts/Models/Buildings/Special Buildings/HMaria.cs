using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HMaria : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public HMaria()
    {
        solarRequired = true;
        heatRequired = false;
    }

    public override void GetSpecialBonus()
    {
        for (int i = 0; i < 3; i++)
        {
            int rnd = Random.Range(1, 3);
            GameState.instance.player1.gameData.Inventory.Add(
                    (rnd == 1)? 
                new StandardConnector() : (
                    (rnd == 2)?
                new StandardConnector2() : 
                new StandardConnector3()));
        }

        base.GetSpecialBonus();
    }
}
