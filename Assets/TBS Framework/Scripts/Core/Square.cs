﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of square cell.
/// </summary>
public abstract class Square : Cell
{
    List<Cell> neighbours = null;
    protected static readonly Vector2[] _directions =
    {
        new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)
    };
    
    public override int GetDistance(Cell other)
    {
        return (int)(Mathf.Abs(OffsetCoord.x - other.OffsetCoord.x) + Mathf.Abs(OffsetCoord.y - other.OffsetCoord.y));
    }//Distance is given using Manhattan Norm.
    public override List<Cell> GetNeighbours(List<Cell> cells)
    {
        if(neighbours == null)
        {
            neighbours = new List<Cell>(4);
            foreach (var direction in _directions)
            {
                var neighbour = cells.Find(c => c.OffsetCoord == OffsetCoord + direction);
                if (neighbour == null) continue;

                neighbours.Add(neighbour);
            }
        }
        
        return neighbours;
    }
    //Each square cell has four neighbors, which positions on grid relative to the cell are stored in _directions constant.
    //It is totally possible to implement squares that have eight neighbours, it would require modification of GetDistance function though.
}