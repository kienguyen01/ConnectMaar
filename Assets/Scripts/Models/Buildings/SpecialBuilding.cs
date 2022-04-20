using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBuilding : Structure
{

    private bool solarRequired;
    private bool heatRequired;

    public override bool IsSpecial { get => true; }

    public override bool SolarRequired { set => solarRequired = value; get => solarRequired; }

    public override bool HeatRequired { set => heatRequired = value; get => heatRequired; }
    public SpecialBuilding()
    {
        
    }
}
