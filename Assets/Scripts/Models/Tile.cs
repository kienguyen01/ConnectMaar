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
            if(value == null)
            {
                //remove highlight
                Destroy(this.gameObject.GetComponent<Outline>());
            }
            else
            {
                //change to highlight
                this.gameObject.AddComponent<Outline>()
                    .OutlineWidth = 4.0f;
                this.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
            }
            
            selectedBy = value;
        }
    }

    public PlayerState OwnedBy
    {
        get => ownedBy;
        set
        {
            //todo add colours to playerstates so we can do playerstate.playercolour
            this.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            ownedBy = value;
        }
    }

    private Tile SetStructure(Structure structure)
    {
        Structure = structure;

        occupied = false;
        occupied = structure.IsBuilding ? true : false;
        /*if (structure.isBuilding == true)
        {
            occupied = true;
        }
        else
        {
            occupied = false;
        }*/
        return this;
    }

    public bool IsSpecial(TileManager tm)
    {
        if (this.Structure.IsSpecial)
        {
            return true;
        }
        else
        {
            if(this.Y%2 == 0 && X > 0)
            {
                if (Y > 0 && tm.tiles[X - 1][Y - 1].Structure.IsSpecial)
                {
                    return true;
                }
                
                if (tm.tiles[X - 1][Y].Structure.IsSpecial)
                {
                    return true;
                }

                if (Y < (tm.tiles[0].Count - 1) && tm.tiles[X - 1][Y + 1].Structure.IsSpecial)
                {
                    return true;
                }
            }
            else
            {
                if (Y > 0 && tm.tiles[X][Y - 1].Structure.IsSpecial)
                {
                    return true;
                }

                if (X > 0 && tm.tiles[X - 1][Y].Structure.IsSpecial)
                {
                    return true;
                }

                if (Y < (tm.tiles[0].Count - 1) && tm.tiles[X][Y + 1].Structure.IsSpecial)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Tile AddStructure<T>()
    {
        Structure currentObj = this.gameObject.GetComponent<Structure>();

        if (currentObj != null)
        {
            Destroy(currentObj);
        }

        SetStructure((Structure)this.gameObject.AddComponent(typeof(T)));
        return this;
    }

    public UnityAction<PlayerState> onSelected;

    public UnityAction<PlayerState> onTaken;
}
