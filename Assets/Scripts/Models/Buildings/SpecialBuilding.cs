using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBuilding : Structure
{
    public override bool IsSpecial { get => true; }
    public bool SolarRequired;
    public bool HeatRequired;
    public SpecialBuilding()
    {
        
    }
}
