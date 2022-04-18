﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    public virtual bool IsBuilding { get => true;  }

    public virtual bool IsSpecial  { get => false; }
}