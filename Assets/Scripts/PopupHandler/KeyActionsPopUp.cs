using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class KeyActionsPopUp : MonoBehaviour
{
    //void Start()
    //{

    //}


    private void Update()
    {
        //if (Input.GetKeyDown(SpacebarKey()))
        //{

        //    Debug.Log("entered");
        //    //PopupHander handler = GameObject.FindGameObjectWithTag("PopUpManager").GetComponent<PopupHander>();
        //    //handler.PopUp();
        //}


        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("entered");
        }



    }
    //public static KeyCode SpacebarKey()
    //{
    //    if (Application.isEditor) return KeyCode.O;
    //    else return KeyCode.Space;
    //}


}
