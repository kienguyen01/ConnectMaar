using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecialBuilding : Structure
{
    public override bool IsSpecial { get => true; }
    public SpecialBuilding()
    {
        
    }

    public virtual void GetSpecialBonus() { GameState.instance.player1.gameData.totalPoint -= 2; }
}
