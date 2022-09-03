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
        base.GetSpecialBonus();
    }
}
