using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    private int x;
    private int y;

    private PlayerState selectedBy;
    private PlayerState ownedBy;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public PlayerState SelectedBy { get => selectedBy; 
        set 
        {

            this.GetComponentInChildren<MeshRenderer>().material.color = Color.red;

            //TODO: selectedBy is empty.
            
            selectedBy = value;
        }
    }

    public PlayerState OwnedBy
    {
        get => ownedBy;
        set
        {
            //todo add colours to playerstates so we can do playerstate.playercolour
            //this.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            selectedBy = value;
        }
    }


    public UnityAction<PlayerState> onSelected;

    public UnityAction<PlayerState> onTaken;
}
