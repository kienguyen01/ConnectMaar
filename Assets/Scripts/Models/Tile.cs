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
    private Tile specialOriginTile;
    public bool IsScrambleForSolar;
    public bool IsScrabbleForHeat;

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
                if (this.gameObject.GetComponent<Outline>())
                {
                    Destroy(this.gameObject.GetComponent<Outline>());
                }
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
            this.GetComponentInChildren<MeshRenderer>().material.color = value.gameData.PlayerColour;
            ownedBy = value;
        }
    }

    private Tile SetStructure(Structure structure)
    {
        Structure = structure;
        occupied = structure.IsBuilding || structure.IsConnector || structure.IsSolar || structure.IsHeat || structure.IsPlaceable;
        return this;
    }

    public bool HasBuilding()
    {
        return this.Structure.IsBuilding;
    }

    public bool IsSpecial()
    {
        if (this.Structure.IsSpecial)
        {
            specialOriginTile = this;
            return true;
        }
        else
        {
            if(this.Y%2 == 0 && X > 0)
            {
                if (Y > 0 && TileManager.tiles[X - 1][Y - 1].Structure.IsSpecial)
                {
                    specialOriginTile = TileManager.tiles[X - 1][Y - 1];
                    return true;
                }
                try
                {
                    if (TileManager.tiles[X - 1][Y].Structure.IsSpecial)
                    {
                        specialOriginTile = TileManager.tiles[X - 1][Y];
                        return true;
                    }
                }
                catch(Exception EX)
                {
                    Debug.LogWarning(EX.Message);
                }
                try
                {
                    if (Y < (TileManager.tiles[0].Count - 1) && TileManager.tiles[X - 1][Y + 1].Structure.IsSpecial)
                    {
                        specialOriginTile = TileManager.tiles[X - 1][Y + 1];
                        return true;
                    }
                }
                catch(Exception EX)
                {
                    Debug.LogWarning(EX.Message);
                }
            }
            else
            {
                if (Y > 0 && TileManager.tiles[X][Y - 1].Structure.IsSpecial)
                {
                    specialOriginTile = TileManager.tiles[X][Y - 1];
                    return true;
                }

                if (X > 0 && TileManager.tiles[X - 1][Y].Structure.IsSpecial)
                {
                    specialOriginTile = TileManager.tiles[X - 1][Y];
                    return true;
                }

                if (Y < (TileManager.tiles[0].Count - 1) && TileManager.tiles[X][Y + 1].Structure.IsSpecial)
                {
                    specialOriginTile = TileManager.tiles[X][Y + 1];
                    return true;
                }
            }
        }
        return false;
    }

    public Tile GetSpecialOriginTile()
    {
        if (this.IsSpecial())
            return specialOriginTile;
        else
            return null;
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

    public Tile AddStructure<T>(Structure s)
    {
        s.transform.SetParent(this.transform);
        SetStructure(s);
        return this;
    }

    public UnityAction<PlayerState> onSelected;

    public UnityAction<PlayerState> onTaken;

    public UnityAction<PlayerState> openInfoCard;
}
