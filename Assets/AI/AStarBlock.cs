using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An A* pathfinding node
/// </summary>
public class AStarBlock
{
    public Vector2Int index;

    public Vector2 center;

    public float g_score, f_score;
    public AStarBlock previous;
    public int AStarIndex;

    public bool blocked = false;

    public bool passed = false;

    public AStarBlock(float centerX, float centerY, Vector2Int index)
    {
        center = new Vector2(centerX, centerY);
        g_score = int.MaxValue;
        f_score = int.MaxValue;
        this.index = index;
    }

}
