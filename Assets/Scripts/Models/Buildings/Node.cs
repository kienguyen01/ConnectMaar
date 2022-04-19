using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Structure
{
    protected List<Tile> tiles;
    public override bool IsPlacable { get => true; }
    // Start is called before the first frame update
    void Awake()
    {
        tiles = new List<Tile>();
    }
    public Node AddTile(Tile t)
    {
        tiles.Add(t);
        return this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
