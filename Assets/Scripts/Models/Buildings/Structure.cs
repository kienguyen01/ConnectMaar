﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    public virtual bool IsBuilding { get => true;  }

    public virtual bool IsNode { get => false; }

    public virtual bool IsConnector { get => false; }

    public virtual bool IsSpecial  { get => false; }

    public virtual bool IsSolar { get => false; }

    public virtual bool IsHeat { get => false; }

    public virtual bool IsPlaceable { get => false; }

    public virtual bool SolarRequired { set => value = false; get => false; }

    public virtual bool HeatRequired { set => value = false; get => false; }
}
