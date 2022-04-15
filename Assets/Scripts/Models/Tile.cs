using System;
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

    public Structure Structure;

    public bool occupied;

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

    public Tile SetStructure(Structure structure)
    {
        Structure = structure;
        occupied = true;
        return this;
    }

    public bool IsSpecial()
    {
        if ((this.Structure as SpecialBuilding).isSpecial)
        {
            return true;
        }
        else
        {
            Tile temp = new Tile();
            if(this.Y%2 == 0)
            {
                temp.X = this.X - 1;
                temp.Y = this.Y - 1;
                if ((temp.Structure as SpecialBuilding).isSpecial)
                {
                    return true;
                }
                temp.X = this.X - 1;
                temp.Y = this.Y;
                if ((temp.Structure as SpecialBuilding).isSpecial)
                {
                    return true;
                }
                temp.X = this.X - 1;
                temp.Y = this.Y + 1;
                if ((temp.Structure as SpecialBuilding).isSpecial)
                {
                    return true;
                }
            }
            else
            {
                temp.X = this.X;
                temp.Y = this.Y - 1;
                if ((temp.Structure as SpecialBuilding).isSpecial)
                {
                    return true;
                }
                temp.X = this.X - 1;
                temp.Y = this.Y;
                if ((temp.Structure as SpecialBuilding).isSpecial)
                {
                    return true;
                }
                temp.X = this.X;
                temp.Y = this.Y + 1;
                if ((temp.Structure as SpecialBuilding).isSpecial)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public UnityAction<PlayerState> onSelected;

    public UnityAction<PlayerState> onTaken;
}
