using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiEvents : MonoBehaviour
{


    void Update()
    {
    }

    public void DisableCamera()
    {
        GameObject varGameObject = GameObject.Find("object");
        varGameObject.GetComponent<PhoneCameraMovement>().enabled = false;

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

    public void BtnClickConnectorSolar()
    {
        Debug.Log("Solar");
        GameState.instance.SelectSolarConnector();    
    }

    public void BtnClickConnectorHeat()
    {
        Debug.Log("Heat");
        GameState.instance.SelectHeatConnector();    
    }

    public void BtnClickConnectorNode()
    {
        Debug.Log("Node");
        GameState.instance.SelectNodeConnector();
    }

    public void BtnClickEndTurn()
    {
        GameState.instance.turnCheck ^= true;
        if (GameState.instance.turnCheck == false)
        {
            GameObject varGameObject = GameObject.Find("object");

            varGameObject.GetComponent<PhoneCameraMovement>().enabled = true;

        }
    }

    public void CloseStadiumPopup()
    {
        GameState.instance.CheckStadiumPopup();
    }

    




}

