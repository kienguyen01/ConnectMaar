using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatPipeConnector : Connector
{
    public override bool IsSpecial { get => false; }

    // Start is called before the first frame update
    void Start()
    {
        length = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
