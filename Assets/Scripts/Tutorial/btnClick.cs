using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class btnClick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    public bool buttonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        Debug.Log("I can finallyu go to sleep");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    public void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse was clicked over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("KILL ME NOW");
            }
        }*/
    }


    public void BTN()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // This line prevents the Code from activating UI
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Debug.Log("THIS WORKS");    
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
