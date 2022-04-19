using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct TutorialStateConfig
{
    public PlayerState PlayerStateClass;
    public TileManager TileManagerClass;
}


public class TutorialState :  GameState
{
   
    bool firstBtn = false;
    Button button;


    public override void startPoint()
    {
        if (Input.GetKeyDown(KeyCode.A)|| Input.GetMouseButtonDown(1))
        {
            tileManager.tiles[0][0].OwnedBy = playerStates[0];
        }
    }



}
