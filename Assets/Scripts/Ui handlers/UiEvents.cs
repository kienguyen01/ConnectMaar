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
    public void btnClickConnector2()
    {
        Debug.Log("It wotrks2");
        GameState.instance.SelectDoubleConnector();
    }
    public void btnClickConnector3()
    {
        Debug.Log("It wotrks3");
        GameState.instance.SelectTrippleConnector();
    }


}

