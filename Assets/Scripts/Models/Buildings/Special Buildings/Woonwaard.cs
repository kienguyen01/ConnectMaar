using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Woonwaard : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public Woonwaard()
    {
        solarRequired = false;
        heatRequired = true;
    }

    public override void GetSpecialBonus()
    {
        GameState.instance.player1.gameData.handSize += 1;

        base.GetSpecialBonus();
    }
}
