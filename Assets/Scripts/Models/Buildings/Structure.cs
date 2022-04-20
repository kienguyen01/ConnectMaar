using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Structure : MonoBehaviour
{
    public virtual bool IsBuilding { get => true;  }

    public virtual bool IsNode { get => false; }

    public virtual bool IsConnector { get => false; }

    public virtual bool IsSpecial  { get => false; }

    public virtual bool IsSolar { get => false; }

    public virtual bool IsHeat { get => false; }

    public virtual bool IsPlaceable { get => false; }

    protected bool solarRequired;
    protected bool heatRequired;
    public virtual bool SolarRequired { get => solarRequired; set => solarRequired = value; }
    public virtual bool HeatRequired { get => heatRequired; set => heatRequired = value; }
}
