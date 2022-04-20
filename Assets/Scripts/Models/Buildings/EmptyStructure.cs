using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyStructure : Structure
{
    public override bool IsBuilding { get => false; }

    public override bool IsSpecial
    {
        get
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
