using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBuildings : MonoBehaviour
{
    public Tile original;
    public Tile firstRelated;
    public Tile secondRelated;
    public Tile thirdRelated;



    public SpecialBuildings(Tile original)
    {
        this.original = original;
        firstRelated.X = original.X - 1;
        firstRelated.Y = original.Y - 1;
        secondRelated.X = original.X;
        secondRelated.Y = original.Y - 2;
        thirdRelated.X = original.X;
        thirdRelated.Y = original.Y - 1;
    }
}
