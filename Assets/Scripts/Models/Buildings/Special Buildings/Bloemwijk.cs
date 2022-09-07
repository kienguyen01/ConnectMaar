using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bloemwijk : SpecialBuilding
{
    public override bool IsSpecial { get => true; }
    public Bloemwijk()
    {
        solarRequired = false;
        heatRequired = true;
    }

    public override void GetSpecialBonus()
    {
        GameState.instance.player1.gameData.HasPassiveIncome = true;
        GameState.instance.player1.gameData.PassiveIncome += 1.0f;

        base.GetSpecialBonus();
    }
}
