using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    private int x;
    private int y;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public UnityAction<Player> onSelected;
}
