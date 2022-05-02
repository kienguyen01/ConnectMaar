using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEvents : MonoBehaviour
{

    void Update()
    {
        
    }

    public void btnClickConnector1()
    {
        Debug.Log("It wotrks");
        GameState.instance.SelectSingleConnector();

    }
}

